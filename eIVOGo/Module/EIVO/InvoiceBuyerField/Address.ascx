<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceBuyer.Address%>
<%# bindData(Container) %>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.DataBinding += module_eivo_invoicebuyerfield_address_ascx_DataBinding;
    }

    void module_eivo_invoicebuyerfield_address_ascx_DataBinding(object sender, EventArgs e)
    {
        
    }

    public override void DataBind()
    {
        base.DataBind();
    }

    String bindData(Object contaner)
    {
        return null;
    }
</script>