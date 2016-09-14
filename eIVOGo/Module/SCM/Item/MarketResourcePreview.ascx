<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarketResourcePreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.MarketResourcePreview" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="MarketAttributeSelector.ascx" TagName="MarketAttributeSelector"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<asp:Repeater ID="rpAttr" runat="server" EnableViewState="false">
    <ItemTemplate>
        <tr>
            <th width="20%">
                <%# loadItem((ORDERS_MARKET_ATTRIBUTE_MAPPING)Container.DataItem).MARKET_ATTR_NAME %>
            </th>
            <td class="tdleft">
                <%# ((ORDERS_MARKET_ATTRIBUTE_MAPPING)Container.DataItem).MARKET_ATTR_VALUE %>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
<cc1:MarketResourceDataSource ID="dsEntity" runat="server">
</cc1:MarketResourceDataSource>
