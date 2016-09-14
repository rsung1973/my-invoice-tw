<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicSendSMSMessage.ascx.cs" Inherits="eIVOGo.Module.Inquiry.DynamicSendSMSMessage" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<script type="text/javascript" language="javascript">
    function doConfirm(obj) {
        if ($("#ddlCompany").val() == "請選擇") {
            alert("請選擇店家!!");
            $("#ddlCompany").focus();
            return false;
        }

        if ($("#rdbFreeType").is(":checked")) {
            if ($("#txtMobilNo").val() == "") {
                alert("請填寫手機號碼!!");
                $("#txtMobilNo").focus();
                return false;
            }
        } else if ($("#rdbUpload").is(":checked")) {
            if ($("#txtMobilePreview").val() == "") {
                alert("請匯入手機號碼並執行預覽!!");
                $("#Upload").focus();
                return false;
            }
        }

        if ($("#txtMsgContent").val() == "") {
            alert("請填寫發送內容!!");
            $("#txtMsgContent").focus();
            return false;
        }

        if (!confirm('確定送出？')) {
            return false;
        }
    }

    function doVerify(obj) {
        if ($("#Upload").val() == "") {
            alert("請匯入手機號碼!!");
            $("#Upload").focus();
            return false;
        }
    }
</script>


<uc1:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 客服訊息" />        
<!--交易畫面標題-->
<uc2:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="客服訊息" />
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title" id="typeTable" runat="server">
    <tr>
        <th>店家</th>
        <td class="tdleft">
                <asp:DropDownList ID="ddlCompany" CssClass="textfield" Width="30%" ClientIDMode="Static" runat="server">
                    <asp:ListItem>請選擇</asp:ListItem>
                    <asp:ListItem Value="1">網際優勢</asp:ListItem>
                </asp:DropDownList>
        </td>
    </tr>            
    <tr>
        <th rowspan="2" style="width:20%;">手機號碼</th>
        <td class="tdleft">
            <table width="65%">
                <tr>
                    <td style="width:5%;border-width:0px !important; text-align:left;">
                        <asp:RadioButton ID="rdbFreeType" GroupName="mobil" runat="server" Checked="true"
                            AutoPostBack="true" oncheckedchanged="rdbFreeType_CheckedChanged" ClientIDMode="Static"/>
                    </td>
                    <td style="border-width:0px !important;text-align:left;">
                        <asp:TextBox ID="txtMobilNo" TextMode="MultiLine" Width="90%" Rows="3" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                        <font color="red" >說明：多筆接收人時，請以半形逗點隔開( , )，如 0912345678,0922333444。</font>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdleft">
            <table width="65%">
                <tr>
                    <td rowspan="2" style="width:5%;border-width:0px !important; text-align:left;">
                        <asp:RadioButton ID="rdbUpload" GroupName="mobil" runat="server" 
                            AutoPostBack="true" oncheckedchanged="rdbFreeType_CheckedChanged" 
                            ClientIDMode="Static" />
                    </td>
                    <td style="border-width:0px !important;text-align:left;">
                        <asp:TextBox ID="txtMobilePreview" TextMode="MultiLine" Width="90%" ClientIDMode="Static" Rows="3" Enabled="false" runat="server"></asp:TextBox><br />
                        <font color="red" >說明：檔案內容格式一行為一筆電話號碼,不加任何標點符號,可存成 CSV 或 txt 檔。</font>
                    </td>
                </tr>
                <tr>
                    <td style="border-width:0px !important;text-align:left;">
                        匯入檔案 <asp:FileUpload ID="Upload" Width="65%" runat="server" Enabled="false" ClientIDMode="Static" />
                        <asp:Button ID="btnUploadPreview" CssClass="btn" Text="號碼預覽" runat="server" Enabled="false"
                            onclick="btnUploadPreview_Click" OnClientClick="return doVerify(this);"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <th>發送內容</th>
        <td class="tdleft">
            <asp:TextBox ID="txtMsgContent" TextMode="MultiLine" Width="60%" Rows="5" ClientIDMode="Static"
                runat="server" ></asp:TextBox>
        </td>
    </tr>
</table>

<center><asp:Label ID="lblMsg" Visible="false" Text="此功能尚未開啟,請洽系統管理員!!!" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label></center>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0" id="btnTable" runat="server">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnSend" CssClass="btn" runat="server" Text="發送" onclick="btnSend_Click" OnClientClick="return doConfirm(this);" />
        </td>
    </tr>
</table>

<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
