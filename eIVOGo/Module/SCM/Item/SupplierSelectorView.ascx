<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.Item.SupplierSelector" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>
                供應商</h2>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
                <tr>
                    <th colspan="2" class="Head_style_a">
                        選擇供應商
                    </th>
                </tr>
                <tr>
                    <th width="15%">
                        供 應 商
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="selector" runat="server" EnableViewState="false" 
                            AutoPostBack="True" ondatabound="selector_DataBound" DataSourceID="dsEntity">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvSupplier" runat="server" Width="100%" CellPadding="0"
                EnableViewState="False" GridLines="None" AutoGenerateColumns="False" 
                CssClass="table01">
                <Columns>
                    <asp:TemplateField HeaderText="供應商名稱">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_NAME %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="統編">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_BAN%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="地址">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_ADDRESS%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="電話">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_PHONE%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="傳真">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_FAX%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="聯絡人">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).SUPPLIER_CONTACT_NAME%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="聯絡人Email">
                        <ItemTemplate>
                            <%# ((SUPPLIER)Container.DataItem).CONTACT_EMAIL%></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
            </asp:GridView>
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:SupplierDataSource ID="dsEntity" runat="server">
</cc1:SupplierDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        selector.DataBound += new EventHandler(Selector_DataBound);
    }

    void Selector_DataBound(object sender, EventArgs e)
    {
        selector.Items.Insert(0, new ListItem("請選擇", ""));
        
        if (!String.IsNullOrEmpty(selector.SelectedValue))
        {
            gvSupplier.DataSource = ((SupplierDataSource)dsEntity).CreateDataManager().EntityList.Where(s => s.SUPPLIER_SN == int.Parse(selector.SelectedValue));
            gvSupplier.DataBind();
        }
    }
</script>
