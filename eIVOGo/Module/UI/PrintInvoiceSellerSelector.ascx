<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataTextField="Expression" DataValueField="CompanyID"
    ondatabound="selector_DataBound" >
</asp:DropDownList>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        var userProfile = Business.Helper.WebPageUtility.UserProfile;
        var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();
        selector.DataSource = mgr.GetTable<InvoiceItem>()
            .Select(i=>i.SellerID).Distinct()
            .Join(mgr.EntityList, b => b, o => o.CompanyID, (b, o) => o)
            .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });

        this.PreRender += new EventHandler(module_eivo_forseller_masterbusinessselector_ascx_PreRender);
    }

    void module_eivo_forseller_masterbusinessselector_ascx_PreRender(object sender, EventArgs e)
    {
        if (!_isBound)
        {
            selector.DataBind();
        }
    }

    public Boolean Postback
    {
        get { return this.selector.AutoPostBack; }
        set { this.selector.AutoPostBack = value; }
    }
</script>