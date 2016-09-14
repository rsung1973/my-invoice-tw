<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerPreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.BuyerPreview" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="MarketAttributeSelector.ascx" TagName="MarketAttributeSelector"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="BuyerQuery.ascx" TagName="BuyerQuery" TagPrefix="uc2" %>
<%@ Register src="../../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>
<div class="border_gray">
    <h2>
        買受人</h2>
    <table class="table01" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <td class="Head_style_a" colspan="6">
                    買受人資料
                </td>
            </tr>
            <tr>
                <th nowrap>
                    名稱
                </th>
                <th nowrap>
                    地址
                </th>
                <th nowrap>
                    電話
                </th>
                <th nowrap>
                    行動電話
                </th>
                <th nowrap>
                    電子郵件
                </th>
                <th nowrap>
                    統編
                </th>
            </tr>
            <asp:Repeater ID="rpBuyer" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_NAME %>
                        </td>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_ADDRESS %>
                        </td>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_PHONE %>
                        </td>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_MOBILE %>
                        </td>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_EMAIL %>
                        </td>
                        <td align="center">
                            <%# ((BUYER_DATA)Container.DataItem).BUYER_BAN %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
<cc1:BuyerDataSource ID="dsEntity" runat="server">
</cc1:BuyerDataSource>
<uc3:DataModelContainer ID="buyer" runat="server" />

