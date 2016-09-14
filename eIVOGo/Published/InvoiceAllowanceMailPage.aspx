<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAllowanceMailPage.aspx.cs" StylesheetTheme="Paper" 
Inherits="eIVOGo.Published.InvoiceAllowanceMailPage" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Module.Common" %>
<%@ Register src="../Module/EIVO/Item/InvoiceAllowanceMailView.ascx" tagname="InvoiceAllowanceMailView" tagprefix="uc1" %>
<%@ Register Src="~/Module/SYS/Info/GovInvoiceLink.ascx" TagPrefix="uc1" TagName="GovInvoiceLink" %>
<%@ Register Src="~/Module/SYS/Info/ServiceNotation.ascx" TagPrefix="uc1" TagName="ServiceNotation" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        body
        {
            margin: 10px;
            padding: 0px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }
        p
        {
            margin: 5px;
            padding: 0px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }
        a:link
        {
            margin-left: 5px;
            margin-right: 5px;
            font-size: 12px; /*color:#0C419A;*/
            color: #FF6600;
            text-decoration: none;
            border-bottom: 1px dotted #666666;
        }
        a:hover
        {
            color: #0066CC;
            text-decoration: none;
            border-bottom: 1px solid #666666;
        }
        
        div.border_gray
        {
            border: 4px solid #DDD;
            background: #FFFFFF;
            margin: 10px 0px;
            padding: 10px;
        }
        .left_title
        {
            font-family: sans-serif,Geneva,Arial,Helvetica;
            border-top: 1px solid #DEB887;
            border-left: 1px solid #F5DEB3;
            border-bottom: 1px solid #F5DEB3;
            margin: 0px;
            color: #666666;
        }
        .left_title th
        {
            padding: 3px 5px;
            border-top: 1px solid #FFFFFF;
            font-size: 12px;
            font-weight: normal;
            line-height: 160%;
            color: #A0522D;
            background-color: #F5DEB3;
        }
        .left_title th.bordertop
        {
            border-top-width: 0px;
        }
        .left_title td
        {
            padding: 3px 5px;
            border-right: 1px solid #F5DEB3;
            border-top: 1px solid #F5DEB3;
            font-size: 12px;
            line-height: 160%;
            text-align: center;
        }
        .left_title td.tdright
        {
            text-align: right;
        }
        .left_title td.tdleft
        {
            text-align: left;
        }
        /*--明細列視窗--*/
        .table01
        {
            font-family: sans-serif,Geneva,Arial,Helvetica;
            border-top: 1px solid #DEB887;
            border-right: 1px solid #DDDDDD;
            margin: 0px;
            color: #666666;
        }
        .table01 th
        {
            padding: 3px 5px;
            border-left: 1px solid #FFF;
            border-bottom: 1px solid #FFF;
            font-size: 12px;
            font-weight: normal;
            line-height: 160%;
            color: #FFFFFF;
            background-color: #c99040;
        }
        .table01 th.borderleft
        {
            padding: 3px 5px;
            border-left: 1px solid #DEB887;
            font-size: 12px;
            font-weight: normal;
            line-height: 160%;
            color: #FFFFFF;
            background-color: #c99040;
        }
        .table01 td
        {
            padding: 3px 5px;
            border-left: 1px solid #DDDDDD;
            border-bottom: 1px solid #DDDDDD;
            font-size: 12px;
            line-height: 160%;
        }
        .table01 td.tdright
        {
            text-align: right;
        }
        .table01 td.tdleft
        {
            text-align: left;
        }
        .OldLace
        {
            background-color: #FDF5E6;
        }
        .Head_style_a
        {
            font-size: 12px !important;
            color: #A0522D;
            background: url(../images/Head_style_1.gif) repeat-x top;
            padding-top: 5px 2px 5px 2px;
            text-align: center;
            line-height: 15px;
            letter-spacing: 1px;
        }
        .blue
        {
            color: #0066CC;
        }
        .red
        {
            color: #FF0000;
        }
        .Head_style_a
        {
            font-size: 12px !important;
            color: #A0522D;
            background: url(images/Head_style_1.gif) repeat-x top;
            padding-top: 5px 2px 5px 2px;
            text-align: center;
            line-height: 15px;
            letter-spacing: 1px;
        }
    </style>
    <title>電子發票系統</title>
</head>
<body>
    <form id="form1" runat="server">
    <!--交易畫面標題-->
    <p>
        您的客戶編號：<span class="blue"><%# SharedFunction.StringMask(_item.InvoiceItem.InvoiceBuyer.CustomerID, 4, 3, 'X')%></span> 發票已經開立，電子發票不直接寄送，以下為您的電子發票內容：</p>
    <div class="border_gray">
        <!--表格 開始-->
        <uc1:InvoiceAllowanceMailView ID="mailView" runat="server" />
        <!--表格 結束-->
    </div>
    <p>
        <% if(eIVOGo.Properties.Settings.Default.ShowAuthorizationNoInMail) { %>
        本公司奉財政部核准文號：<span class="blue">臺北市國稅局信義分局100年7月22日財北國稅信義營業字第1000213452號</span>，使用電子發票。
        <% } %>
        對於二聯式發票用戶，本公司會以電子郵件將發票開立與中獎訊息通知客戶。</p>
    <p class="red">
        注意事項：</p>
    <ol>
        <li>依財政部令此通知內容僅供參考，不可直接兌換。若發票中獎，將於開獎十五日內，將中獎發票掛號寄給您。</li>
        <li>如需發票正本，請點選[<a id="invRequest" runat="server" href="#">索取紙本發票</a>]。</li>
        <li>發票正本只能索取一次，一旦遺失，無法再次索取。</li>
        <li>根據財政部「電子發票實施作業要點」規範，營業人將您的發票明細資料上傳到政府<uc1:GovInvoiceLink runat="server" ID="GovInvoiceLink" />
            儲存，若您要至該網站查詢發票開立狀況，「個人識別碼」欄位，請輸入本通知之「個人識別碼」。</li>
    </ol>
    <div id="showService" runat="server" visible='<%# _item.Organization.OrganizationStatus.SetToOutsourcingCS == true %>'>
        <uc1:ServiceNotation runat="server" ID="ServiceNotation" />
    </div>
    <p class="red">
        ※此信件為系統發出信件，請勿直接回覆。</p>
    <!--按鈕-->
    <cc1:InvoiceDataSource ID="dsEntity" runat="server">
    </cc1:InvoiceDataSource>
    </form>
</body>
</html>
