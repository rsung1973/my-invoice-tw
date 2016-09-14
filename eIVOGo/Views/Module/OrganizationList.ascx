<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>

<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" ID="gridInit" FieldName="fields" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
<script>

    var fields = [
        {
            "name": "CompanyName",
            "type": "text",
            "title": "店家名稱",
            "width": "160",
            "align": "left"
        },
        {
            "name": "ReceiptNo",
            "type": "text",
            "title": "統一編號",
            "width": "80",
            "align": "center"
        },
        {
            "name": "UndertakerName",
            "type": "text",
            "title": "負責人姓名",
            "width": "120",
            "align": "left"
        },
        {
            "name": "ContactEmail",
            "type": "text",
            "title": "電子郵件",
            "width": "160",
            "align": "left",
            itemTemplate: function (value, item) {
                return $('<pre>')
                    .html(value);
            }
        },
        {
            "name": "Status",
            "type": "text",
            "title": "狀態",
            "width": "50",
            "align": "left"
        },
        {
            "name": "CompanyID",
            "type": "text",
            "title": "管理",
            "width": "120",
            "align": "center",
            itemTemplate: function (value, item) {
                var $d = $('<div>');
                if(item.Editable) {
                    var $button = $('<input type="button" class="btn" value="編輯" name="btnEdit">');
                    $button.on('click',function(evt) {
                        actionHandler('<%= VirtualPathUtility.ToAbsolute("~/Helper/EditCompany.aspx") %>',
                            { 'companyID': value },
                            function () {
                                $("#jsGrid").jsGrid("loadData", null);
                            }, 800, 560);
                    });
                    $d.append($button);
                    $button.after($.parseHTML('&nbsp;&nbsp;&nbsp;'));
                }
                if(item.Enabled) {
                    var $button = $('<input type="button" class="btn" value="停用" name="btnDelete">');
                    $button.on('click',function(evt) {
                        invokeAction('<%= VirtualPathUtility.ToAbsolute("~/Handling/DisableCompany") %>',
                            { 'companyID': value },
                            function () {
                                $("#jsGrid").jsGrid("loadData", null);
                            });
                    });
                    $d.append($button);
                    $button.after($.parseHTML('&nbsp;&nbsp;&nbsp;'));
                } else {
                    var $button = $('<input type="button" class="btn" value="啟用" name="btnActivate">');
                    $button.on('click',function(evt) {
                        invokeAction('<%= VirtualPathUtility.ToAbsolute("~/Handling/EnableCompany") %>',
                            { 'companyID': value },
                            function () {
                                $("#jsGrid").jsGrid("loadData", null);
                            });
                    });
                    $d.append($button);
                    $button.after($.parseHTML('&nbsp;&nbsp;&nbsp;'));
                }
                
                if(item.ApplyProxy) {

                    var $button = $('<input type="button" class="btn" value="設定發票代理店家" name="btnProxy">');
                    $button.on('click',function(evt) {
                        actionHandler('<%= VirtualPathUtility.ToAbsolute("~/Helper/ApplyProxyForIssuer.aspx") %>',
                            { 'companyID': value }, null, 700, 360);
                    });
                    $d.append($button);
                    $button.after($.parseHTML('&nbsp;&nbsp;&nbsp;'));
                }

                var $button = $('<input type="button" class="btn" value="建立憑證資訊" name="btnCASetup">');
                $button.on('click',function(evt) {
                    ///doEdit.GetPostBackEventReference(String.Format("{0}", Eval(gvEntity.DataKeyNames[0])))
                });
                $d.append($button);

                return $d;
            },
            footerTemplate: function () { return ""; }
        }
            ];

</script>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gridInit.DataSourceUrl = ((ModelSource<Organization>)Model).DataSourcePath;
        gridInit.GetRecordCount = () =>
            {
                return ((ModelSource<Organization>)Model).Items.Count();
            };
        gridInit.AllowPaging = ((ModelSource<Organization>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<Organization>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
