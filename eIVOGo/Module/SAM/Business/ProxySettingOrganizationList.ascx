<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProxySettingOrganizationList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Business.ProxySettingOrganizationList" EnableViewState="false"%>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagPrefix="uc1" TagName="ActionHandler" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/InvoiceAgentSelector.ascx" TagName="InvoiceAgentSelector"
    TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Utility" %>
<%@ Register Src="../../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc3" %>
<asp:Button ID="btnHidden" runat="server" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <!--設定 代理店家-->
        <div id="divPurchase" runat="server">
            <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="開立發票傳送代理" />
            <div id="border_gray">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                    <tr>
                        <th>
                            發票開立人
                        </th>
                        <td class="tdleft">
                            <%# _entity.CompanyName + "(" +  _entity.ReceiptNo + ")" %>
                        </td>
                    </tr>
                    <tr>
                        <th width="100">
                            發票傳送代理單位
                        </th>
                        <td class="tdleft">
                            <uc2:InvoiceAgentSelector ID="agentSelector" runat="server" />
                        </td>
                </table>
            </div>
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="Bargain_btn">
                    <span class="table-title">
                        <asp:Button ID="btnOK" CssClass="btn" runat="server" Text="確定" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClose" CssClass="btn" runat="server" Text="關閉" />
                    </span>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
<ajaxtoolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHidden"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="btnClose"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<cc1:OrganizationDataSource ID="dsEntity" runat="server" />
<uc1:ActionHandler runat="server" ID="doConfirm" />
<uc1:ActionHandler runat="server" ID="doCancel" />
<uc3:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += module_sam_business_proxysettingorganizationlist_ascx_PreRender;
        btnClose.OnClientClick = doCancel.GetPostBackEventReference(null);
        btnOK.OnClientClick = doConfirm.GetPostBackEventReference(null);
        this.QueryExpr = a => a.CompanyID == IssureID;
    }

    void module_sam_business_proxysettingorganizationlist_ascx_PreRender(object sender, EventArgs e)
    {
        if (this.Visible)
            this.ModalPopupExtender.Show();

    }

    public override void BindData()
    {
        base.BindData();
        if (_entity != null)
        {
            agentSelector.SelectedValues.Clear();
            agentSelector.SelectedValues.AddRange(_entity.AsInvoiceInsurer.Select(i => i.AgentID.ToString()));
        }
    }
</script>
