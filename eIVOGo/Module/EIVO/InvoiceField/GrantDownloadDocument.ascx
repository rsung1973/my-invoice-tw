<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register src="~/Module/Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.Module.Common" %>
<%@ Register src="../ConfirmDocumentDownload.ascx" tagname="ConfirmDocumentDownload" tagprefix="uc2" %>
<asp:Button ID="btnReset" runat="server" Text='授權下載'
    CausesValidation="false" OnClientClick='<%# doGrant.GetPostBackEventReference(((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceID.ToString()) %>' />

<script runat="server">

    ActionHandler doGrant;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        doGrant = (ActionHandler)Page.Items["GrantDownload"];

        if (doGrant == null)
        {
            doGrant = new ASP.module_common_actionhandler_ascx();
            Page.Controls.Add(doGrant);
            Page.Items["GrantDownload"] = doGrant;
            doGrant.DoAction = arg =>
            {
                var confirmDownload = new ASP.module_eivo_ConfirmDocumentDownload_ascx();
                confirmDownload.InitializeAsUserControl(Page);
                confirmDownload.DocID = int.Parse(arg);
                confirmDownload.Show();
            };
        }        
    }
</script>

