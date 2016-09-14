<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintAllowancePage.aspx.cs" Inherits="eIVOGo.SAM.PrintAllowancePage" StylesheetTheme="Allowance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <style type="text/css" media="print">
        div.fspace {
            height: 9.3cm;
        }

        div.bspace {
            height: 10cm;
        }
    </style>
    <title>電子發票系統</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
</head>
<body onload='javascript:self.print();self.close();'>
    <form id="theForm" runat="server">
    </form>
</body>
</html>
