<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportInvoiceFile.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.ImportInvoiceFile" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc3" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>
<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc5" %>
<!--���|�W��-->


<uc1:PageAction ID="actionItem" runat="server" ItemName="���� > �o���ɶפJ" />
<!--����e�����D-->
<uc4:FunctionTitleBar ID="titleBar" runat="server" ItemName="�o���ɶפJ" />
<div class="border_gray">
    <!--��� �}�l-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th width="150" nowrap="nowrap">
                <span class="red">*</span> �o���ɶפJ
            </th>
            <td class="tdleft">
                &nbsp;
                <asp:FileUpload ID="FileUpload" runat="server" />
                &nbsp;
                <asp:Button ID="btnConfirm" runat="server" CssClass="btn" OnClick="btnConfirm_Click"
                    Text="�T�{" />
            </td>
        </tr>
    </table>
</div>
<!--��� ����-->
<!--���s-->
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
            alert('�ɮ׮�O���~!�ФW��CVS�ɮ׮榡!!');
            return false;
        }
        else {
            //                alert('The file had readed success.');
            return true;
        }
    }
</script>
<uc5:PageAnchor ID="NextAction" runat="server" RedirectTo="~/EIVO/InvoiceUploadPreview.aspx" />

