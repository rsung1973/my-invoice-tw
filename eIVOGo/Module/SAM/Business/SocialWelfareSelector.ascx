<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ListControlSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Model.Locale" %>
<asp:RadioButtonList ID="selector" runat="server" RepeatColumns="3" Width="100%"
    RepeatDirection="Horizontal" EnableViewState="false" DataTextField="Expression" DataValueField="CompanyID"
    ondatabound="selector_DataBound">
</asp:RadioButtonList>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += new EventHandler(module_sam_business_socialwelfareselector_ascx_PreRender);
        var mgr = ((OrganizationDataSource)dsEntity).CreateDataManager();

        selector.DataSource = mgr.EntityList
                .Where(o => o.OrganizationCategory.Count(oc => oc.CategoryID == (int)Naming.CategoryID.COMP_WELFARE) > 0
                    && o.OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Checked)
                .Select(o => new { Expression = String.Format("{0} {1}", o.ReceiptNo, o.CompanyName), o.CompanyID });
    }

    void module_sam_business_socialwelfareselector_ascx_PreRender(object sender, EventArgs e)
    {
        if (!_isBound)
        {
            selector.DataBind();
        }
    }
</script>


