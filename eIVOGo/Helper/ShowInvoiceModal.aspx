<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Utility" %>
<form runat="server" id="theForm">
    <div id="mainContent">
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="發票" />
        <div class="border_gray">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="100">發票號碼
                    </th>
                    <td class="tdleft">
                        <%# _invoice.TrackCode + _invoice.No %>
                    </td>
                    <th width="100" nowrap="nowrap">個人識別碼
                    </th>
                    <td class="tdleft">
                        <%# _invoice.InvoiceBuyer.IsB2C() ? _invoice.InvoiceBuyer.Name : "" %>
                    </td>
                </tr>
                <tr>
                    <th width="100">未稅金額
                    </th>
                    <td class="tdleft">
                        <%# String.Format("{0:0,0.00}", _invoice.InvoiceAmountType.SalesAmount) %>
                    </td>
                    <th width="100">稅 額
                    </th>
                    <td class="tdleft">
                        <%# String.Format("{0:0,0.00}", _invoice.InvoiceAmountType.TaxAmount) %>
                    </td>
                </tr>
                <tr>
                    <th width="100">含稅金額
                    </th>
                    <td class="tdleft">
                        <%# String.Format("{0:0,0.00}", _invoice.InvoiceAmountType.TotalAmount) %>
                    </td>
                    <th width="100">日 期
                    </th>
                    <td class="tdleft">
                        <%# ValueValidity.ConvertChineseDateString(_invoice.InvoiceDate) %>
                    </td>
                </tr>
                <tr>
                    <th width="100">買受人統編
                    </th>
                    <td colspan="3" class="tdleft">
                        <%# _invoice.InvoiceBuyer.IsB2C() ? "" : _invoice.InvoiceBuyer.ReceiptNo %>
                    </td>
                </tr>
            </table>
            <br />
            <asp:gridview id="gvEntity" runat="server" autogeneratecolumns="False" width="100%"
                gridlines="None" cellpadding="0" cssclass="table01" clientidmode="Static" enableviewstate="false">
                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名">
                            <ItemTemplate>
                                <%# Eval("Brief")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <%# Eval("Piece")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單位">
                            <ItemTemplate>
                                <%# Eval("PieceUnit")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單價">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}",Eval("UnitCost"))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金額">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}",Eval("CostAmount"))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="備註">
                            <ItemTemplate>
                                <%# Eval("Memo")%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle />
                    <PagerStyle HorizontalAlign="Center" />
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <AlternatingRowStyle CssClass="OldLace" />
                    <RowStyle />
                    <EditRowStyle />
                </asp:gridview>
        </div>
    </div>
</form>
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script runat="server">

    CDS_Document _item;
    int _docID;
    InvoiceItem _invoice;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += helper_editinvoicebuyer_aspx_PreRender;

        if (Request["docID"] != null && int.TryParse(Request["docID"], out _docID))
        {
            _item = dsEntity.CreateDataManager().EntityList.Where(d => d.DocID == _docID).FirstOrDefault();
        }
    }

    void helper_editinvoicebuyer_aspx_PreRender(object sender, EventArgs e)
    {
        if (_item != null && _item.InvoiceItem != null)
        {
            _invoice = _item.InvoiceItem;
            gvEntity.DataSource = _invoice.InvoiceDetails.Select(d => new
                               {
                                   Brief = d.InvoiceProduct.Brief,
                                   Piece = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().Piece,
                                   PieceUnit = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().PieceUnit,
                                   UnitCost = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().UnitCost,
                                   CostAmount = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().CostAmount,
                                   Memo = d.InvoiceProduct.InvoiceProductItem.Where(ipt => ipt.ProductID == d.ProductID).FirstOrDefault().Remark
                               });
            this.DataBind();
        }
        else
        {
            this.Visible = false;
        }
    }
    
</script>
