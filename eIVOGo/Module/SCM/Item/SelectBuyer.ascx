<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectBuyer.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.SelectBuyer" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="MarketAttributeSelector.ascx" TagName="MarketAttributeSelector"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="BuyerQuery.ascx" TagName="BuyerQuery" TagPrefix="uc2" %>
<%@ Register src="../../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc3" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>
                買受人</h2>
            <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <th width="15%">
                            買受人名稱
                        </th>
                        <td class="tdleft">
                            <uc2:BuyerQuery ID="buyerItem" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvEntity" runat="server" EnableViewState="False" Visible="False"
                AutoGenerateColumns="False" DataKeyNames="BUYER_SN" Width="100%" CssClass="table01">
                <Columns>
                    <asp:BoundField DataField="BUYER_NAME" HeaderText="名稱" SortExpression="BUYER_NAME" />
                    <asp:BoundField DataField="BUYER_ADDRESS" HeaderText="地址" SortExpression="BUYER_ADDRESS" />
                    <asp:BoundField DataField="BUYER_PHONE" HeaderText="電話" SortExpression="BUYER_PHONE" />
                    <asp:BoundField DataField="BUYER_MOBILE" HeaderText="行動電話" SortExpression="BUYER_MOBILE" />
                    <asp:BoundField DataField="BUYER_EMAIL" HeaderText="電子郵件" SortExpression="BUYER_EMAIL" />
                    <asp:BoundField DataField="BUYER_BAN" HeaderText="統編" SortExpression="BUYER_BAN" />
                </Columns>
            </asp:GridView>
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:BuyerDataSource ID="dsEntity" runat="server">
</cc1:BuyerDataSource>
<uc3:DataModelContainer ID="buyer" runat="server" />

