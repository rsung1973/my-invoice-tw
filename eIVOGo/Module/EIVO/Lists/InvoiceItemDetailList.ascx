<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.Lists.InvoiceItemDetailForm" %>
<%@ Register Src="InvoiceItemTypeList.ascx" TagPrefix="uc" TagName="InvoiceItemTypeList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc3" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Utility" %>
<br />
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" 
    AllowPaging="false" GridLines="None" CellPadding="0" EnableViewState="False" CssClass="table01"
    ShowFooter="false" DataKeyNames="ItemID">
    <Columns>
        <asp:TemplateField HeaderText="項次">
            <ItemTemplate>
                <%# gvEntity.PageIndex * gvEntity.PageSize + Container.DataItemIndex + 1%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <asp:Button ID="btnItemType" Text="選擇類別" runat="server" CausesValidation="False" OnClientClick='<%# doShow.GetPostBackEventReference(null) %>'></asp:Button>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="編號">
            <ItemTemplate>
                <%# Eval("ItemNo") %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="ItemNo" value='<%# tempNo %>' maxlength="40" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="品名">
            <ItemTemplate>
                <%# ((InvoiceProductItem)Container.DataItem).InvoiceProduct.Brief %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="ItemName" value='<%# tempName %>' maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="UnitPrice">
            <ItemTemplate>
                <%# String.Format("{0:#,0.##}",Eval("UnitCost")) %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="UnitCost" value='<%# String.Format("{0:#,0.##}",tempUnitePrice) %>' size="4" maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單位">
            <ItemTemplate>
                <%# Eval("PieceUnit") %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="PieceUnit" value='<%# tempUnit %>' size="4" maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量">
            <ItemTemplate>
                <%# Eval("Piece") %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="Piece" value='' size="4" maxlength="20" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="小記">
            <ItemTemplate>
                <%# String.Format("{0:#,0.##}", Eval("CostAmount")) %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="CostAmount" value='' size="4" maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單一欄位備註">
            <ItemTemplate>
                <%# Eval("Remark") %>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                <input type="text" name="Remark" value='' maxlength="40" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ItemID" SortExpression="ItemID" Visible="false">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確定刪除此項貨品?",String.Format("{0}",Container.DataItemIndex)) %>' />
            </ItemTemplate>
            <FooterTemplate>
                <asp:Button ID="btnCreate" runat="server" Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle CssClass="total-count" />
    <PagerStyle HorizontalAlign="Center" />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
        <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
    </PagerTemplate>
</asp:GridView>

<cc1:InvoiceProductItemDataSource ID="dsEntity" runat="server">
</cc1:InvoiceProductItemDataSource>
<uc1:ActionHandler ID="doCreate" runat="server" />
<uc1:ActionHandler ID="doDelete" runat="server" />
<uc1:ActionHandler ID="doShow" runat="server" />
<uc3:DataModelCache ID="modelItem" runat="server" KeyName="LcItem" />
<uc:InvoiceItemTypeList ID="InvoiceItemTypeList1" runat="server" Visible="false" />
