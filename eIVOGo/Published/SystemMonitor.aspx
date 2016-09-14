<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemMonitor.aspx.cs" Inherits="eIVOGo.Published.SystemMonitor" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        發票資料待傳輸：<asp:GridView ID="gvInvoice" 
            runat="server" AutoGenerateColumns="False" EnableViewState="False">
            <Columns>
                <asp:TemplateField HeaderText="TrackCode" SortExpression="TrackCode">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.TrackCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No" SortExpression="No">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.No")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LastActionTime" HeaderText="LastActionTime" 
                    SortExpression="LastActionTime" />
                <asp:BoundField DataField="Message" HeaderText="Message" 
                    SortExpression="Message" />
            </Columns>
        </asp:GridView>
        作廢發票資料待傳輸：<asp:GridView ID="gvCancel" runat="server" 
            AutoGenerateColumns="False" 
            EnableViewState="False">
            <Columns>
                <asp:TemplateField HeaderText="TrackCode" SortExpression="TrackCode">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.TrackCode") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No" SortExpression="No">
                    <ItemTemplate>
                        <%# Eval("CDS_Document.InvoiceItem.No")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LastActionTime" HeaderText="LastActionTime" 
                    SortExpression="LastActionTime" />
                <asp:BoundField DataField="Message" HeaderText="Message" 
                    SortExpression="Message" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnGov" runat="server" onclick="btnGov_Click" Text="傳輸資料至大平台" />
    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnRefresh" runat="server" Text="重新整理" />
    </div>
    <cc1:InvoiceDataSource ID="dsEntity" runat="server">
    </cc1:InvoiceDataSource>
    </form>
</body>
</html>
