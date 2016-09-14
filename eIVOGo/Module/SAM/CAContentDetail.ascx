<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
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
                <%# _item.CDS_Document.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 ? _item.CDS_Document.InvoiceItem.Organization.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 ? _item.CDS_Document.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceItem.Organization.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
                                                        : null %>
            </td>
            <td align="center">
                <%# _item.CDS_Document.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 ? _item.CDS_Document.InvoiceItem.Organization.CompanyName
                                                                            : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 ? _item.CDS_Document.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceItem.Organization.CompanyName
                                                                            : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName
                                                        : null %>
            </td>
            <td align="center">
                <%# ValueValidity.ConvertChineseDateTimeString(_item.LogDate)%>
            </td>
        </tr>
    </table>
</div>
<uc1:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="憑證作業內容描述" />
<div class="border_gray">
    <div style="font-family: Arial,Helvetica,Geneva,sans-serif; font-size: 13px; color: #3264C8;">
        <!--表格 開始-->
        營業人統編：<%# _item.CDS_Document.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 ? _item.CDS_Document.InvoiceItem.Organization.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 ? _item.CDS_Document.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceItem.Organization.ReceiptNo
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
                                                        : null %>
        營業人名稱：<%# _item.CDS_Document.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 ? _item.CDS_Document.InvoiceItem.Organization.CompanyName
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 ? _item.CDS_Document.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceItem.Organization.CompanyName
                                                                          : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 ? _item.CDS_Document.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName
                                                        : null %>
        在<%# ValueValidity.ConvertChineseDateString(_item.LogDate)%>
        傳送 1 張<%# _item.DocumentType.TypeName %>
        ，資料如下：
        <%# _item.CDS_Document.DocType==(int)Naming.B2BInvoiceDocumentTypeDefinition.電子發票 ? String.Format("發票號碼:{0}{1}",_item.CDS_Document.InvoiceItem.TrackCode,_item.CDS_Document.InvoiceItem.No)
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.發票折讓 ? String.Format("折讓單號碼:{0}",_item.CDS_Document.InvoiceAllowance.AllowanceNumber)
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 ? String.Format("作廢發票號碼:{0}",_item.CDS_Document.DerivedDocument.ParentDocument.InvoiceItem.InvoiceCancellation.CancellationNo)
                                                        : _item.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 ? String.Format("作廢折讓單號碼:{0}",_item.CDS_Document.DerivedDocument.ParentDocument.InvoiceAllowance.AllowanceNumber)
                                                        : null %>
    </div>
    <!--表格 結束-->
</div>
<!--按鈕-->
<%--<center>
    <input name="closewin" type="button" value="關閉視窗" onclick="window.close();" class="btn" />
</center>
--%>
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
