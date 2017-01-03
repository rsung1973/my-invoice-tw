using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;
using System.IO;
using Utility;
using System.Security.Cryptography;
using System.Text;
using eIVOGo.Helper;
using eIVOGo.Properties;

namespace eIVOGo.Module.EIVO
{
    public partial class NewInvoicePrintView : System.Web.UI.UserControl
    {
        protected InvoiceItem _item;
        protected InvoiceBuyer _buyer;
        protected Organization _buyerOrg;
        protected UserProfileMember _userProfile;
        protected InvoiceProductItem[] _productItem;
        protected int ItemCount;
        protected const int _FirstCheckCount = 6;
        protected const int _SecondCheckCount = 16;
        protected const int _ItemPagingCount = 10;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsSysAdmin == true)
            {
                _userProfile = UserProfileFactory.CreateInstance(Settings.Default.SystemAdmin);
                if (_userProfile == null)
                {
                    throw new Exception("SystemAdmin Not Found!!");
                }
            }
            else
            {
                _userProfile = WebPageUtility.UserProfile;
            }
        }

        [Bindable(true)]
        public int? InvoiceID
        {
            get
            {
                return _item != null ? _item.InvoiceID : (int?)null;
            }
            set
            {
                _productItem = null;
                _buyer = null;
                _buyerOrg = null;

                var mgr = dsEntity.CreateDataManager();
                _item = mgr.EntityList.Where(i => i.InvoiceID == value).FirstOrDefault();
                _productItem = mgr.GetTable<InvoiceDetail>().Where(d => d.InvoiceID == value)
                    .Join(mgr.GetTable<InvoiceProduct>(), d => d.ProductID, p => p.ProductID, (d, p) => p)
                    .Join(mgr.GetTable<InvoiceProductItem>(), p => p.ProductID, i => i.ProductID, (p, i) => i).ToArray();
            }
        }

        [Bindable(true)]
        public bool IsFinal
        {
            get;
            set;
        }

        [Bindable(false)]
        public bool? IsSysAdmin
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoicePrintView_PreRender);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (_item != null)
            {
                var mgr = dsEntity.CreateDataManager();
                if (!_item.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice))
                {
                    _item.CDS_Document.DocumentPrintLogs.Add(new DocumentPrintLog
                    {
                        PrintDate = DateTime.Now,
                        UID = _userProfile.UID,
                        TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice
                    });
                }

                mgr.DeleteAnyOnSubmit<DocumentPrintQueue>(d => d.DocID == _item.InvoiceID);
                mgr.SubmitChanges();
            }
        }

        void InvoicePrintView_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                _buyer = _item.InvoiceBuyer;
                if (_buyer != null && _buyer.BuyerID.HasValue)
                    _buyerOrg = _buyer.Organization;

                String data = String.Format("{0:000}{1:00}{2}{3}{4}", _item.InvoiceDate.Value.Year - 1911, _item.InvoiceDate.Value.Month, _item.TrackCode, _item.No, _item.RandomNo);
                this.barcode.Src = String.Format("{0}{1}?{2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/GetBarCode39.ashx"), data);

                String returnData1 = "", returnData2 = "**";
                getQRCodeContent(_item, ref returnData1, ref returnData2);
                this.qrcode1.Src = String.Format("{0}{1}?text={2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/GetQRCode.ashx"), Server.UrlEncode(returnData1));
                this.qrcode2.Src = String.Format("{0}{1}?text={2}", Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/GetQRCode.ashx"), Server.UrlEncode(returnData2));
            }
        }


        protected void getQRCodeContent(InvoiceItem item, ref String data1, ref String data2)
        {
            //byte[] key = File.ReadAllBytes(Path.Combine(Logger.LogPath, "ORCodeKey.txt"));
            //String EncryptContent = item.TrackCode + item.No + item.RandomNo;
            //byte[] encQuote = EncryptContent.EncryptString(key);
            //String finalEncryData = Convert.ToBase64String(encQuote);

            String key = File.ReadAllText(Path.Combine(Logger.LogPath, "ORCodeKey.txt"));
            String EncryptContent = item.TrackCode + item.No + item.RandomNo;
            com.tradevan.qrutil.QREncrypter qrencrypter = new com.tradevan.qrutil.QREncrypter();
            String finalEncryData = qrencrypter.AESEncrypt(EncryptContent, key);

            StringBuilder sb = new StringBuilder();
            sb.Append(item.TrackCode + item.No);
            sb.Append(String.Format("{0:000}{1:00}{2:00}", item.InvoiceDate.Value.Year - 1911, item.InvoiceDate.Value.Month, item.InvoiceDate.Value.Day));
            sb.Append(item.RandomNo);
            sb.Append(String.Format("{0:X8}", (int)item.InvoiceAmountType.SalesAmount.Value));
            sb.Append(String.Format("{0:X8}", (int)item.InvoiceAmountType.TotalAmount.Value));
            sb.Append(_buyer.IsB2C() ? "00000000" : _buyer.ReceiptNo);
            sb.Append(item.InvoiceSeller != null ? item.InvoiceSeller.ReceiptNo : item.Organization.ReceiptNo);
            sb.Append(finalEncryData);
            sb.Append(":");
            sb.Append("**********");
            sb.Append(":");
            sb.Append(item.InvoiceDetails.Count);
            sb.Append(":");
            sb.Append(item.InvoiceDetails.Count);
            sb.Append(":");
            sb.Append(1);
            sb.Append(":");
            foreach (var p in item.InvoiceDetails)
            {
                //sb.Append(p.InvoiceProduct.Brief);
                //sb.Append(":");
                foreach (var pd in p.InvoiceProduct.InvoiceProductItem)
                {
                    if (!pd.Piece.Value.Equals(0))
                    {
                        sb.Append(p.InvoiceProduct.Brief);
                        sb.Append(":");
                        sb.Append(String.Format("{0:#0}", pd.Piece));
                        sb.Append(":");
                        sb.Append(String.Format("{0:#0}", pd.UnitCost));
                        sb.Append(":");
                    }
                }
            }

            //data1 = sb.Length > 120 ? sb.ToString().Substring(0, 120) : sb.ToString();
            //data2 += sb.Length > 120 ? sb.ToString().Substring(120) : "";

            getStringByByte(sb.ToString(), 130, ref data1, ref data2);
        }


        protected void getStringByByte(string data, int LimitedByteLength, ref String d1, ref String d2)
        {
            int index = 0;
            string firstString = "";
            string RestOfString = "";
            string str = data;
            bool doAgain = false;

            do
            {                
                string spp = "";
                int byteLength;
                
                for (int i = 0; i <= str.Length; i++)
                {
                    spp = str.Substring(0, i);
                    byteLength = Encoding.UTF8.GetBytes(spp).Length;
                    if (byteLength <= LimitedByteLength)
                    { firstString = spp; }
                    else
                    {
                        firstString = str.Substring(0, i - 1);
                        RestOfString = str.Substring(i - 1);
                        break;
                    }
                }

                if (index.Equals(0))
                {
                    d1 = firstString;
                }
                else if (index.Equals(1))
                {
                    d2 += firstString;
                    break;
                }

                if (!String.IsNullOrEmpty(RestOfString) && Encoding.UTF8.GetBytes(RestOfString).Length > LimitedByteLength)
                {
                    doAgain = true;
                    index++;
                    str = RestOfString;
                }
                else
                    d2 += RestOfString;

            } while (doAgain);
        }

        protected int showState(int type)
        {
            switch (type)
            {
                case (int)Naming.InvoiceTypeDefinition.三聯式:
                    return (int)Naming.InvoiceTypeFormat.三聯式;
                case (int)Naming.InvoiceTypeDefinition.二聯式:
                    return (int)Naming.InvoiceTypeFormat.二聯式;
                case (int)Naming.InvoiceTypeDefinition.二聯式收銀機:
                    return (int)Naming.InvoiceTypeFormat.二聯式收銀機;
                case (int)Naming.InvoiceTypeDefinition.特種稅額:
                    return (int)Naming.InvoiceTypeFormat.特種稅額;
                case (int)Naming.InvoiceTypeDefinition.電子計算機:
                    return (int)Naming.InvoiceTypeFormat.電子計算機;
                case (int)Naming.InvoiceTypeDefinition.三聯式收銀機:
                    return (int)Naming.InvoiceTypeFormat.三聯式收銀機;
                case (int)Naming.InvoiceTypeDefinition.一般稅額計算之電子發票:
                    return (int)Naming.InvoiceTypeFormat.一般稅額計算之電子發票;
                default:
                    return 0;
            }
        }
    }
}