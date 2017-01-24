<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc6" %>
<%@ Register Src="ImportCounterpartBusinessList.ascx" TagName="ImportCounterpartBusinessList"
    TagPrefix="uc3" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="Business.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>

<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "匯入相對營業人資料"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table id="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
<%--            <tr class="other">
                <th width="20%" nowrap>
                    匯入格式
                </th>
                <td class="tdleft">
                    <asp:RadioButtonList ID="rbChange" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="True" OnSelectedIndexChanged="rbChange_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="~/SAM/ImportCounterpartBusiness">CSV&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="~/SAM/ImportCounterpartBusinessXml">XML&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>--%>
            <tr class="other">
                <th width="20%" nowrap>
                    匯入檔案範本
                </th>
                <td class="tdleft">
                    <a id="sample" runat="server" enableviewstate="false" href="~/Published/ImportCompany.csv">
                        <img enableviewstate="false" runat="server" id="img1" border="0" alt="" align="absMiddle"
                            src="~/images/icon_ca.gif" width="27" height="28" /></a> <font color="blue">請依據檔案中各欄位名稱填入相對應內容，每一列代表唯一家相對營業人資料，若匯入資料已存在系統，系統會以編輯方式修改原存在資料</font>
                </td>
            </tr>
            <tr class="other">
                <th width="20%" nowrap>
                    所屬集團成員
                </th>
                <td class="tdleft">
                    <%  Html.RenderPartial("~/Views/BusinessRelationship/Module/GroupMemberSelector.ascx",
                            new InputViewModel
                            {
                                Name = "CompanyID"
                            });   %>
                </td>
            </tr>
            <tr class="other">
                <th width="20%" nowrap>
                    相對營業人類別
                </th>
                <td class="tdleft">
                    <uc6:EnumSelector ID="BusinessType" FieldName="BusinessType" runat="server" SelectorIndication="請選擇" TypeName="Model.Locale.Naming+InvoiceCenterBusinessQueryType, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
                </td>
            </tr>
            <tr class="other">
                <th width="20%" nowrap>
                    相對營業人資料匯入
                </th>
                <td class="tdleft">
                    <input id="csvFile" type="file" name="csvFile" style="display:inline;" />
                    &nbsp;
                    <button type="button" onclick="$('form').prop('action','<%= Url.Action("ImportCsv","BusinessRelationship") %>').submit();">確認</button>
                </td>
            </tr>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<%  if(_uploadMgr!=null)
    { %>
<script>
    var uiImportBusiness;
    $(function () {

        uiImportBusiness = {
            $result: null,
            inquireImport: function(pageNum, onPaging) {
                $('form').ajaxForm({
                    url: "<%= Url.Action("ImportCounterpartBusinessList","BusinessRelationship") %>" + "?pageIndex=" + pageNum,
                    beforeSubmit: function () {
                    },
                    success: function (data) {
                        if (data) {
                            if (onPaging) {
                                onPaging(data);
                            } 
                        }
                    },
                    error: function () {
                    }
                }).submit();
            },
            commitImport: function (value) {
                var event = event || window.event;
                $.post('<%= Url.Action("CommitImport","BusinessRelationship") %>', { }, function (data) {
                    if (data.result) {
                        $('.tblAction').remove();
                        alert('匯入資料完成!!');
                    } else {
                        alert(data.message);
                    }
                });
            },
            cancelImport: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("CancelImport","BusinessRelationship") %>', $.param($tr.find('input,select,textarea')), function (data) {
                    if (data.result) {
                        window.location.href = '<%= Url.Action("ImportCounterpartBusiness","BusinessRelationship") %>';
                    }
                });
            },
        };
    });
</script>

<div class="border_gray" style="overflow-x: auto; max-width: 1024px;">
    <%  var recordCount = _uploadMgr.ItemCount;
        if(recordCount>0)
        { 
             Html.RenderPartial("~/Views/BusinessRelationship/Module/ImportCounterpartBusinessList.ascx"); %>

            <nav aria-label="Page navigation">
                <ul class="pagination" id="importPagination"></ul>
            </nav>
            <script>
                        $(function () {
                            var obj = $('#importPagination').twbsPagination({
                                        totalPages: <%= (recordCount+Uxnet.Web.Properties.Settings.Default.PageSize-1) / Uxnet.Web.Properties.Settings.Default.PageSize %>,
                                        totalRecordCount: <%= recordCount %>,
                                        visiblePages: 10,
                                        first: '最前',
                                        prev: '上頁',
                                        next: '下頁',
                                        last: '最後',
                                        initiateStartPageClick: false,
                                        onPageClick: function (event, page) {
                                            uiImportBusiness.inquireImport(page,function(data){
                                                var $node = $('.importList').next();
                                                $('.importList').remove();
                                                $node.before(data);
                                            });
                                        }
                                    });
                                });
            </script>
    <%  } %>
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="tblAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button onclick="uiImportBusiness.commitImport();" type="button">確定</button>
                &nbsp;&nbsp;&nbsp;
                <button onclick="uiImportBusiness.cancelImport();" type="button">取消</button>
            </td>
        </tr>
    </tbody>
</table>
<%  } %>
<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    BusinessCounterpartUploadManager _uploadMgr;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        var profile = Context.GetUser();
        _uploadMgr = (BusinessCounterpartUploadManager)profile["UploadManager"];
    }

</script>
