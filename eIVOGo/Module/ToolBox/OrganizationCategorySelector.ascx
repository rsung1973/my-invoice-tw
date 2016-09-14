<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataTextField="Expression" DataValueField="OrgaCateID"
    ondatabound="selector_DataBound" >
</asp:DropDownList>
<cc1:OrganizationCategoryDataSource ID="dsEntity" 
    runat="server">
</cc1:OrganizationCategoryDataSource>


<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        var mgr = ((OrganizationCategoryDataSource)dsEntity).CreateDataManager();
        selector.DataSource = mgr.EntityList.Where(r => r.CategoryDefinition.UserMenus.Count > 0)
            .Select(c => new { Expression = String.Format("{0}({1})", c.CategoryDefinition.Category, c.Organization.CompanyName), c.OrgaCateID });
        this.PreRender += new EventHandler(current_control_PreRender);
    }

    void current_control_PreRender(object sender, EventArgs e)
    {
        if (!_isBound)
        {
            selector.DataBind();
        }
    }
</script>