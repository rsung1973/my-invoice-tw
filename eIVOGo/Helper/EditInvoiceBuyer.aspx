<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>

<td align="left"><%# _item.InvoiceItem.InvoiceSeller.CustomerName %></td>
<td align="center">
    <uc1:InvoiceNoPreview runat="server" ID="preview" />
</td>
<td align="left">
    <input type="text" name="CustomerName" value="<%# _item.InvoiceItem.InvoiceBuyer.CustomerName %>" />
</td>
<td align="center"><%# _item.InvoiceItem.InvoiceBuyer.ReceiptNo %>  </td>
<td align="left">
    <input type="text" name="ContactName" value="<%# _item.InvoiceItem.InvoiceBuyer.ContactName %>" />
</td>
<td>
    <input type="text" name="Phone" value="<%# _item.InvoiceItem.InvoiceBuyer.Phone %>" />
</td>
<td align="left">
    <textarea id="Address" name="Address" rows="4"><%# _item.InvoiceItem.InvoiceBuyer.Address %></textarea>
</td>
<td align="center">
    <input id="btnConfirm" type="button" value="更新" onclick='<%# "updateInvoiceBuyer(" + _index.ToString() + "," + _docID.ToString() + ");" %>' />
    <input id="btnCancel" type="button" value="取消" onclick='<%# "loadInvoiceBuyer(" + _index.ToString() + "," + _docID.ToString() + ");" %>' /><br />
    <input id="btnPostCode" type="button" value="查詢郵遞區號" onclick='<%# "queryZipCode(" + _index.ToString() + ");" %>' />
</td>
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script runat="server">

    CDS_Document _item;
    int _index;
    int _docID;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += helper_editinvoicebuyer_aspx_PreRender;

        if (Request["index"] != null && int.TryParse(Request["index"], out _index))
        {
            if (Request["docID"] != null && int.TryParse(Request["docID"], out _docID))
            {
                _item = dsEntity.CreateDataManager().EntityList.Where(d => d.DocID == _docID).FirstOrDefault();
            }
        }

    }

    void helper_editinvoicebuyer_aspx_PreRender(object sender, EventArgs e)
    {
        if (_item != null)
        {
            preview.DataItem = _item;
            this.DataBind();
        }
        else
        {
            this.Visible = false;
        }
    }
    
</script>
