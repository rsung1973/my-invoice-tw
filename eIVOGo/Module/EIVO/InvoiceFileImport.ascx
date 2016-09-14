<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceFileImport.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoiceFileImport" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
    <%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc4" %>
    <!--路徑名稱-->
    

    <uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 發票檔匯入" />
    <!--交易畫面標題-->
    <uc4:FunctionTitleBar ID="titleBar" runat="server" ItemName="發票檔匯入" />
    <div id="DIV1" runat="server" class="border_gray">
        <!--表格 開始-->
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th width="150" nowrap="nowrap">
                    <span class="red">*</span> 發票檔匯入
                </th>
                <td class="tdleft">
                    &nbsp;
                    <asp:FileUpload ID="FileUpload" runat="server" />
                    &nbsp;
                    <asp:Button ID="btnConfirm" runat="server" CssClass="btn" OnClick="btnConfirm_Click"
                        Text="確認" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DIV2" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01">
            <tr>
                <th nowrap="nowrap">
                    序號
                </th>
                <th nowrap="nowrap">
                    Google ID
                </th>
                <th nowrap="nowrap">
                    發票號碼
                </th>
                <th nowrap="nowrap">
                    買受人名稱
                </th>
                <th nowrap="nowrap">
                    買受人統編
                </th>
                 <th nowrap="nowrap">
                    未稅金額
                </th>
                 <th nowrap="nowrap">
                    稅額
                </th>
                <th nowrap="nowrap">
                    含稅金額
                </th>
                <th nowrap="nowrap">
                    匯入狀態
                </th>
            </tr>
            <asp:Repeater ID="rpList" runat="server" EnableViewState="true">
                <ItemTemplate>
                    <tr>
                        <td align="left">
                            <%# ((ToReadData)Container.DataItem).SerialNumber%>
                        </td>
                        <td align="left">
                            <%# ((ToReadData)Container.DataItem).GoogleID%>
                        </td>
                        <td align="center">
                            <%# ((ToReadData)Container.DataItem).InvoiceTrackCode + ((ToReadData)Container.DataItem).InvoiceNo%>
                        </td>
                        <td align="left">
                            <%# ((ToReadData)Container.DataItem).BuyerName%>
                        </td>
                        <td align="left">
                            <%# ((ToReadData)Container.DataItem).BuyerUniformNumber%>
                        </td>
                        <td align="right">
                            <%# ((ToReadData)Container.DataItem).BuyAmount == "" ? "" : String.Format("{0:###,###}", decimal.Parse(((ToReadData)Container.DataItem).BuyAmount))%>
                        </td>
                        <td align="right">
                            <%# ((ToReadData)Container.DataItem).BuyTax == "" ? "" : String.Format("{0:###,###}", decimal.Parse(((ToReadData)Container.DataItem).BuyTax))%>
                        </td>
                        <td align="right">
                            <%# ((ToReadData)Container.DataItem).BuyAmountTax == "" ? "" : String.Format("{0:###,###}", decimal.Parse(((ToReadData)Container.DataItem).BuyAmountTax))%>
                        </td>
                        <td align="left" >
                            <font color="red" ><%# ((ToReadData)Container.DataItem).DataMessage%></font> 
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td colspan="9" align="right" class="total-count">
                    總計&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;匯入總筆數：<asp:Label 
                        ID="lblTotalCount" runat="server" EnableViewState="False"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;成功：<asp:Label ID="lblYesCount" 
                        runat="server" EnableViewState="False"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;失敗：<asp:Label ID="lblNoCount" 
                        runat="server" EnableViewState="False"></asp:Label>
        &nbsp;</td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="table-count">
        <tr>
            <td id="TDPC" runat="server" align="center">
                <uc2:PagingControl ID="PagingControl1" runat="server" width="100%" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
    <table id="TB3" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="center">
                <asp:Label ID="lblEx" runat="server" Text="新增號碼失敗!" Style="color: Red; font-size: Medium;"></asp:Label>
            </td>
        </tr>
    </table>
    <!--按鈕-->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn">
                <uc3:PrintingButton2 ID="btnPrint" runat="server" />
                <asp:Button ID="btnAddCode" runat="server" Text="確定匯入" OnClick="btnAddCode_Click" />
                <asp:Button ID="btnReset" runat="server" Text="取消匯入" OnClick="btnReset_Click" />
                <asp:Button ID="btnExcell" runat="server" Text="匯出EXCEL檔" 
                    OnClick="btnExcell_Click" />
            </td>
        </tr>
    </table>
    <cc1:InvoiceDataSource ID="dsEntity" runat="server">
    </cc1:InvoiceDataSource>
<asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" ClientIDMode="Static" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc4:FunctionTitleBar ID="msgBar" runat="server" ItemName="正在處理中，請稍後..." />
                <!--按鈕-->
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="Button1"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" 
    DropShadow="true" PopupDragHandleControlID="Panel3" />

<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" ClientIDMode="Static" />
<asp:Panel ID="Panel2" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel4" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc4:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="正在處理中，請稍後..." />
                <!--按鈕-->
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel2" BackgroundCssClass="modalBackground" 
    DropShadow="true" PopupDragHandleControlID="Panel4" />

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
                //                alert('Files with extension ".' + fileExtension.toUpperCase() + '" are not allowed.\n\nYou can upload the files with following extensions only:*.cvs');
                alert('檔案格是錯誤!請上傳CVS檔案格式!!');
                return false;
            }
            else {
                //                alert('The file had readed success.');
                $('#Button1').trigger('click');
                return true;
            }
        }
    </script>
