<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPagingControl.aspx.cs" Inherits="eIVOGo.Published.TestPagingControl"  %>

<%@ Register src="../Module/Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" DataKeyNames="InvoiceID" 
            DataSourceID="dsEntity" EnableViewState="False" 
            onrowcommand="gvEntity_RowCommand">
            <Columns>
                <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID" ReadOnly="True" 
                    SortExpression="InvoiceID" />
                <asp:BoundField DataField="No" HeaderText="No" SortExpression="No" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="InvoiceDate" 
                    SortExpression="InvoiceDate" />
                <asp:BoundField DataField="CheckNo" HeaderText="CheckNo" 
                    SortExpression="CheckNo" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" 
                    SortExpression="Remark" />
                <asp:BoundField DataField="BuyerRemark" HeaderText="BuyerRemark" 
                    SortExpression="BuyerRemark" />
                <asp:BoundField DataField="CustomsClearanceMark" 
                    HeaderText="CustomsClearanceMark" SortExpression="CustomsClearanceMark" />
                <asp:BoundField DataField="TaxCenter" HeaderText="TaxCenter" 
                    SortExpression="TaxCenter" />
                <asp:BoundField DataField="PermitDate" HeaderText="PermitDate" 
                    SortExpression="PermitDate" />
                <asp:BoundField DataField="PermitWord" HeaderText="PermitWord" 
                    SortExpression="PermitWord" />
                <asp:BoundField DataField="PermitNumber" HeaderText="PermitNumber" 
                    SortExpression="PermitNumber" />
                <asp:BoundField DataField="Category" HeaderText="Category" 
                    SortExpression="Category" />
                <asp:BoundField DataField="RelateNumber" HeaderText="RelateNumber" 
                    SortExpression="RelateNumber" />
                <asp:BoundField DataField="InvoiceType" HeaderText="InvoiceType" 
                    SortExpression="InvoiceType" />
                <asp:BoundField DataField="GroupMark" HeaderText="GroupMark" 
                    SortExpression="GroupMark" />
                <asp:BoundField DataField="DonateMark" HeaderText="DonateMark" 
                    SortExpression="DonateMark" />
                <asp:BoundField DataField="SellerID" HeaderText="SellerID" 
                    SortExpression="SellerID" />
                <asp:BoundField DataField="DonationID" HeaderText="DonationID" 
                    SortExpression="DonationID" />
                <asp:BoundField DataField="RandomNo" HeaderText="RandomNo" 
                    SortExpression="RandomNo" />
                <asp:BoundField DataField="TrackCode" HeaderText="TrackCode" 
                    SortExpression="TrackCode" />
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="True" 
                            CommandName="Update" Text="Update" />
                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <input type="submit" name="invoke" value='<%# Eval("InvoiceID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </PagerTemplate>
        </asp:GridView>
        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
        <input type="submit" name="btnSubmit" value="Submit" /><asp:Button ID="Button3" 
            runat="server" Text="Button" />
            </div>
    </form>
</body>
</html>