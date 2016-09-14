<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarketResourceSelectorView.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.MarketResourceSelectorView" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="MarketAttributeSelector.ascx" TagName="MarketAttributeSelector"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>
                購物平台</h2>
            <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <th width="15%">
                            購物平台
                        </th>
                        <td class="tdleft" width="35%">
                            <asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="MARKET_RESOURCE_NAME"
                                DataValueField="MARKET_RESOURCE_SN" EnableViewState="false" 
                                OnDataBound="selector_DataBound" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <th class="tdleft" width="15%">
                            <span style="color: red">*</span>平台屬性
                        </th>
                        <td class="tdleft">
                            <uc1:MarketAttributeSelector ID="MarketAttribute" runat="server" />
                            <asp:TextBox ID="MARKET_ATTR_VALUE" runat="server" EnableViewState="False"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnAddAttr" runat="server" OnClick="btnAddAttr_Click" Text="新增屬性" />
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <div id="attrResult" runat="server" visible="false" enableviewstate="false">
                <table style="margin-top: 5px" class="table01" border="0" cellspacing="0" cellpadding="0"
                    width="100%">
                    <tbody>
                        <tr>
                            <th nowrap>
                                購物平台
                            </th>
                            <asp:Repeater ID="rpAttrName" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <th nowrap>
                                        <%# loadItem((ORDERS_MARKET_ATTRIBUTE_MAPPING)Container.DataItem).MARKET_ATTR_NAME %>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                        <tr>
                            <td align="center">
                                <%# Item.MARKET_RESOURCE_NAME %>
                            </td>
                            <asp:Repeater ID="rpAttrValue" runat="server" EnableViewState="false">
                                <ItemTemplate>
                                    <td align="center">
                                        <%# ((ORDERS_MARKET_ATTRIBUTE_MAPPING)Container.DataItem).MARKET_ATTR_VALUE %>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:MarketResourceDataSource ID="dsEntity" runat="server">
</cc1:MarketResourceDataSource>
