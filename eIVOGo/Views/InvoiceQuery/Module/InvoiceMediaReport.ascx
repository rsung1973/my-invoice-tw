<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Views/InquireInvoice/ByWinningNumber.ascx" TagPrefix="uc5" TagName="ByWinningNumber" %>
<%@ Register Src="~/Views/InquireInvoice/ByInvoiceNo.ascx" TagPrefix="uc5" TagName="ByInvoiceNo" %>
<%@ Register Src="~/Views/InquireInvoice/ByCancellation.ascx" TagPrefix="uc5" TagName="ByCancellation" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "發票媒體申報檔查詢／匯出"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <tr>
            <th>發票開立人
            </th>
            <td class="tdleft">
                <%  Html.RenderAction("SellerSelector", "DataFlow"); %>
                <script>
                    $(function () {
                        function getTaxNo(sellerID) {
                            $.post('<%= Url.Action("OrganizationExtension","DataEntity") %>', { 'id': sellerID }, function (data) {
                                if (data) {
                                    $('input[name="taxNo"]').val(data.TaxNo);
                                }
                            });
                        }
                        $('select[name="SellerID"]').on('change', function (evt) {
                            getTaxNo($(this).val())
                        });

                        getTaxNo($('select[name="SellerID"]').val());

                    });
                </script>
            </td>
        </tr>        
        <tr>
            <th>稅籍編號
            </th>
            <td class="tdleft">
                <input type="text" name="taxNo" />
            </td>
        </tr>
        <tr>
            <th>發票年度
            </th>
            <td class="tdleft">
                <%  Html.RenderPartial("~/Views/Shared/YearSelector.ascx"); %>
            </td>
        </tr>
        <tr>
            <th>發票期別
            </th>
            <td class="tdleft">
                <select name="periodNo">
                    <%  for (int y = 1; y < 7; y++)
                        { %>
                    <option value="<%= y %>"><%= String.Format("{0:00}-{1:00}月",y*2-1,y*2) %></option>
                    <%  } %>
                </select>
                <script>
                    $(function(){
                        $('select[name="periodNo"]').val('<%= (DateTime.Today.Month+1)/2 %>');
                    });
                </script>
            </td>
        </tr>
     </table>
    <!--表格 結束-->
</div>
<% 
//((CommonInquiry<InvoiceItem>)this.Model).RenderAlert(Html);
%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="查詢" name="btnQuery" class="btn" onclick="uiMediaReport.download();" />
        </td>
    </tr>
</table>
<script>
    var uiMediaReport;
    $(function () {
        var $postForm;
        uiMediaReport = {

            download: function () {
                if ($postForm) {
                    $postForm.remove();
                }

                $postForm = $('<form method="post" />').prop('action', '<%= Url.Action("InquireInvoiceMedia","InvoiceQuery") %>')
                    .css('display', 'none').appendTo($('body'));

                $('#theForm').serializeArray().forEach(function (item, index) {
                    $('<input type="hidden">')
                        .prop('name', item.name).prop('value', item.value)
                        .appendTo($postForm);
                });
                $postForm.submit();
                //showLoading();
            },
        };
    });
</script>

<script runat="server">


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
</script>

