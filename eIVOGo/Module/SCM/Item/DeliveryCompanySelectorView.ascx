<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.DataModel.ItemSelector" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>
                配送資訊</h2>
            <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <th class="Head_style_a" colspan="2">
                            選擇宅配公司
                        </th>
                    </tr>
                    <tr>
                        <th width="20%">
                            <span style="color: red">*</span>宅配公司
                        </th>
                        <td class="tdleft">
                            <asp:DropDownList ID="selector" runat="server" DataSourceID="dsEntity" DataTextField="DELIVERY_COMPANY_NAME"
                                DataValueField="DELIVERY_COMPANY_SN" EnableViewState="false" OnDataBound="selector_DataBound"
                                AutoPostBack="True" OnSelectedIndexChanged="selector_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:GridView ID="gvEntity" runat="server" Width="100%" CellPadding="0" EnableViewState="False"
                GridLines="None" AutoGenerateColumns="False" CssClass="table01" DataSourceID="dsView"
                DataKeyNames="DELIVERY_COMPANY_SN">
                <Columns>
                    <asp:BoundField DataField="DELIVERY_COMPANY_NAME" HeaderText="宅配公司名稱" SortExpression="DELIVERY_COMPANY_NAME" />
                    <asp:BoundField DataField="DELIVERY_COMPANY_BAN" HeaderText="統編" SortExpression="DELIVERY_COMPANY_BAN" />
                    <asp:BoundField DataField="DELIVERY_COMPANY_ADDRESS" HeaderText="地址" SortExpression="DELIVERY_COMPANY_ADDRESS" />
                    <asp:BoundField DataField="DELIVERY_COMPANY_PHONE" HeaderText="電話" SortExpression="DELIVERY_COMPANY_PHONE" />
                    <asp:BoundField DataField="DELIVERY_COMPANY_FAX" HeaderText="傳真" SortExpression="DELIVERY_COMPANY_FAX" />
                    <asp:BoundField DataField="CONTACT_NAME" HeaderText="聯絡人" SortExpression="CONTACT_NAME" />
                    <asp:BoundField DataField="CONTACT_EMAIL" HeaderText="聯絡人Email" SortExpression="CONTACT_EMAIL" />
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
            </asp:GridView>
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:DeliveryCompanyDataSource ID="dsEntity" runat="server">
</cc1:DeliveryCompanyDataSource>
<cc1:DeliveryCompanyDataSource ID="dsView" runat="server">
</cc1:DeliveryCompanyDataSource>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        dsView.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<DELIVERY_COMPANY>>(dsView_Select);
    }

    protected override void selector_DataBound(object sender, EventArgs e)
    {
        base.selector_DataBound(sender, e);
        selector.Items.Insert(0, new ListItem("請選擇", ""));
    }

    protected void selector_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvEntity.DataBind();
    }
    

    void dsView_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<DELIVERY_COMPANY> e)
    {
        if (!String.IsNullOrEmpty(SelectedValue))
        {
            e.QueryExpr = d => d.DELIVERY_COMPANY_SN == int.Parse(SelectedValue);
        }
        else
        {
            e.QueryExpr = d => false;
        }
    }
</script>