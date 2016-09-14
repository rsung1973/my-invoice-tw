<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ListControlSelectorV2" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:ListBox ID="selector" runat="server" EnableViewState="false" DataTextField="Expression" DataValueField="CompanyID" SelectionMode="Multiple" >
</asp:ListBox>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">

    protected override void ListControlSelectorV2_PreRender(object sender, EventArgs e)
    {
        var mgr = dsEntity.CreateDataManager();
        selector.DataSource = mgr.GetTable<Organization>()
            .Where(o => o.OrganizationCategory.Count(c => c.CategoryID == (int)Model.Locale.Naming.B2CCategoryID.開立發票店家代理) > 0)
            .OrderBy(o => o.ReceiptNo)
            .Select(o => new { Expression = String.Format("({0}) {1}", o.ReceiptNo, o.CompanyName), CompanyID = o.CompanyID });
        selector.DataBind();

        base.ListControlSelectorV2_PreRender(sender, e);
    }
   
</script>
