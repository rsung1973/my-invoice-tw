<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestAll.aspx.cs" Inherits="eIVOGo.Published.TestAll" StylesheetTheme="Visitor" %>

<%@ Register Src="../Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc2" %>
<%@ Register Src="../Module/Base/InvoiceTrackCodeList.ascx" TagName="InvoiceTrackCodeList"
    TagPrefix="uc1" %>
<%@ Register src="../Module/UI/WaitEventModal.ascx" tagname="WaitEventModal" tagprefix="uc3" %>
<%@ Register src="../Module/UI/WaitEventClientModal.ascx" tagname="WaitEventClientModal" tagprefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <uc2:CalendarInputDatePicker ID="CalendarInputDatePicker1" runat="server" />
        <uc1:InvoiceTrackCodeList ID="trackCodeList" runat="server" />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload Big5 Table" />
        <br />
        <asp:Button ID="btnWait" runat="server" Text="等10秒" onclick="btnWait_Click" />
        <asp:Button ID="btnDisplay" runat="server" Text="顯示"  />
        <asp:Button ID="btnHide" runat="server" Text="隱藏"  />
        <br />

        <asp:TextBox ID="controlPath" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnLoad" runat="server" onclick="btnLoad_Click" Text="Load" />

    <uc3:WaitEventModal ID="waitModal" runat="server" CompleteMessage="事件已完成!!" />

        <uc4:WaitEventClientModal ID="WaitEventClientModal1" runat="server" 
            CancelControlID="btnHide" TargetControlID="btnDisplay" />

    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $('#btnHide').bind('click', function () {
        alert($(this).text());
         $('#btnDisplay').trigger('click');
    });
   
</script>
