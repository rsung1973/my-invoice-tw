<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnKeyStatusList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.TurnKeyStatusList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc2" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc3" %>
<%@ Register Src="../UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<%@ Import Namespace="Utility" %>
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="TurnKey傳送狀態" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th width="150">
                查詢類別
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="SearchItem" runat="server">
                    <%--<asp:ListItem Value="0">全部</asp:ListItem>--%>
                    <asp:ListItem Value="C0401">電子發票</asp:ListItem>
                    <asp:ListItem Value="D0401">電子折讓單</asp:ListItem>
                    <asp:ListItem Value="C0501">作廢電子發票</asp:ListItem>
                    <asp:ListItem Value="D0501">作廢電子折讓單</asp:ListItem>
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <th width="150">
                發票日期區間 
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="DateTo" runat="server" />
            </td>
        </tr>
                <tr>
            <th width="150">
                傳送日期區間  
            </th>
            <td class="tdleft">
                自&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="MESSAGEDTSFrom" runat="server" />
                &nbsp;
                至&nbsp;&nbsp;<uc3:CalendarInputDatePicker ID="MESSAGEDTSTo" runat="server" />
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Bargain_btn" colspan="4">
            <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQuery_Click" Text=" 查詢" />
        </td>
    </tr>
    <!--按鈕-->
</table>
<div id="divResult" visible="false" runat="server">
<uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
         <div id="border_gray">
    <table class="left_title" width="100%">
        <tr>
            <th style="width:25%">上傳成功筆數</th>
            
            <td >
                <span style="color:Red;"><%= _totalRecordCount %></span>
                &nbsp;&nbsp;&nbsp;&nbsp;
               
        </tr>
        </table>
<br />
            
             <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <td class="Head_style_a">未上傳發票</td>
                </tr>
            </table>
     <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" 
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False"  ShowFooter="True" >
    <Columns>
           <asp:TemplateField>
                <HeaderTemplate>
                    全選<input id="chkAll" name="chkAll" type="checkbox" onclick="$('input[id$=\'chkItem\']').attr('checked',$('input[id$=\'chkAll\']').is(':checked'));" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input id="chkItem" name="chkItem" type="checkbox" 
                        value='<%# getinvoiceitem((String)Container.DataItem)  %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <%#_Invoice !=null?_Invoice.InvoiceSeller.CustomerName: _InvoiceAllowance.InvoiceAllowanceSeller.CustomerName%>
               </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票/折讓單號">
            <ItemTemplate>
                <%#  ((String)Container.DataItem)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="發票/折讓日期" >
                <ItemTemplate>
                    <%#_Invoice !=null? ValueValidity.ConvertChineseDateString(_Invoice.InvoiceDate):
                   ValueValidity.ConvertChineseDateString(_InvoiceAllowance.AllowanceDate) %></ItemTemplate>
            </asp:TemplateField>
        <asp:TemplateField HeaderText="開立日期" >
                <ItemTemplate>
                    <%#_Invoice !=null? ValueValidity.ConvertChineseDateString(_Invoice.CDS_Document.DocDate):
                   ValueValidity.ConvertChineseDateString(_InvoiceAllowance.CDS_Document.DocDate) %></ItemTemplate>
            </asp:TemplateField>
        <asp:TemplateField HeaderText="金額">
            <ItemTemplate>
                <%#_Invoice !=null?String.Format("{0:0,0.00}", _Invoice.InvoiceAmountType.TotalAmount):
                String.Format("{0:0,0.00}", _InvoiceAllowance.TotalAmount)%></ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <FooterTemplate>
                共<%# _FalseCount %>筆</FooterTemplate>
        </asp:TemplateField>
        
    </Columns>
    <FooterStyle CssClass="total-count" />
    <EmptyDataTemplate>
       <span style="color: Red;">查無資料!!</span>
    </EmptyDataTemplate>
    <PagerStyle HorizontalAlign="Center" />
    <SelectedRowStyle />
    <HeaderStyle />
    <AlternatingRowStyle CssClass="OldLace" />
    <PagerTemplate>
        
    </PagerTemplate>
    <RowStyle />
    <EditRowStyle />
</asp:GridView>
                 <table border="0" cellspacing="0" cellpadding="0" width="100%" runat="server" visible="false"
        enableviewstate="false" id="tblAction">
        <tbody>
            <tr>
                <td class="Bargain_btn">
                    <asp:Button ID="btnShow" runat="server" Text="重送" OnClick="btnShow_Click" />&nbsp;&nbsp;
                </td>
            </tr>
        </tbody>
    </table>
   <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
</div></div>
<cc1:V_LogsDataSource ID="dsInv" runat="server">
</cc1:V_LogsDataSource>
<cc2:InvoiceDataSource ID="dsInvoice" runat="server">
</cc2:InvoiceDataSource>
<script runat="server">

InvoiceItem _Invoice;
InvoiceAllowance _InvoiceAllowance;
String getinvoiceitem(String invoiceno)
{
    if (SearchItem.SelectedValue.Equals("C0401") || SearchItem.SelectedValue.Equals("C0501"))
    {
        _Invoice = dsInvoice.CreateDataManager().GetTable<InvoiceItem>()
           .Where(i => (i.TrackCode + i.No).Equals(invoiceno)).FirstOrDefault();
        _InvoiceAllowance = null;
        if (SearchItem.SelectedValue.Equals("C0401"))
            return _Invoice.InvoiceID.ToString();
        else
            return dsInvoice.CreateDataManager().GetTable<DerivedDocument>().Where(d => d.SourceID == _Invoice.InvoiceID).FirstOrDefault().DocID.ToString();
        
    }
    else
    {
        _InvoiceAllowance = dsInvoice.CreateDataManager().GetTable<InvoiceAllowance>()
            .Where(a => a.AllowanceNumber.Equals(invoiceno)).FirstOrDefault();
        _Invoice = null;
        if (SearchItem.SelectedValue.Equals("D0401"))
        return _InvoiceAllowance.AllowanceID.ToString();
        else
            return dsInvoice.CreateDataManager().GetTable<DerivedDocument>().Where(d => d.SourceID == _InvoiceAllowance.AllowanceID).FirstOrDefault().DocID.ToString();
        
    }
    
}    


</script>