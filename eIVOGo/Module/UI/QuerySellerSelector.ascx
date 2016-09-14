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
    UserProfileMember _userProfile;
    protected override void OnInit(EventArgs e)
    {
        _userProfile = WebPageUtility.UserProfile;
        base.OnInit(e);
        var userProfile = Business.Helper.WebPageUtility.UserProfile;
        var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();
        
        if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理)
        {
            selector.DataSource = mgr.EntityList

                .Join(mgr.GetTable<InvoiceIssuerAgent>(), o => o.CompanyID, a => a.AgentID, (o, a) => o);
                
        }
        else
        {

            selector.DataSource = mgr.EntityList.Where(o => o.OrganizationCategory
            .Any(c => c.CategoryID == (int)Naming.B2CCategoryID.Google台灣
                || c.CategoryID == (int)Naming.B2CCategoryID.店家
                || c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
                || c.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理))
            .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });
            
        }
        this.PreRender += new EventHandler(module_eivo_forseller_masterbusinessselector_ascx_PreRender);
    }

    void module_eivo_forseller_masterbusinessselector_ascx_PreRender(object sender, EventArgs e)
    {
        if (!_isBound)
        {
            var userProfile = Business.Helper.WebPageUtility.UserProfile;
            var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();

            if (_userProfile.CurrentUserRole.OrganizationCategory.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理)
            {
                selector.DataSource = mgr.EntityList

                    .Join(mgr.GetTable<InvoiceIssuerAgent>().Where(a => a.AgentID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID)
                    , o => o.CompanyID, a => a.IssuerID, (o, a) => o)
                    .Concat(mgr.EntityList.Where(o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID))
                    .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });

            }
            else
            {

                selector.DataSource = mgr.EntityList.Where(o => o.OrganizationCategory
                .Any(c => c.CategoryID == (int)Naming.B2CCategoryID.Google台灣
                    || c.CategoryID == (int)Naming.B2CCategoryID.店家
                    || c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
                    || c.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理))
                .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });

            }
            //selector.DataSource = this.Select().OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });
            selector.DataBind();
           
        }
    }

    public Boolean Postback
    {
        get { return this.selector.AutoPostBack; }
        set { this.selector.AutoPostBack = value; }
    }
</script>
