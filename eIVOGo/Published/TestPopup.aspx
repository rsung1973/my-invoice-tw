<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPopup.aspx.cs" Inherits="eIVOGo.Published.TestPopup" StylesheetTheme="Visitor" %>

<%@ Register src="../Module/SAM/ShowTimeModal.ascx" tagname="ShowTimeModal" tagprefix="uc2" %>

<%@ Register src="../Module/Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <uc2:ShowTimeModal ID="timeModal" runat="server" />
        <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" /> 
        <uc1:PrintingButton2 ID="btnPrint" runat="server" />
        <br />
        <asp:Button ID="btnWait" runat="server" Text="等待10秒..." OnClick="btnWait_Click"    />
    &nbsp;
        <asp:Button ID="btnWait0" runat="server" Text="等待5秒..." onclick="btnWait0_Click" 
                />
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
//    $('#btnWait').click(
//            function () {
//                $('#waitingMsg_btnPopup').trigger('click');
//            }
//        );
</script>