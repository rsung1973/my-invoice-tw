<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GovPlatformNotification.ascx.cs" Inherits="eIVOGo.Module.UI.GovPlatformNotification" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
<tr>
    <th class="Head_style_a">發票</th>
</tr>
</table>
<asp:GridView ID="gvInvoice" runat="server" Width="100%" AutoGenerateColumns="False" EnableViewState="False" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red" EmptyDataText="沒有新資料!!" CssClass="table01">
            <Columns>
                <asp:TemplateField HeaderText="發票號碼">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.TrackCode") %><%# Eval("CDS_Document.InvoiceItem.No")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastActionTime" HeaderText="傳送時間" 
                    SortExpression="LastActionTime" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Message" HeaderText="錯誤代碼" 
                    SortExpression="Message" ItemStyle-HorizontalAlign="Center" />
            </Columns>
            <AlternatingRowStyle CssClass="OldLace" />
        </asp:GridView>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
<tr>
    <th class="Head_style_a">作廢發票</th>
</tr>
</table>
<asp:GridView ID="gvCancel" runat="server" Width="100%" AutoGenerateColumns="False" EnableViewState="False" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red" EmptyDataText="沒有新資料!!" CssClass="table01">
            <Columns>
                <asp:TemplateField HeaderText="發票號碼">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.TrackCode") %><%# Eval("CDS_Document.InvoiceItem.No")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastActionTime" HeaderText="傳送時間" 
                    SortExpression="LastActionTime" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Message" HeaderText="錯誤代碼" 
                    SortExpression="Message" ItemStyle-HorizontalAlign="Center" />
            </Columns>
            <AlternatingRowStyle CssClass="OldLace" />
        </asp:GridView>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
    </cc1:InvoiceDataSource>