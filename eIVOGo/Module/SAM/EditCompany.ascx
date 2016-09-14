<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCompany.ascx.cs"
    Inherits="eIVOGo.Module.SAM.EditCompany" %>
<%@ Register Src="../UI/RegisterMessage.ascx" TagName="RegisterMessage" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="EditOrganization.ascx" tagname="EditOrganization" tagprefix="uc1" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc4" %>
<!--路徑名稱-->


<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <div id="mainpage" runat="server">
            <uc3:PageAction ID="actionItem" runat="server" ItemName="店家管理-公司資料" />
            <!--交易畫面標題-->
            <h1>
                <img id="img4" runat="server" enableviewstate="false" src="~/images/icon_search.gif"
                    width="29" height="28" border="0" align="absmiddle" />店家管理-公司資料</h1>
            <uc1:EditOrganization ID="EditItem" runat="server" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += new EventHandler(module_sam_editcompany_ascx_PreRender);
    }

    void module_sam_editcompany_ascx_PreRender(object sender, EventArgs e)
    {
        EditItem.BindData();
    }
</script>


