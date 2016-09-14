<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerDataList.ascx.cs" Inherits="eIVOGo.Module.Inquiry.BuyerDataList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Register src="~/Module/Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />

<asp:Panel ID="Panel1" runat="server" Style="display: none; width:750px; background-color: #ffffdd; border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; /*background-color: #DDDDDD; */
        border: solid 1px Gray; color: Black">
        <table borderder="0" cellspacing="0" cellpadding="0" class="full-content">
            <tr>
                <td valign="top" nowrap="nowrap">
                    請輸入買受人統一編號：<asp:TextBox ID="txtBuyerReNo" runat="server" EnableViewState="false" />
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn-blue" Text="搜尋" 
                        CausesValidation="false" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        <div class="border_gray" style="height:auto; max-height: 300px; overflow:scroll">
            <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="95%"
                        GridLines="None" CellPadding="0" EnableViewState="False" CssClass="table01" AllowPaging="True"
                DataSourceID="dsEntity" DataKeyNames="OrgaCateID" OnPreRender="GV_PreRender">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input name="rbItem" type="radio" value='<%#((OrganizationCategory)Container.DataItem).Organization.CompanyID %>' />                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="統一編號">
                        <ItemTemplate>
                            <%# ((OrganizationCategory)Container.DataItem).Organization.ReceiptNo %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="買方名稱">
                        <ItemTemplate>
                            <%# ((OrganizationCategory)Container.DataItem).Organization.CompanyName %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="電話">
                        <ItemTemplate>
                            <%# ((OrganizationCategory)Container.DataItem).Organization.Phone %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="地址">
                        <ItemTemplate>
                            <%# ((OrganizationCategory)Container.DataItem).Organization.Addr %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
                <PagerTemplate>
                    <uc2:PagingControl  ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    <%--<uc2:PagingControl  ID="PagingControl1" runat="server" />--%>
                </PagerTemplate>
            </asp:GridView>
        </div>
         <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                <asp:Button ID="btnConfirm" runat="server" Text="確認" OnClick="btnConfirm_Click">
                                </asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="取消"  />
                            </span>
                        </td>
                    </tr>
                </table>
    </asp:Panel>
</asp:Panel>

<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
    DropShadow="true" PopupDragHandleControlID="Panel3" />

<cc1:OrganizationCategoryDataSource ID="dsEntity" runat="server" />
<uc1:ActionHandler ID="doCancel" runat="server" />

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnCancel.OnClientClick = doCancel.GetPostBackEventReference(null);
        buildQuery();

        doCancel.DoAction = arg => {
            this.Close();
        };        
    }
    
    void btnConfirm_Click(object sender, EventArgs e)
    {
        
        if (String.IsNullOrEmpty(this.SelectedValue))
        {
            this.AjaxAlert("請選擇買受人!!");
        }
        else
        {
            this.Close();
            OnDone(this, new EventArgs { });
        }
    }


    void GV_PreRender(object sender, EventArgs e)
    {
        this.BindData();
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        buildQuery();
    }

    void buildQuery()
    {
        //建立非中鋼集團買受人資料

        this.BuildQuery = table => 
        {
            if (!String.IsNullOrEmpty(this.txtBuyerReNo.Text))
            {
                return table.Where(o => o.CategoryID.Equals((int)Model.Locale.Naming.CategoryID.COMP_E_INVOICE_B2B_BUYER)
                & o.Organization.OrganizationStatus.CurrentLevel.Value.Equals((int)Model.Locale.Naming.MemberStatusDefinition.Checked) 
                & o.Organization.GroupRole == false
                & o.Organization.ReceiptNo.Contains(this.txtBuyerReNo.Text.Trim()))
                .OrderBy(o => o.Organization.ReceiptNo);
            }
            else
            {
                return table.Where(o => o.CategoryID.Equals((int)Model.Locale.Naming.CategoryID.COMP_E_INVOICE_B2B_BUYER)
                & o.Organization.OrganizationStatus.CurrentLevel.Value.Equals((int)Model.Locale.Naming.MemberStatusDefinition.Checked) 
                & o.Organization.GroupRole == false)
                .OrderBy(o => o.Organization.ReceiptNo);
            }
        };
    }
    
</script>