<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Paper" 
    CodeBehind="AlertAdmTurnKeyInfo.aspx.cs" Inherits="eIVOGo.Published.AlertAdmTurnKeyInfo"%>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="theForm" runat="server">
      <div align="center"><span style="color:Red; border:outset;text-align:center">兩個月內未上傳筆數
          (<%=Utility.ValueValidity.ConvertChineseDateString(DateTime.Today.AddMonths(-2) )%>
          至 <%=Utility.ValueValidity.ConvertChineseDateString(DateTime.Today)%>)</span>
         </div><br />
          <table class="table-bk" width="100%">
        <tr >
            <th style="width:40%">發票</th>
            
            <td >
                <span style="color:Red;"><%= _C0401FalseCount %></span>
                &nbsp;&nbsp;&nbsp;&nbsp;
               </td>
        </tr>
             
             <tr>
            <th style="width:40%">折讓</th>
            
            <td >
                <span style="color:Red;"><%= _D0401FalseCount %></span>
                &nbsp;&nbsp;&nbsp;&nbsp;
               </td>
        </tr>
             <tr>
            <th style="width:49%">作廢發票</th>
            
            <td >
                <span style="color:Red;"><%= _C0501FalseCount %></span>
                &nbsp;&nbsp;&nbsp;&nbsp;
               </td>
        </tr>
             <tr>
            <th style="width:40%">作廢折讓</th>
            
            <td >
                <span style="color:Red;"><%= _D0501FalseCount %></span>
                &nbsp;&nbsp;&nbsp;&nbsp;
               </td>
        </tr>
        </table>
    </form>

</body>
</html>
<cc1:V_LogsDataSource ID="dsInv" runat="server">
</cc1:V_LogsDataSource>
<cc2:InvoiceDataSource ID="dsInvoice" runat="server">
</cc2:InvoiceDataSource>
