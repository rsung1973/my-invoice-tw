<%@ Control Language="C#" AutoEventWireup="true" %>
<tr>
    <th colspan="2" class="Head_style_a">查詢條件
    </th>
</tr>
<tr>
    <th>查詢項目
    </th>
    <td class="tdleft">
        <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
            <ItemTemplate>
                <input type="radio" name="urlTo" <%# DefaultName==NamingDirection[Container.ItemIndex*2] ? "checked='checked'" : "" %> onchange="window.location.href = '<%# VirtualPathUtility.ToAbsolute(NamingDirection[Container.ItemIndex*2+1]) %>';" />
                <%# NamingDirection[Container.ItemIndex*2] %>
            </ItemTemplate>
        </asp:Repeater>
    </td>
</tr>
<script runat="server">

    public String[] NamingDirection
    { get; set; }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += module_ui_urlradiodirective_ascx_PreRender;
    }

    void module_ui_urlradiodirective_ascx_PreRender(object sender, EventArgs e)
    {
        if (NamingDirection != null && NamingDirection.Length > 0)
        {
            rpList.DataSource = Enumerable.Range(0, NamingDirection.Length / 2);
            rpList.DataBind();
        }
    }

    [System.ComponentModel.Bindable(true)]
    public String DefaultName
    { get; set; }

    public void CreateDefaultQuery()
    {
        NamingDirection = new String[] { 
                "電子發票", "~/Inquiry/InquireInvoice.aspx",
                "電子折讓單", "~/Inquiry/InquireAllowance.aspx",
                "作廢電子發票", "~/Inquiry/InquireInvoiceCancellation.aspx",
                "作廢電子折讓單", "~/Inquiry/InquireAllowanceCancellation.aspx",
                "中獎發票", "~/Inquiry/InquireWinningInvoice.aspx"};
    }

    public void CreatePrintQuery()
    {
        NamingDirection = new String[] { 
                "電子發票", "~/Inquiry/PrintInvoices.aspx",
                "電子折讓單", "~/Inquiry/PrintAllowances.aspx",
                "中獎發票", "~/Inquiry/PrintWinningInvoices.aspx"};
    }

    public void CreateVoidQuery()
    {
        NamingDirection = new String[] { 
                "電子發票", "~/Inquiry/VoidInvoices.aspx"};
    }    
     

</script>
