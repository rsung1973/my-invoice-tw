<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>

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

<% if(!models.InquiryHasError) {  %>
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "查詢結果"); %>
<div class="container border_gray" style="overflow-x: auto; max-width: 1024px;">
    <%  var recordCount = models.Items.Count();
        if (recordCount > 0)
        {   %>
            <%  Html.RenderPartial("~/Views/Module/CompanyList.ascx", models.Items);    %>
            <nav aria-label="Page navigation">
                <ul class="pagination" id="compPagination"></ul>
            </nav>
            <script>
                var pageNum = 0;
                $(function () {
                    var obj = $('#compPagination').twbsPagination({
                        totalPages: <%= (recordCount+Uxnet.Web.Properties.Settings.Default.PageSize-1) / Uxnet.Web.Properties.Settings.Default.PageSize %>,
                        visiblePages: 10,
                        initiateStartPageClick: false,
                        first: '最前',
                        prev: '上頁',
                        next: '下頁',
                        last: '最後',
                        onPageClick: function (event, page) {
                            pageNum = page;
                            inquireCompany(page ,function(data){
                                $('.companyList').html(data);
                            });
                        }
                    });
                });

                function editCompany(value) {
                    actionHandler('<%= VirtualPathUtility.ToAbsolute("~/Helper/EditCompany.aspx") %>',
                        { 'companyID': value },
                        function () {
                            inquireCompany(pageNum,function(data){
                                $('.companyList').html(data);
                            });
                        }, 800, 560);
                }

                function disableCompany(value) {
                    if (confirm('確認停用此筆資料?')) {
                        invokeAction('<%= VirtualPathUtility.ToAbsolute("~/Handling/DisableCompany") %>',
                                        { 'companyID': value },
                                        function () {
                                            inquireCompany(pageNum,function(data){
                                                $('.companyList').html(data);
                                            });
                                        });
                    }
                }

                function enableCompany(value) {
                    if (confirm('確認啟用此筆資料?')) {
                        invokeAction('<%= VirtualPathUtility.ToAbsolute("~/Handling/EnableCompany") %>',
                                                    { 'companyID': value },
                                                    function () {
                                                        inquireCompany(pageNum,function(data){
                                                            $('.companyList').html(data);
                                                        });
                                                    });
                    }
                }

                function applyWelfare(value) {

                }

                function applyCertificate(value) {
                    window.open('<%= VirtualPathUtility.ToAbsolute("~/SAM/CreateCertificateIdentity.aspx") %>' + '?companyID=' + value,'','toolbar=no,scrollbars=1,width=640,height=200');
                }

                function applyAgency(value) {
                    actionHandler('<%= VirtualPathUtility.ToAbsolute("~/Helper/ApplyProxyForIssuer.aspx") %>',
                                        { 'companyID': value }, null, 700, 360);
                }

                function applyPOS(value) {
                    actionHandler('<%= Url.Action("ApplyPOSDevice", "InvoiceBusiness") %>',
                        { 'id': value },
                        function () {
                        }, 800, 560);
                }

                function applyRelationship(value) {
                    if (confirm('確認設定此開立人為B2B營業人?')) {
                        invokeAction('<%= VirtualPathUtility.ToAbsolute("~/Handling/ApplyRelationship") %>',
                                                    { 'companyID': value },
                                                    function () {
                                                        inquireCompany(pageNum,function(data){
                                                            $('.companyList').html(data);
                                                        });
                                                    });
                    }
                }

                function inquireUser(value) {
                    $.post('<%= Url.Action("AccountIndex","Account",new { showTab = true }) %>',{'sellerID':value},function(data) {
                        $global.createTab('<%= "listUser"+DateTime.Now.Ticks %>','使用者管理',data,true);
                    });
                }

            </script>
    <%  }
        else
        {   %>
            <font color="red">查無資料!!</font>
    <%  }  %>
    <!--按鈕-->
</div>
<%--<%   if(models.Items.Count()<=10000) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <% Html.RenderPartial("~/Views/Module/PrintData.ascx"); %>
            <input type="button" value="CSV下載" name="btnCsv" class="btn" onclick="$('form').prop('action', '<%= Url.Action("DownloadCSV") %>    ').submit();" />
            <input type="button" value="Excel下載" name="btnXlsx" class="btn" onclick="$('form').prop('action', '<%= Url.Action("CreateXlsx") %>    ').submit();" />
        </td>
    </tr>
</table>
<%   } %>--%>
<% } %>
<script runat="server">
    ModelSource<Organization> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<Organization>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/OrganizationQuery/GridPage");
        
    }

</script>