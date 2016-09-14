<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUploadReturnMailMessage.ascx.cs" Inherits="eIVOGo.Module.EIVO.WebUploadReturnMailMessage" %>
<%@ Register  Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1"%>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc3" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>



    
<uc1:PageAction ID="PageAction1" runat="server" ItemName="首頁 > Web上傳退回掛號郵件" />
<uc2:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="Web上傳退回掛號郵件" />
<div id="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th>發票號碼(起號)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtInv_No_begin" runat="server"></asp:TextBox>
            </td>
            <th>郵件張數
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_Amount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>郵件號碼1
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_No_1" runat="server"></asp:TextBox>
            </td>
            <th>郵件號碼2
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_No_2" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="預覽" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>

<div id="divResult" visible="false" runat="server">
    <uc2:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="資料預覽" />
    <div id="border_gray">
        <div runat="server" id="ResultTitle">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <!--查詢結果_資料呈現-->
                <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
                    GridLines="None" CellPadding="0" CssClass="table01"
                    AllowPaging="True" ClientIDMode="Static"
                    EnableViewState="False" ShowFooter="True">
                    <Columns>
                        <asp:TemplateField HeaderText="重寄日期">
                            <ItemTemplate>
                                <%# String.Format("{0:yyy/MM/dd}", ((InvoiceDeliveryTracking)Container.DataItem).DeliveryDate )%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GoogleID">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.CustomerID %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="發票號碼">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.TrackCode + ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.No %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="掛號號碼">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).TrackingNo1 + "　" +((InvoiceDeliveryTracking)Container.DataItem).TrackingNo2 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收件人">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.ContactName %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收件人地址">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.Address %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="total-count" />
                    <PagerStyle HorizontalAlign="Center" />
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <AlternatingRowStyle CssClass="OldLace" />
                    <PagerTemplate>
                        <uc3:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </PagerTemplate>
                    <RowStyle />
                    <EditRowStyle />
                </asp:GridView>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn">
                            <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="確定" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </table>

        </div>
    </div>
</div>