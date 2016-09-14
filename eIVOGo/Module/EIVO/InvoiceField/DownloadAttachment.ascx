<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="~/Module/Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.Module.Common" %>
<%@ Import Namespace="Utility" %>
<asp:Repeater ID="rpList" EnableViewState="false" runat="server" Visible="<%# bindData((CDS_Document)((GridViewRow)Container).DataItem) %>">
    <ItemTemplate>
        <%# "<a onclick='" + doGrant.GetPostBackEventReference(((Attachment)Container.DataItem).KeyName) + "' href='###'>" + ((Attachment)Container.DataItem).KeyName + "</a></br>" %>
    </ItemTemplate>
</asp:Repeater>
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script runat="server">

    ActionHandler doGrant;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        doGrant = (ActionHandler)Page.Items["downloadAttachment"];

        if (doGrant == null)
        {
            doGrant = new ASP.module_common_actionhandler_ascx();
            Page.Controls.Add(doGrant);
            Page.Items["downloadAttachment"] = doGrant;
            doGrant.DoAction = arg =>
            {
                var item = dsEntity.CreateDataManager().GetTable<Attachment>().Where(i => i.KeyName == arg).FirstOrDefault();
                if (item != null)
                {
                    Response.WriteFileAsDownload(item.StoredPath);
                }
            };
        }        
    }

    bool bindData(CDS_Document docItem)
    {
        if (docItem.Attachment.Count > 0)
        {
            rpList.DataSource = docItem.Attachment;
            return true;
        }
        return false;
    }
</script>

