<%@ Control Language="C#" AutoEventWireup="true"%>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<%@ Import Namespace="Utility" %>
<!--路徑名稱-->
<!--交易畫面標題-->
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
        <pre style="font-family: Arial,Helvetica,Geneva,sans-serif; font-size: 13px; color: #3264C8;">
            <%#  showDataToSign()%>
        </pre>
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
            System.Xml.XmlDocument docCA = new System.Xml.XmlDocument();
            docCA.Load(_item.ContentPath);
            return docCA.DocumentElement["pkcs7Envelop"]["DataToSign"].InnerXml;
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
