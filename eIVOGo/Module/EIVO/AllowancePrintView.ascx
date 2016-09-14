<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllowancePrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.AllowancePrintView" %>
<%@ Register src="Item/AllowanceBalanceView.ascx" tagname="AllowanceBalanceView" tagprefix="uc1" %>
<%@ Register src="Item/AllowanceAccountingView.ascx" tagname="AllowanceAccountingView" tagprefix="uc2" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<uc1:AllowanceBalanceView ID="balanceView" runat="server" />
<p style="border-bottom: 1px dotted #000; margin-top: 20px; margin-bottom: 20px;">
    &nbsp;</p>

<uc2:AllowanceAccountingView ID="accountingView" runat="server" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>

