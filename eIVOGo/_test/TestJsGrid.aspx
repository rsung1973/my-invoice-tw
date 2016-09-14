<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>
<%@ Register Src="~/Module/Common/JsGrid.ascx" TagPrefix="uc1" TagName="JsGrid" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Reference Page="~/_test/PutFiles.aspx" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
        <uc1:JsGrid runat="server" ID="JsGrid" />
        <div id="jsGrid"></div>
        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
</body>
</html>
<script>

    $(function () {

        var $gridConfig = $eivo.createJsConfig();

        $gridConfig.controller = {
            loadData: function (filter) {

                var d = $.Deferred();

                $.ajax({
                    url: 'TestJsGrid.aspx?q=true&index=' + filter.pageIndex + '&size=' + filter.pageSize,
                    dataType: "json"
                }).done(function (response) {
                    d.resolve(response);
                });

                return d.promise();
            }
        };

        $gridConfig.fields = [
            { name: "InvoiceID", type: "number", width: 50 },
            { name: "InvoiceDate", type: "text", width: 200 },
            { name: "TrackCode", type: "text", width: 200 },
            { name: "No", type: "text", width: 200 }
        ];

        $("#jsGrid").jsGrid($gridConfig);

        $("#pager").on("change", function () {
            var page = parseInt($(this).val(), 10);
            $("#jsGrid").jsGrid("openPage", page);
        });

    });
</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_testjsgrid_aspx_Load;
    }

    void _test_testjsgrid_aspx_Load(object sender, EventArgs e)
    {
        if(Request["q"]=="true")
        {
            int pageIndex = 0, pageSize = 15;
            if (Request.GetRequestValue("index", out pageIndex) && pageIndex > 0)
            {
                pageIndex--;
            }
            Request.GetRequestValue("size", out pageSize);

            var items = dsEntity.CreateDataManager().EntityList;

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(serializer.Serialize(
                new
                {
                    data = items.OrderBy(i=>i.InvoiceID)
                        .Skip(pageIndex * pageSize).Take(pageSize).ToArray()
                        .Select(i => new { i.InvoiceID, InvoiceDate = i.InvoiceDate.Value.ToString("yyyy/MM/dd"), i.TrackCode, i.No }),
                    itemsCount = items.Count()
                }));
            Response.End();
        }
    }
</script>