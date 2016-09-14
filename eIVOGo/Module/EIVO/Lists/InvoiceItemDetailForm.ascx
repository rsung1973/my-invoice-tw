<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemDetailForm.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.Lists.InvoiceItemDetailForm" %>
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
    ShowFooter="True" DataKeyNames="ItemID">
    <Columns>
        <asp:TemplateField HeaderText="項次">
            <ItemTemplate>
                <%# gvEntity.PageIndex * gvEntity.PageSize + Container.DataItemIndex + 1%>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Button ID="btnItemType" Text="選擇類別" runat="server" CausesValidation="False" OnClientClick='<%# doShow.GetPostBackEventReference(null) %>'></asp:Button>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="編號">
            <ItemTemplate>
                <input type="text" name="ItemNo" value='<%# Eval("ItemNo") %>' maxlength="40" readonly />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="ItemNo" value='<%# tempNo %>' maxlength="40" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="品名">
            <ItemTemplate>
                <input type="text" name="ItemName" value='<%# ((InvoiceProductItem)Container.DataItem).InvoiceProduct.Brief %>' maxlength="20" readonly />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="ItemName" value='<%# tempName %>' maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="UnitPrice">
            <ItemTemplate>
                <input type="text" name="UnitCost" value='<%# String.Format("{0:#,0.##}",Eval("UnitCost")) %>'
                    onblur="<%# doCreate.GetPostBackEventReference(null) %>" size="4" maxlength="20" />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="UnitCost" value='<%#Eval("Piece")!=null? Eval("Piece"):String.Format("{0:#,0.##}",tempUnitePrice) %>'
                   size="4" maxlength="20" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單位">
            <ItemTemplate>
                <input type="text" name="PieceUnit" value='<%# Eval("PieceUnit") %>' size="4" maxlength="20" readonly />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="PieceUnit" value='<%# tempUnit %>' size="4" maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量">
            <ItemTemplate>
                <input type="text" name="Piece" value='<%# Eval("Piece") %>' size="4" onblur="<%# doCreate.GetPostBackEventReference(null) %>" maxlength="20" />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="Piece" value='' size="4" maxlength="20" onblur="<%# doCreate.GetPostBackEventReference(null) %>" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="小計">
            <ItemTemplate>
                <input type="text" name="CostAmount" value='<%# String.Format("{0:#,0.##}", Eval("CostAmount")) %>' size="4" maxlength="20" readonly />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="CostAmount" value='' size="4" maxlength="20" readonly />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單一欄位備註">
            <ItemTemplate>
                <input type="text" name="Remark" value='<%# Eval("Remark") %>' onblur="<%# doCreate.GetPostBackEventReference(null) %>" maxlength="40" />
            </ItemTemplate>
            <FooterTemplate>
                <input type="text" name="Remark" value='' maxlength="40" onblur="<%# doCreate.GetPostBackEventReference(null) %>" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" Text="刪除" OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確定刪除此項貨品?",String.Format("{0}",Container.DataItemIndex)) %>' />
            </ItemTemplate>

        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table border="0" width="100%">
            <tr>
                <th>                                        
                </th>
                <th align="center">
                    編號
                </th>
                <th align="center">
                    品名
                </th>
                <th align="center">
                    單價
                </th>
                <th align="center">
                    單位
                </th>
                <th align="center">
                    數量
                </th>
                <th align="center">
                    小計
                </th>
                <th align="center">
                    單一欄位備註
                </th>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnItemType" Text="選擇類別" runat="server" CausesValidation="False" OnClientClick='<%# doShow.GetPostBackEventReference(null) %>'></asp:Button>
                </td>
                <td>
                    <input name="ItemNo" type="text" value="<%# tempNo %>" maxlength="40" readonly />
                </td>
                <td>
                    <input name="ItemName" type="text" value="<%# tempName %>" maxlength="20" readonly />
                </td>
                <td>
                    <input name="UnitCost" type="text" value="<%# tempUnitePrice %>" size="4" maxlength="20"  />
                </td>
                <td>
                    <input name="PieceUnit" type="text" value="<%# tempUnit %>" size="4" maxlength="20" readonly />
                </td>
                <td>
                    <input name="Piece" type="text" value="" size="4" onblur="<%# doCreate.GetPostBackEventReference(null) %>"  maxlength="20"/>                    
                </td>
                <td>
                    <input name="CostAmount" type="text" value="" size="4" maxlength="20" readonly />
                </td>
                <td>
                    <input name="Remark" type="text" value="" onblur="<%# doCreate.GetPostBackEventReference(null) %>" maxlength="40" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
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
