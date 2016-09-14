<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="System.IO" %>
<%@ Register src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" tagname="UrlRadioDirective" tagprefix="uc2" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
    <div style="vertical-align:top;">
        資料修改:<textarea name="dataArea" cols="100" rows="20"><%= Request["dataArea"] %></textarea>
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="更新" />
    </div>
        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_testall_aspx_Load;
        
    }


    void _test_testall_aspx_Load(object sender, EventArgs e)
    {
        if(Request["dataArea"]!=null)
        {
            var mgr = dsEntity.CreateDataManager();
            int count = 0;
            using (StringReader sr = new StringReader(Request["dataArea"]))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    String[] cols = line.Split(',');
                    if (cols.Length>1 && cols[0].Length == 10 && cols[1].Length>0)
                    {
                        var item = mgr.EntityList.Where(i => i.TrackCode == cols[0].Substring(0, 2)
                            && i.No == cols[0].Substring(2)).FirstOrDefault();
                        if (item != null && item.InvoiceBuyer != null)
                        {
                            item.InvoiceBuyer.EMail = cols[1].Trim();
                            mgr.SubmitChanges();
                            count++;
                        }
                    }
                }
            }
            this.AjaxAlert(count + "筆資料已更新!!");
        }
    }

</script>
