<%@ Control Language="C#" AutoEventWireup="true" %>
<script>
    function createJsConfig() {

        var config = {
            height: "auto",
            width: "100%",

            autoload: true,
            paging: true,
            pageLoading: true,
            pageSize: 10,
            pageButtonCount: 10,
            pageIndex: 2,
            pagerFormat: "{first} | {prev} | {pages} | {next} | {last} &nbsp;&nbsp; {pageIndex} / {pageCount}",
            pageNextText: "下一頁",
            pagePrevText: "上一頁",
            pageFirstText: "首頁",
            pageLastText: "末頁",
            pageNavigatorNextText: "下10頁",
            pageNavigatorPrevText: "上10頁",
            noDataContent: "查無資料!!",

            controller: {},

            fields: []
        };

        return config;
    }

</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (Page.Items["jsGrid"] == null)
        {
            HtmlLink link = new HtmlLink { Href = "~/css/jsgrid.css" };
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);

            link = new HtmlLink { Href = "~/css/jsgrid-theme.css" };
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);
            
            link = new HtmlLink { Href = "~/css/eivo.css" };
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);

            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/jsgrid.js");
            Page.Header.Controls.Add(script);

            Page.Items["jsGrid"] = this;
        }
        else
        {
            this.Visible = false;
        }
    }
</script>
