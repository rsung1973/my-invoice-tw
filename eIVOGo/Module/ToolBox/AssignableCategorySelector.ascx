<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="selector" runat="server" EnableViewState="false" DataTextField="Category" DataValueField="CategoryID"
    ondatabound="selector_DataBound" >
</asp:DropDownList>
<cc1:CategoryDefinitionDataSource ID="dsEntity" 
    runat="server">
</cc1:CategoryDefinitionDataSource>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        var mgr = ((CategoryDefinitionDataSource)dsEntity).CreateDataManager();
        selector.DataSource = mgr.EntityList.Where(r => r.UserMenus.Count > 0 && r.OrganizationCategories.Count > 0);
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