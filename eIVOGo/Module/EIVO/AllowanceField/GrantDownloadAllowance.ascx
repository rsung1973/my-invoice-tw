<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/EIVO/ConfirmAllowanceDownload.ascx" TagPrefix="uc1" TagName="ConfirmAllowanceDownload" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.Module.Common" %>
<asp:Button ID="btnReset" runat="server" Text='授權下載'
    CausesValidation="false" OnClientClick='<%# doGrant.GetPostBackEventReference(((CDS_Document)((GridViewRow)Container).DataItem).InvoiceAllowance.AllowanceID.ToString()) %>' />

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
                var confirmDownload = new ASP.module_eivo_confirmallowancedownload_ascx();
                confirmDownload.InitializeAsUserControl(Page);
                confirmDownload.AllowanceID = int.Parse(arg);
                confirmDownload.Show();
            };
        }        
    }
</script>

