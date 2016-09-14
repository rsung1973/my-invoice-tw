<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<asp:Repeater ID="rpList" runat="server" EnableViewState="false">
    <ItemTemplate>
        <a onclick='javascript:window.location.href = "<%# VirtualPathUtility.ToAbsolute("~/Helper/DownloadAttachment.ashx") + "?keyName=" + ((Attachment)Container.DataItem).KeyName %>";' href="#"><%# ((Attachment)Container.DataItem).KeyName %></a></br>
    </ItemTemplate>
</asp:Repeater>
<script runat="server">

    [System.ComponentModel.Bindable(true)]
    public CDS_Document DataItem
    { get; set; }

    public override void DataBind()
    {
        if (DataItem == null)
            DataItem = Page.GetDataItem() as CDS_Document;
        if (DataItem != null)
        {
            rpList.DataSource = DataItem.Attachment;
            base.DataBind();
        }
    }

</script>