<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Import Namespace="Utility" %>
<!--路徑名稱-->
<!--交易畫面標題-->
<style>
    .st
    {
        line-height: 20px;
    }
    .c
    {
        cursor: hand;
    }
    .b
    {
        color: red;
        font-family: 'Courier New';
        font-weight: bold;
        text-decoration: none;
    }
    .e
    {
        margin-left: 1em;
        text-indent: -1em;
        margin-right: 1em;
    }
    .k
    {
        margin-left: 1em;
        text-indent: -1em;
        margin-right: 1em;
    }
    .t
    {
        color: #990000;
    }
    .xt
    {
        color: #990099;
    }
    .ns
    {
        color: red;
    }
    .dt
    {
        color: green;
    }
    .m
    {
        color: blue;
    }
    .tx
    {
        font-weight: bold;
    }
    .db
    {
        text-indent: 0px;
        margin-left: 1em;
        margin-top: 0px;
        margin-bottom: 0px;
        padding-left: .3em;
        border-left: 1px solid #CCCCCC;
        font: small Courier;
    }
    .di
    {
        font: small Courier;
    }
    .d
    {
        color: blue;
    }
    .pi
    {
        color: blue;
    }
    .cb
    {
        text-indent: 0px;
        margin-left: 1em;
        margin-top: 0px;
        margin-bottom: 0px;
        padding-left: .3em;
        font: small Courier;
        color: #888888;
    }
    .ci
    {
        font: small Courier;
        color: #888888;
    }
    PRE
    {
        margin: 0px;
        display: inline;
    }
</style>
<script type="text/javascript">
    function f(e) {
        if (e.className == "ci") {
            if (e.children(0).innerText.indexOf("\n") > 0) fix(e, "cb");
        }
        if (e.className == "di") {
            if (e.children(0).innerText.indexOf("\n") > 0) fix(e, "db");
        } e.id = "";
    }
    function fix(e, cl) {
        e.className = cl;
        e.style.display = "block";
        j = e.parentElement.children(0);
        j.className = "c";
        k = j.children(0);
        k.style.visibility = "visible";
        k.href = "#";
    }
    function ch(e) {
        mark = e.children(0).children(0);
        if (mark.innerText == "+") {
            mark.innerText = "-";
            for (var i = 1; i < e.children.length; i++) {
                e.children(i).style.display = "block";
            }
        }
        else if (mark.innerText == "-") {
            mark.innerText = "+";
            for (var i = 1; i < e.children.length; i++) {
                e.children(i).style.display = "none";
            }
        }
    }
    function ch2(e) {
        mark = e.children(0).children(0);
        contents = e.children(1);
        if (mark.innerText == "+") {
            mark.innerText = "-";
            if (contents.className == "db" || contents.className == "cb") {
                contents.style.display = "block";
            }
            else {
                contents.style.display = "inline";
            }
        }
        else if (mark.innerText == "-") {
            mark.innerText = "+";
            contents.style.display = "none";
        }
    }
    function cl() {
        e = window.event.srcElement;
        if (e.className != "c") {
            e = e.parentElement;
            if (e.className != "c") {
                return;
            }
        }
        e = e.parentElement;
        if (e.className == "e") {
            ch(e);
        }
        if (e.className == "k") {
            ch2(e);
        }
    }
    function ex() { }
    function h() { window.status = " "; }
    document.onclick = cl;
</script>
<uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="憑證作業明細列表" />
<div class="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01">
        <tr>
            <th nowrap="nowrap">
                營業人統編
            </th>
            <th nowrap="nowrap">
                營業人名稱
            </th>
            <th nowrap="nowrap">
                憑證作業時間
            </th>
        </tr>
        <tr>
            <td align="center">
                <%# _item.CDS_Document.DocumentOwner.Organization.ReceiptNo%>
            </td>
            <td align="center">
                <%# _item.CDS_Document.DocumentOwner.Organization.CompanyName%>
            </td>
            <td align="center">
                <%# ValueValidity.ConvertChineseDateTimeString(_item.LogDate)%>
            </td>
        </tr>
    </table>
</div>
<uc1:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="憑證作業內容描述" />
<div class="border_gray">
    <!--表格 開始-->
            <%#  showDataToSign()%>
    <!--表格 結束-->
</div>
<!--按鈕-->
<%--<center>
    <input name="closewin" type="button" value="關閉視窗" onclick="window.close();" class="btn" />
</center>
--%>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">
    internal String showDataToSign()
    {
        if (!String.IsNullOrEmpty(_item.ContentPath) && System.IO.File.Exists(_item.ContentPath))
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(_item.ContentPath);

            System.IO.Stream xmlStream;
            System.Xml.Xsl.XslCompiledTransform xsl = new System.Xml.Xsl.XslCompiledTransform();
            ASCIIEncoding enc = new ASCIIEncoding();
            System.IO.StringWriter writer = new System.IO.StringWriter();

            // Get Xsl
            xsl.Load(Server.MapPath("~/Published/IEXmlView.xslt"));

            // Remove the utf encoding as it causes problems with the XPathDocument
            //TransactionXML = TransactionXML.Replace("utf-32", "");
            //TransactionXML = TransactionXML.Replace("utf-16", "");
            //TransactionXML = TransactionXML.Replace("utf-8", "");
            //xmlDoc.LoadXml(TransactionXML);

            // Get the bytes
            xmlStream = new System.IO.MemoryStream(enc.GetBytes(xmlDoc.OuterXml), true);

            // Load Xpath document
            System.Xml.XPath.XPathDocument xp = new System.Xml.XPath.XPathDocument(xmlStream);

            // Perform Transform
            xsl.Transform(xp, null, writer);
            return writer.ToString();
        }
       
        return null;
            
    }
</script>
<script runat="server">
    internal CALog _item;

    public CALog DataItem
    {
        get
        { return _item; }
        set
        {
            _item = value;
        }
    }
</script>
