<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>

<td align="left"><%# _item.InvoiceItem.InvoiceSeller.CustomerName %></td>
<td align="center">
    <uc1:InvoiceNoPreview runat="server" ID="preview" />
</td>
<td align="left">
    <%# _item.InvoiceItem.InvoiceBuyer.CustomerName %>
</td>
<td align="center"><%# _item.InvoiceItem.InvoiceBuyer.ReceiptNo %>  </td>
<td align="left">
    <%# _item.InvoiceItem.InvoiceBuyer.ContactName %>
</td>
<td>
    <%# _item.InvoiceItem.InvoiceBuyer.Phone %>
</td>
<td align="left">
    <%# _item.InvoiceItem.InvoiceBuyer.Address %>
</td>
<td align="center">
    <input type="button" name="btnEdit" value="編輯" onclick='<%# "editInvoiceBuyer(" + _index.ToString() + "," + _docID.ToString() + ");" %>' />
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
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('hello!!');", true);
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["message"] != null)
        {
            writer.WriteLine(String.Format("<{0}>alert('{1}')</{0}>", HtmlTextWriterTag.Script, Page.PreviousPage.Items["message"]));
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
