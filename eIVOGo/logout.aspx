<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="eIVOGo.logout" StylesheetTheme="Login" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>電子發票系統</title>
<script type="text/javascript" language="javascript">
<!--
    //顯示年份//
    function show_date() {
        var time = new Date(); //宣告日期物件，儲存目前系統時間
        t_year = time.getFullYear(); //取得今年年分
        if (t_year > 2011) {
            document.write(" - " + t_year);
        }
    }
-->
</script>
</head>
<body>
<div class="copyright">
        &copy; 2011
        <script type="text/javascript" language="javascript">            show_date();</script>
        UXB2B. All rights reserved.
    </div>
</body>
</html>