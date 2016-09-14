<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.ImportInvoiceFile" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc5" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--路徑名稱-->


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 上期發票檔匯入" />
<!--交易畫面標題-->
<uc4:FunctionTitleBar ID="titleBar" runat="server" ItemName="上期發票檔匯入" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr id="trNormal" runat="server" visible="false">
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
        <tr id="trNone" runat="server" visible="true">
            <td colspan="2" style="color:Red;">上期發票只能在單數月的1日到10日期間匯入!!</td>
        </tr>
    </table>
</div>
<!--表格 結束-->
<!--按鈕-->
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
            return true;
        }
    }
</script>
<uc5:PageAnchor ID="NextAction" runat="server" RedirectTo="~/EIVO/LastPeriodInvoiceUploadPreview.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DateTime now = DateTime.Now;
        if (now.Month % 2 == 1 && now.Day <= 10)
        {
            trNone.Visible = false;
            trNormal.Visible = true;
        }
    }
    
    protected override Model.InvoiceManagement.IGoogleUploadManager createUploadManager()
    {
        DateTime now = DateTime.Now;
        if (now.Month % 2 != 1 || now.Day > 10)
        {
            this.AjaxAlert("上期發票只能在單數月的1日到10日期間匯入!!");
            return null;
        }

        var mgr = new Model.InvoiceManagement.GoogleInvoiceUploadManager();
        mgr.UploadInvoiceDate = (new DateTime(now.Year, now.Month, 1)).AddDays(-1);
        return mgr;
    }
</script>
