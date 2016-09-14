<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModifyInvoiceTrackCode.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Action.ModifyInvoiceTrackCode" %>
<%@ Register Src="../../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Src="../Handler/EditInvoiceTrackCode.ascx" TagName="EditInvoiceTrackCode"
    TagPrefix="uc4" %>
<!--路徑名稱-->
<div>
    <uc3:PageAction ID="actionItem" runat="server" ItemName="首頁 > 電子發票字軌維護" />
    <!--交易畫面標題-->
    <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="修改發票字軌" />
    <uc4:EditInvoiceTrackCode ID="EditItem" runat="server" />
</div>
