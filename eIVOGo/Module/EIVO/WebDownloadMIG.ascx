<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register src="../Common/CalendarInputDatePicker.ascx" tagname="ROCCalendarInput" tagprefix="uc1" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc3" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/EnumSelector.ascx" tagname="EnumSelector" tagprefix="uc7" %>
<%@ Register Src="~/Module/UI/SelectInvoiceSeller.ascx" TagPrefix="uc1" TagName="SelectInvoiceSeller" %>


<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 查詢發票" />        
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="查詢發票" />

<div class="border_gray">
    
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件</th>
        </tr>
        <% if (_userProfile.CurrentUserRole.RoleID == ((int)Naming.RoleID.ROLE_SYS)) { %>
        <tr>
            <th>統編</th>
            <td class="tdleft">
                <uc1:SelectInvoiceSeller runat="server" ID="sellerSelector" />
            </td>
        </tr>
        <% } %>
        <tr>
            <th>客戶ID / Google ID</th>
            <td class="tdleft">
                <input name="customerID" type="text" class="textfield" size="16" maxlength="20" /></td>
        </tr>
        <tr>
            <th width="20%">日期區間</th>
            <td class="tdleft">自&nbsp;<input id="appDateFrom" name="appDateFrom" type="text" class="textfield" size="10" readonly="readonly" />
                &nbsp;至&nbsp;<input id="appDateTo" name="appDateTo" type="text" class="textfield" size="10" readonly="readonly" />
            </td>
        </tr>
        <tr>
            <th>發票號碼
            </th>
            <td class="tdleft">
                <input name="invoiceNo" type="text" class="textfield" size="10" maxlength="10" />
            </td>
        </tr>
        <tr>
            <th>
                MIG格式
            </th>
            <td class="tdleft">
                <input type="radio" name="rbType" value="C0401" />C0401
                <input type="radio" name="rbType" value="C0701" />C0701
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
            var $loading = showLoadingModal();
            $("form").ajaxForm({
                url: '<%= VirtualPathUtility.ToAbsolute("~/EIVO/Helper/ListInvoiceItemForDownloadingMIG.aspx") %>',
                success: function (data) {
                    $('#result').remove();
                    $('#tblQuery').after($(data).find('#result'));
                    $loading.remove();
                }
            }).submit();
        });

        submitPagingIndex = function (pageNum) {
            $("#btnQuery").click();
        }
    });
</script>

<script runat="server">

    Model.Security.MembershipManagement.UserProfileMember _userProfile;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
    }

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

        if (!String.IsNullOrEmpty(Request["sellerID"]))
        {
            queryExpr = queryExpr.And(i => i.SellerID == int.Parse(Request["sellerID"]));
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

        if (!String.IsNullOrEmpty(Request["customerID"]))
        {
            queryExpr = queryExpr.And(i => i.InvoiceBuyer.CustomerID == Request["customerID"].Trim());
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