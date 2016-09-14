<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../UI/SellerSelector.ascx" TagName="SellerSelector" TagPrefix="uc7" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc1" TagName="DataModelCache" %>

<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 開立作廢電子發票" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="開立作廢電子發票" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="20%">
                日期區間
            </th>
            <td class="tdleft">
                自&nbsp;<input id="appDateFrom" name="appDateFrom" type="text" class="textfield" size="10" readonly="readonly" />
                &nbsp;至&nbsp;<input id="appDateTo" name="appDateTo" type="text" class="textfield" size="10" readonly="readonly" />
            </td>
        </tr>
        <tr>
            <th>
                買受人統編
            </th>
            <td class="tdleft">
                <input name="receiptNo" type="text" class="textfield" size="10" maxlength="8" />
            </td>
        </tr>
        <tr>
            <th>
                發票號碼
            </th>
            <td class="tdleft">
                <input name="invoiceNo" type="text" class="textfield" size="10" maxlength="10" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tblQuery">
    <tr>
        <td class="Bargain_btn">
            <input type="button" id="btnQuery" name="btnQuery" class="btn" value="查詢" />
        </td>
    </tr>
</table>
<script>
    $(function () {
        $('#appDateFrom').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
        $('#appDateTo').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
    });

    submitPagingIndex = undefined;

    $(function () {
        $("#btnQuery").on("click", function (ev, data) {
            var $this = $(this);
            $("form").ajaxForm({
                url: '<%= VirtualPathUtility.ToAbsolute("~/EIVO/Helper/ListInvoiceItemForCancelling.aspx") %>',
                success: function (data) {
                    $('#result').remove();
                    $('#tblQuery').after($(data).find('#result'));
                }
            }).submit();
        });

        submitPagingIndex = function (pageNum) {
            $("#btnQuery").click();
        }
    });
</script>
<script runat="server">


    public static void BuildQuery(Uxnet.Web.Module.DataModel.EntityDataSource<EIVOEntityDataContext, InvoiceItem> itemList)
    {
        var Request = HttpContext.Current.Request;
        
        Model.Security.MembershipManagement.UserProfileMember userProfile = Business.Helper.WebPageUtility.UserProfile;

        System.Linq.Expressions.Expression<Func<InvoiceItem, bool>> queryExpr = f => f.InvoiceCancellation == null;

        DateTime? dateFrom;
        if (Request["appDateFrom"] != null && Request["appDateFrom"].ParseDate(out dateFrom))
        {
            queryExpr = queryExpr.And(r => r.InvoiceDate >= dateFrom);
        }
        DateTime? dateTo;
        if (Request["appDateTo"] != null && Request["appDateTo"].ParseDate(out dateTo))
        {
            queryExpr = queryExpr.And(r => r.InvoiceDate < dateTo.Value.AddDays(1));
        }


        if (!String.IsNullOrEmpty(Request["receiptNo"]))
        {
            queryExpr = queryExpr.And(i => i.InvoiceBuyer.ReceiptNo == Request["receiptNo"].Trim());
        }

        if (!String.IsNullOrEmpty(Request["invoiceNo"]))
        {
            String invoiceNo = Request["invoiceNo"].Trim();
            if (invoiceNo.Length == 10)
            {
                String trackCode = invoiceNo.Substring(0, 2);
                String no = invoiceNo.Substring(2);
                queryExpr = queryExpr.And(i => i.No == no && i.TrackCode == trackCode);
            }
            else
            {
                queryExpr = queryExpr.And(i => i.No == invoiceNo);
            }
        }


        itemList.BuildQuery = table =>
        {
            var ctx = table.Context;
            if (userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS)
            {
                return table.Where(queryExpr)
                    .OrderByDescending(i => i.InvoiceID);
            }
            else
            {
                return table.Where(queryExpr)
                    .Where(i => i.SellerID == userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                    .OrderByDescending(i => i.InvoiceID);
            }
        };

    }

</script>