<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateInvoiceTrackCode.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Action.CreateInvoiceTrackCode" %>
<%@ Register Src="../../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="../Handler/EditInvoiceTrackCode.ascx" TagName="EditInvoiceTrackCode"
    TagPrefix="uc4" %>
<!--路徑名稱-->


<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <div>
            <uc3:PageAction ID="actionItem" runat="server" ItemName="首頁 > 電子發票字軌維護" />
            <!--交易畫面標題-->
            <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增發票字軌" />
            <uc4:EditInvoiceTrackCode ID="EditItem" runat="server" /></div>
    </ContentTemplate>
</asp:UpdatePanel>
