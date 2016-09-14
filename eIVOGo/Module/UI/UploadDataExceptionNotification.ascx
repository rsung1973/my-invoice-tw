<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadDataExceptionNotification.ascx.cs"
    Inherits="eIVOGo.Module.UI.UploadDataExceptionNotification" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%--發票錯誤訊息--%>
<div id="invInfo" runat="server" enableviewstate="false">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th class="Head_style_a">
                發票
            </th>
        </tr>
    </table>
    <asp:GridView ID="gvInvoice" runat="server" Width="100%" AutoGenerateColumns="False"
        EnableViewState="False" EmptyDataText="沒有新資料!!" CssClass="table01" DataKeyNames="LogID">
        <Columns>
            
            <asp:TemplateField HeaderText="發票號碼" SortExpression="DataContent">
                <ItemTemplate>
                    <a href='<%# String.Format("{0}{1}?logID={2}",eIVOGo.Properties.Settings.Default.mailLinkAddress,VirtualPathUtility.ToAbsolute("~/Published/DumpExceptionLog.ashx"),Eval("LogID"))%>'
                        target="_blank">
                        <%#  ((bool?)Eval("IsCSV")) == true ? Eval("DataContent") : getInvoiceContent((String)Eval("DataContent"))%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人公司名稱" SortExpression="DataContent">
                <ItemTemplate>
                   <%#_seller!=null?_seller.CompanyName:""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人統編" SortExpression="DataContent">
                <ItemTemplate>
                       <%#_invoiceItem.SellerId%></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogTime" HeaderText="時間" SortExpression="LogTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
            <asp:BoundField DataField="Message" HeaderText="錯誤訊息" SortExpression="Message" />
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <EmptyDataTemplate>
            <%# setEmpty(invInfo) %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
<%--作廢發票錯誤訊息--%>
<div id="cancelInfo" runat="server" enableviewstate="false">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th class="Head_style_a">
                作廢發票
            </th>
        </tr>
    </table>
    <asp:GridView ID="gvCancel" runat="server" Width="100%" AutoGenerateColumns="False"
        EnableViewState="False" EmptyDataText="沒有新資料!!" CssClass="table01" DataKeyNames="LogID">
        <Columns>
            <asp:TemplateField HeaderText="作廢發票號碼" SortExpression="DataContent">
                <ItemTemplate>
                    <a href='<%# String.Format("{0}{1}?logID={2}",eIVOGo.Properties.Settings.Default.mailLinkAddress,VirtualPathUtility.ToAbsolute("~/Published/DumpExceptionLog.ashx"),Eval("LogID"))%>'
                        target="_blank">
                        <%#  ((bool?)Eval("IsCSV")) == true ? Eval("DataContent") : getCancellationContent((String)Eval("DataContent"))%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人公司名稱" SortExpression="DataContent">
                <ItemTemplate>
                   <%#_seller!=null?_seller.CompanyName:""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人統編" SortExpression="DataContent">
                <ItemTemplate>
                       <%#_cancelItem.SellerId%></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogTime" HeaderText="時間" SortExpression="LogTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
            <asp:BoundField DataField="Message" HeaderText="錯誤訊息" SortExpression="Message" />
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <EmptyDataTemplate>
            <%# setEmpty(cancelInfo) %>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
<%--折讓錯誤訊息--%>
<div ID="allowanceInfo" runat="server" EnableViewState ="false">
       <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th class="Head_style_a">
                    折　　讓
                </th>
            </tr>
        </table>
   <asp:GridView ID="gvAllowance" runat="server" Width="100%" AutoGenerateColumns="False"
        EnableViewState="False" EmptyDataText="沒有新資料!!" CssClass="table01" DataKeyNames="LogID">
        <Columns>
           <asp:TemplateField HeaderText="折讓號碼" SortExpression="DataContent">
              <ItemTemplate>
                  <a href='<%# String.Format("{0}{1}?logID={2}",eIVOGo.Properties.Settings.Default.mailLinkAddress,VirtualPathUtility.ToAbsolute("~/Published/DumpExceptionLog.ashx"),Eval("LogID"))%>'
                            target="_blank">
                            <%#  ((bool?)Eval("IsCSV")) == true ? Eval("DataContent") : getAllowanceContent((String)Eval("DataContent"))%></a>
              </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人公司名稱" SortExpression="DataContent">
                <ItemTemplate>
                  <%#_seller!=null?_seller.CompanyName:""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人統編" SortExpression="DataContent">
                <ItemTemplate>
                       <%#_allowance.SellerId%></ItemTemplate>
            </asp:TemplateField>
           <asp:BoundField DataField="LogTime" HeaderText="時間" SortExpression="LogTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
           <asp:BoundField DataField="Message" HeaderText="錯誤訊息" SortExpression="Message" />
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <EmptyDataTemplate>
            <%# setEmpty(allowanceInfo) %>
        </EmptyDataTemplate>
   </asp:GridView>
</div>
<%--作廢折讓--%>
<div ID="cancelAllowanceInfo" runat="server" EnableViewState ="false">
       <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th class="Head_style_a">
                    作廢折讓
                </th>
            </tr>
        </table>
   <asp:GridView ID="gvCancelAllowance" runat="server" Width="100%" AutoGenerateColumns="False"
        EnableViewState="False" EmptyDataText="沒有新資料!!" CssClass="table01" DataKeyNames="LogID">
        <Columns>
           <asp:TemplateField HeaderText="折讓號碼" SortExpression="DataContent">
              <ItemTemplate>
                  <a href='<%# String.Format("{0}{1}?logID={2}",eIVOGo.Properties.Settings.Default.mailLinkAddress,VirtualPathUtility.ToAbsolute("~/Published/DumpExceptionLog.ashx"),Eval("LogID"))%>'
                            target="_blank">
                            <%#  ((bool?)Eval("IsCSV")) == true ? Eval("DataContent") : getCancellationAllowanceContent((String)Eval("DataContent"))%></a>
              </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人公司名稱" SortExpression="DataContent">
                <ItemTemplate>
                   <%#_seller!=null?_seller.CompanyName:""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="開立人統編" SortExpression="DataContent">
                <ItemTemplate>
                       <%#_cancelAllowanceItem.SellerId%></ItemTemplate>
            </asp:TemplateField>
           <asp:BoundField DataField="LogTime" HeaderText="時間" SortExpression="LogTime" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
           <asp:BoundField DataField="Message" HeaderText="錯誤訊息" SortExpression="Message" />
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <EmptyDataTemplate>
            <%# setEmpty(cancelAllowanceInfo) %>
        </EmptyDataTemplate>
   </asp:GridView>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">
    private String setEmpty(Control ctrl)
    {
        ctrl.Visible = false;
        return "沒有新資料!!";
    }
</script>
