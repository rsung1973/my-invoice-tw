<%@ Control Language="C#" AutoEventWireup="true" %>
<asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
<%--        <asp:ScriptReference Path="~/Scripts/jquery-1.11.3.js" />
        <asp:ScriptReference Path="~/Scripts/jquery-ui-1.11.3.js" />
        <asp:ScriptReference Path="~/Scripts/bootstrap.js" />
        <asp:ScriptReference Path="~/Scripts/jquery.ui.datepicker-zh-TW.js" />
        <asp:ScriptReference Path="~/Scripts/jquery.form.js" />--%>
    </Scripts>
</asp:ToolkitScriptManager>
<script>
    var $currentModal;
    $(function () {
        $('input[type="button"]').addClass('btn');
        //$('form').on('submit', function () {
        //    var $currentModal = showLoadingModal();
        //});
    });
</script>
<asp:PlaceHolder ID="plScript" runat="server">
    <script>
        function showInvoiceModal(docID) {
            var element = event.target;
            $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowInvoiceModal.aspx")%>', 'docID=' + docID, function (html) {
                //            $(element).after($(html).find("#mainContent"));
                $(html).find("#mainContent").dialog({
                    width : 640,
                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            });
        }

        function showAllowanceModal(docID) {
            var element = event.target;
            $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowAllowanceModal.aspx")%>', 'docID=' + docID, function (html) {
                $(html).find("#mainContent").dialog({
                    width: 640,
                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            });
        }

        function showLoadingModal() {
            var $body = $('body');
            //var $modal = $('<div style="position: absolute;top: 0;left: 0;z-index: 1000;width: 100%;opacity: 0.5;filter: Alpha(opacity=50);background: gray;"><img src="<%= VirtualPathUtility.ToAbsolute("~/images/loading.gif") %>" style="position: absolute;top:0;left:0;right:0;bottom:0;margin:auto;max-height:100%;max-width:100%;"></div>');
            var $modal = $('<div style="position: absolute;top: 0;left: 0;z-index: 1000;width: 100%;opacity: 0.5;filter: Alpha(opacity=50);background: gray;"><img src="<%= VirtualPathUtility.ToAbsolute("~/images/loading.gif") %>" style="position: absolute;"></div>');
            var $img = $modal.find('img');
            $modal.css('height', $body.css('height'));
            $img.css('top', $body.scrollTop() + screen.height / 2 - 48);
            $img.css('left', screen.width / 2 - 24);
            $body.append($modal);
            return $modal;
        }

        function actionHandler(url, data, done, width, height) {
            $('<div>').load(url, data, function (evt) {
                var $this = $(this);
                $this.dialog({
                    width: width,
                    height: height,
<%--                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close").remove();
                            }
                        }
                    ],--%>
                    close: function (evt, ui) {
                        $this.remove();
                        if (typeof (done) == 'function') {
                            done();
                        }
                    }
                });
            });
        }

        function invokeAction(url, data, done) {
            $.post(url, data, function (html) {
                console.log(html);
                var $s = $(html);
                $('body').append($s);
                $s.remove();
                if (typeof (done) == 'function') {
                    done();
                }
            });
        }
    </script>
</asp:PlaceHolder>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //HtmlGenericControl script = new HtmlGenericControl("script");
        //script.Attributes["type"] = "text/javascript";
        //script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/jquery-1.11.3.js");
        //Page.Header.Controls.Add(script);
        //script = new HtmlGenericControl("script");
        //script.Attributes["type"] = "text/javascript";
        //script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/jquery-ui-1.11.3.js");
        //Page.Header.Controls.Add(script);
        //script = new HtmlGenericControl("script");
        //script.Attributes["type"] = "text/javascript";
        //script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/bootstrap.js");
        //Page.Header.Controls.Add(script);
        //script = new HtmlGenericControl("script");
        //script.Attributes["type"] = "text/javascript";
        //script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/jquery.ui.datepicker-zh-TW.js");
        //Page.Header.Controls.Add(script);
        //script = new HtmlGenericControl("script");
        //script.Attributes["type"] = "text/javascript";
        //script.Attributes["src"] = VirtualPathUtility.ToAbsolute("~/Scripts/jquery.form.js");
        //Page.Header.Controls.Add(script);

        UserControl script = (UserControl)this.LoadControl("CommonScriptInclude.ascx");
        Page.Header.Controls.Add(script);
        
    }
</script>