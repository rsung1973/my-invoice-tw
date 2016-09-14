<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceCancelImport.ascx.cs"
    Inherits="eIVOGo.Module.Saler.InvoiceAllowanceCancelImport" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 >作廢折讓單滙入" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="作廢折讓單滙入" />
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title" runat="server"
        clientidmode="Static ">
        <tr>
            <th width="20%">
                作廢折讓單匯入
            </th>
            <td class="tdleft">
                <asp:FileUpload ID="FileUpload1" runat="server" /><asp:Button class="btn" Text="確認"
                    ID="btnOK" runat="server" OnClick="btnOK_Click" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<div id="divResult" visible="false" runat="server">
    <div id="border_gray">
        <asp:GridView ID="gvEntity" runat="server" EnableViewState="True" CssClass="table01"
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True"
            Width="100%">
            <AlternatingRowStyle CssClass="OldLace" />
            <Columns>
                <%--<asp:TemplateField HeaderText="序號">
                    <ItemTemplate>
                        <%#  ((InvoiceAllowanceCancels)Container.DataItem).OrderNo%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="goolge id">
                    <ItemTemplate>
                        <asp:Label ID="lable2" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).CustomerID %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="折讓號碼">
                    <ItemTemplate>
                        <asp:Label ID="lable3" runat="server" Text='<%#  ((InvoiceAllowanceCancels)Container.DataItem).AllowanceNo %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="作廢原因">
                    <ItemTemplate>
                        <asp:Label ID="lable3" runat="server" Text='<%#  ((InvoiceAllowanceCancels)Container.DataItem).Remark %>' />
                    </ItemTemplate>
                </asp:TemplateField>                

               <%-- <asp:TemplateField HeaderText="原發票號碼">
                    <ItemTemplate>
                        <asp:Label ID="lable3" runat="server" Text='<%#  ((InvoiceAllowanceCancels)Container.DataItem).InvNo %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="買受人名稱">
                    <ItemTemplate>
                        <asp:Label ID="lable5" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).BuyerCustomerName %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="買受人統編">
                    <ItemTemplate>
                        <asp:Label ID="lable6" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).BuyerReceiptNo %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="未稅金額">
                    <ItemTemplate>
                        <asp:Label ID="lable7" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).Amount.ToString () == "0" ? "N/A" : String.Format("{0:##,###,###,##0.00}", decimal.Parse (((InvoiceAllowanceCancels)Container.DataItem).Amount))%>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="稅額">
                    <ItemTemplate>
                        <asp:Label ID="lable7" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).Tax.ToString () == "0" ? "N/A" : String.Format("{0:##,###,###,##0.00}", decimal.Parse (((InvoiceAllowanceCancels)Container.DataItem).Tax))%>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="含稅金額">
                    <ItemTemplate>
                        <asp:Label ID="lable7" runat="server" Text='<%# ((InvoiceAllowanceCancels)Container.DataItem).TAmount.ToString () == "0" ? "N/A" : String.Format("{0:##,###,###,##0.00}", decimal.Parse (((InvoiceAllowanceCancels)Container.DataItem).TAmount))%>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:Label ID="lable9" runat="server" Text='<%#  "總計  匯入總筆數:"+TotalNum.ToString() %>' />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="滙入狀態">
                    <ItemTemplate>
                        <%#  _queryItems.Where(w => w.Status == "滙入成功").Count() > 0 ? "<font color='red'>" : "<font color='red'>"%>
                        <asp:Label ID="lable8" runat="server" Text='<%#  ((InvoiceAllowanceCancels)Container.DataItem).Status %>' />
                        <%# "</font >" %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:Label ID="lable9" runat="server" Text='<%#("成功:"+ YesNum.ToString() + "  失敗:" + NoNum.ToString())%>' />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <uc2:PagingControl ID="pagingList" runat="server" />
            </PagerTemplate>
        </asp:GridView>
    </div>
    <center>
        <asp:Label ID="lblError" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
        <asp:Button ID="ImportButton" runat="server" Text="確定滙入" OnClick="ImportButton_Click" />
        &nbsp;<asp:Button ID="CannelButton" runat="server" Text="取消滙入" OnClick="CannelButton_Click" />
        <br />
        &nbsp;<uc4:PrintingButton2 ID="PrintingButton21" runat="server" Visible="false" />
        <asp:Button ID="btnExcel" runat="server" OnClick="Button1_Click" Visible="false"
            Text="匯出EXCEL檔" />
    </center>
</div>
<asp:Panel ID="Panel1" runat="server" Style="display: none; z-index: 10; width: 650px;
    background-color: #ffffdd; border-width: 3px; border-style: solid; border-color: Gray;
    padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc4:FunctionTitleBar ID="msgBar" runat="server" ItemName="正在處理中，請稍後..." />
        <!--按鈕-->
    </asp:Panel>
</asp:Panel>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" ClientIDMode="Static" />
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
    PopupDragHandleControlID="Panel3" />
<script type="text/javascript">
    function validateFileUpload(obj) {
        var fileName = new String();
        var fileExtension = new String();
        // store the file name into the variable       
        fileName = obj.value;
        // extract and store the file extension into another variable        
        fileExtension = fileName.substr(fileName.length - 3, 3);
        // array of allowed file type extensions        
        var validFileExtensions = new Array("csv");
        var flag = false;
        // loop over the valid file extensions to compare them with uploaded file        
        for (var index = 0; index < validFileExtensions.length; index++) {
            if (fileExtension.toLowerCase() == validFileExtensions[index].toString().toLowerCase()) {
                flag = true;
            }
        }
        // display the alert message box according to the flag value       
        if (flag == false) {
            //                alert('Files with extension ".' + fileExtension.toUpperCase() + '" are not allowed.\n\nYou can upload the files with following extensions only:*.csv');
            alert('檔案格是錯誤!請上傳CSV檔案格式!!');
            return false;
        }
        else {
            //                alert('The file had readed success.');
            return true;
        }

    }
</script>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
