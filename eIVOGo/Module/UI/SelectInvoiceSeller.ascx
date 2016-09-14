<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Model.Locale" %>
<select name="<%= _requestName %>">
    <asp:Repeater ID="rpList" runat="server">
        <ItemTemplate>
            <option value="<%# Eval("CompanyID") %>"><%# Eval("Expression") %></option>
        </ItemTemplate>
    </asp:Repeater>
</select>
<script>
    $(function () {
        $('select[name="<%= _requestName %>"]').val('<%= Request[_requestName] %>');
    });
</script>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        var userProfile = Business.Helper.WebPageUtility.UserProfile;
        var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();
        rpList.DataSource = mgr.EntityList.Where(o => o.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.B2CCategoryID.Google台灣 || c.CategoryID == (int)Naming.B2CCategoryID.店家 || c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號))
            .OrderBy(o => o.ReceiptNo).Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });

        this.PreRender += new EventHandler(module_eivo_forseller_masterbusinessselector_ascx_PreRender);
    }

    void module_eivo_forseller_masterbusinessselector_ascx_PreRender(object sender, EventArgs e)
    {
        rpList.DataBind();
    }

    String _requestName = "sellerID";

    [System.ComponentModel.Bindable(true)]
    public String RequestName
    {
        get
        {
            return _requestName;
        }
        set
        {
            if (!String.IsNullOrEmpty(value))
                _requestName = value;
        }
    }    

</script>