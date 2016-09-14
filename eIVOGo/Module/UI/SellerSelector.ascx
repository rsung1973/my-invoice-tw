<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelectorV2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.Locale" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataTextField="Expression" DataValueField="CompanyID">
</asp:DropDownList>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        var userProfile = Business.Helper.WebPageUtility.UserProfile;
        if (SelectAll)
        {
            this.SelectorIndication = "全部";
        }

        var mgr = dsEntity.CreateDataManager();
        IQueryable<Organization> orgItems = mgr.GetTable<Organization>();
        selector.DataSource = orgItems.Where(
            o => o.OrganizationCategory.Any(
                c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER || c.CategoryID == (int)Naming.CategoryID.COMP_VIRTUAL_CHANNEL))
                .OrderBy(o => o.ReceiptNo)
                .Select(o => new
                {
                    Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                    o.CompanyID
                });
                
        //selector.DataSource = mgr.GetTable<InvoiceItem>()
        //    .Select(i=>i.SellerID).Distinct()
        //    .Join(mgr.EntityList, b => b, o => o.CompanyID, (b, o) => o)
        //    .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });

    }

    protected override void ItemSelectorV2_PreRender(object sender, EventArgs e)
    {
        selector.DataBind();
        base.ItemSelectorV2_PreRender(sender, e);
    }
    
    [System.ComponentModel.Bindable(true)]
    public bool SelectAll
    {
        get;
        set;
    }
</script>
