<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.UI.InvoiceQuerySellerSelector"
    CodeBehind="InvoiceQuerySellerSelector.ascx.cs" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.Security.MembershipManagement" %>
 <%@ Import Namespace="Business.Helper" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataTextField="Expression"
    DataValueField="CompanyID" OnDataBound="selector_DataBound">
</asp:DropDownList>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var userProfile = WebPageUtility.UserProfile;
        var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();
        IQueryable<Organization> orgItems = mgr.GetTable<Organization>();
        switch ((Naming.CategoryID)userProfile.CurrentUserRole.OrganizationCategory.CategoryID)
        {
            case Naming.CategoryID.COMP_SYS:
                selector.DataSource = orgItems.Where(
                    o => o.OrganizationCategory.Any(
                        c => c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER 
                            || c.CategoryID == (int)Naming.CategoryID.COMP_VIRTUAL_CHANNEL
                            || c.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW
                            || c.CategoryID == (int)Naming.CategoryID.COMP_INVOICE_AGENT))
                        .OrderBy(o => o.ReceiptNo)
                        .Select(o => new
                        {
                            Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                            o.CompanyID
                        });                
                break;
            case Naming.CategoryID.COMP_INVOICE_AGENT:
                selector.DataSource = mgr.GetQueryByAgent(userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                        .OrderBy(o => o.ReceiptNo)
                        .Select(o => new
                        {
                            Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                            o.CompanyID
                        });              

                break;
                
            case Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER:
            case Naming.CategoryID.COMP_VIRTUAL_CHANNEL:
            case Naming.CategoryID.COMP_E_INVOICE_GOOGLE_TW:
                selector.DataSource = orgItems.Where(
                    o => o.CompanyID == userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                        .Select(o => new
                        {
                            Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName),
                            o.CompanyID
                        });

                break;

            default:
                break;
        }  

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
