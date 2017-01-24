<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="Utility" %>

<%  if (_qrCode!=null)
    { %>
        <img style="margin-left: 0.3cm; margin-right: 0.3cm;" alt="" width="60" height="60" src="<%= eIVOGo.Properties.Settings.Default.mailLinkAddress + VirtualPathUtility.ToAbsolute("~/Published/GetQRCode.ashx")+"?text=" + Server.UrlEncode(_qrCode) %>" />
        <img style="margin-left: 0.3cm; margin-right: 0.3cm;" alt="" width="60" height="60" src="<%= eIVOGo.Properties.Settings.Default.mailLinkAddress + VirtualPathUtility.ToAbsolute("~/Published/GetQRCode.ashx")+"?text=" + Server.UrlEncode("**") %>" />
<%  } %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    InvoiceViewModel _item;
    String _qrCode;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _item = (InvoiceViewModel)ViewBag.ViewModel;
        if (_item != null)
            getQRCodeContent();
    }

    protected void getQRCodeContent()
    {
        String keyFile = Path.Combine(Logger.LogPath, "ORCodeKey.txt");
        if (!File.Exists(keyFile))
            return;

        String key = File.ReadAllText(keyFile);
        String EncryptContent = _item.TrackCode + _item.No + _item.RandomNo;
        com.tradevan.qrutil.QREncrypter qrencrypter = new com.tradevan.qrutil.QREncrypter();
        String finalEncryData = qrencrypter.AESEncrypt(EncryptContent, key);

        StringBuilder sb = new StringBuilder();
        sb.Append(_item.TrackCode + _item.No);
        sb.Append(String.Format("{0:000}{1:00}{2:00}", _item.InvoiceDate.Value.Year - 1911, _item.InvoiceDate.Value.Month, _item.InvoiceDate.Value.Day));
        sb.Append(_item.RandomNo);
        sb.Append(String.Format("{0:X8}", (int)_item.SalesAmount.Value));
        sb.Append(String.Format("{0:X8}", (int)_item.TotalAmount.Value));
        sb.Append(String.IsNullOrEmpty(_item.BuyerReceiptNo) ? "0000000000" : _item.BuyerReceiptNo);
        sb.Append(_item.SellerReceiptNo);
        sb.Append(finalEncryData);
        sb.Append(":");
        sb.Append("**********");
        sb.Append(":");
        sb.Append(_item.Brief.Length);
        sb.Append(":");
        sb.Append(_item.Brief.Length);
        sb.Append(":");
        sb.Append(1);
        sb.Append(":");
        for (int i = 0; i < _item.Brief.Length; i++)
        {
            sb.Append(_item.Brief[i]);
            sb.Append(":");
            sb.Append(String.Format("{0:0}", _item.Piece[i]));
            sb.Append(":");
            sb.Append(String.Format("{0:0}", _item.UnitCost[i]));
            sb.Append(":");
        }

        _qrCode = sb.ToString();

    }



</script>
