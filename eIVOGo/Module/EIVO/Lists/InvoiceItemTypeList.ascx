<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemTypeList.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoiceItemTypeList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Register src="~/Module/Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 500px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; /*background-color: #DDDDDD;*/
        border: solid 1px Gray; color: Black">
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <!--按鈕-->
                <table borderder="0" cellspacing="0" cellpadding="0" class="full-content">
                    <tr>
                        <!-- InstanceBeginEditable name="contant" -->
                        <td valign="top" nowrap="nowrap">
                            請輸入品項名稱關鍵字 :<asp:TextBox ID="txtItemName" runat="server" EnableViewState="false"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn-blue" Text="搜尋" 
                                CausesValidation="False" onclick="btnSearch_Click">
                            </asp:Button>&nbsp;
                        </td>
                        <!-- InstanceEndEditable -->
                    </tr>
                </table>
                <div class="border_gray" style="height: auto; max-height: 300px; overflow: scroll">
                    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="95%"
                        GridLines="None" CellPadding="0" EnableViewState="False" CssClass="table01"
                        DataSourceID="dsEntity" DataKeyNames="PICID" AllowPaging="True">
                        <Columns>
                            <asp:TemplateField HeaderText="編號" SortExpression="CRC_Branch">
                                <ItemTemplate>
                                    <%# gvEntity.PageIndex*gvEntity.PageSize+Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <img id="img2" runat="server" enableviewstate="false" src="~/images/yes.gif" width="16"
                                        height="16" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input name="rbItem" type="radio" value='<%# ((ProductItemCategory)Container.DataItem).PICID  %>' <%--<%# ((ProductItemCategory)Container.DataItem).PICID.Equals(int.Parse(SelectedValue)) ? "checked" : null%>--%> />                                    
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="品項編號" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ItemName" HeaderText="品項名稱" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Unit" HeaderText="單位" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="UnitePrice" HeaderText="單價" DataFormatString="{0:#,0.##}" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <AlternatingRowStyle CssClass="OldLace" />
                        <PagerTemplate>
                            <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                <asp:Button ID="btnConfirm" runat="server" Text="確認" OnClick="btnConfirm_Click">
                                </asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="取消" />
                            </span>
                        </td>
                    </tr>
                </table>
<%--            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<cc1:ProductItemCategoryDataSource ID="dsEntity" runat="server">
</cc1:ProductItemCategoryDataSource>
<uc1:ActionHandler ID="doCancel" runat="server" />

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        btnCancel.OnClientClick = doCancel.GetPostBackEventReference(null);
        buildQuery();
        doCancel.DoAction = arg =>
            {
                this.Close();
            };
    }

    void btnConfirm_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(this.SelectedValue))
        {
            this.AjaxAlert("請選擇品項!!");
        }
        else
        {
            this.Close();
            OnDone(this, new EventArgs { });
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        buildQuery();
        this.BindData();
    }

    void buildQuery()
    {
        this.BuildQuery = tabel =>
        {
            if (!String.IsNullOrEmpty(this.txtItemName.Text.Trim()))
            {
                return tabel.Where(b => b.ItemName.Contains(this.txtItemName.Text.Trim())).OrderBy(b => b.PICID);
            }
            else
            {
                return tabel.OrderBy(b => b.PICID);
            }
        };
    }

</script>
