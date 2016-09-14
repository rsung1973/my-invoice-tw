<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadAppconfig.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Handler.DownloadAppconfig" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc1" %>
<%@ Register Src="../../Common/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc3:ActionHandler ID="doConfirm" runat="server" />
<uc1:DataModelCache ID="modelItem" runat="server" KeyName="Config" />
