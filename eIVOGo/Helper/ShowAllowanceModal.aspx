<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Utility" %>
<form runat="server" id="theForm">
    <div id="mainContent">
        <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="發票折讓" />
        <div class="border_gray">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="100" nowrap="nowrap">折讓日期</th>
                    <td class="tdleft">
                        <%# ValueValidity.ConvertChineseDateString(_allowance.AllowanceDate) %>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="100" colspan="4" class="Head_style_a">原開立銷貨發票單位</th>
                </tr>
                <tr>
                    <th width="100" nowrap="nowrap">統一編號</th>
                    <td width="30%" class="tdleft">
                        <%# _allowance.InvoiceAllowanceSeller.ReceiptNo %>
                    </td>
                    <th width="100" nowrap="nowrap">名　　稱</th>
                    <td class="tdleft">
                        <%# _allowance.InvoiceAllowanceSeller.CustomerName %>
                    </td>
                </tr>
                <tr>
                    <th width="100">營業所在地址</th>
                    <td colspan="3" class="tdleft">
                        <%# _allowance.InvoiceAllowanceSeller.Address %>
                    </td>
                </tr>
            </table>
            <br />
            <asp:gridview id="gvEntity" runat="server" autogeneratecolumns="False" width="100%"
                gridlines="None" cellpadding="0" cssclass="table01" clientidmode="Static" enableviewstate="false"
                onrowcreated="gvEntity_RowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="聯式">
                            <ItemTemplate>
                                <%--<%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo.Equals("0000000000") ? "二" : "三"%>--%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="年">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Year%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="月">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Month%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="日">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceDate.Value.Day%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="字軌">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceNo.Substring(0,2)%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="號碼">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.InvoiceNo.Substring(2)%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="品名">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.OriginalDescription%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <%# ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Piece%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單價">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.UnitCost) %></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金額&lt;br/&gt;(不含稅之進貨額)">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", (((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Amount - ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Tax))%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="營業稅額">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0.00}", ((InvoiceAllowanceDetail)Container.DataItem).InvoiceAllowanceItem.Tax) %></ItemTemplate>
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
            <!--表格 結束-->
        </div>
    </div>
</form>
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script runat="server">

    CDS_Document _item;
    int _docID;
    InvoiceAllowance _allowance;

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
        if (_item != null && _item.InvoiceAllowance != null)
        {
            _allowance = _item.InvoiceAllowance;
            gvEntity.DataSource = _allowance.InvoiceAllowanceDetails;
            this.DataBind();
        }
        else
        {
            this.Visible = false;
        }
    }

    protected void gvEntity_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();
            HeaderCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            HeaderCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#c99040");
            HeaderCell.Text = "開立發票";
            HeaderCell.ColumnSpan = 6;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            HeaderCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#c99040");
            HeaderCell.Text = "退貨或折讓內容";
            HeaderCell.ColumnSpan = 5;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            this.gvEntity.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }    
    
</script>
