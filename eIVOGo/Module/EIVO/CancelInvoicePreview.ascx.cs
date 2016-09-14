using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.DataEntity;
using Model.Helper;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using Uxnet.Web.WebUI;
using System.IO;
using System.Threading;

namespace EIVO07Tools.Module.EIVO
{
    public partial class CancelInvoicePreview : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;
        int[] rejectID;
        protected Dictionary<int, String> inputValues;
        static Boolean isCSC;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rejectID = (int[])rejectItem.DataItem;
            inputValues = (Dictionary<int, String>)rejectInput.DataItem;
            this.btnBack.Attributes.Add("onClick", "javascript:history.back(); return false;");
            this.dsInv.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceItem>>(dsInv_Select);
        }

        void dsInv_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceItem> e)
        {
            e.Query = dsInv.CreateDataManager().EntityList.Where(i => rejectID.Contains(i.InvoiceID)).OrderByDescending(i => i.InvoiceID);
        }

        protected void btnCancelCreat_Click(object sender, EventArgs e)
        {
            if (SaveToDB())
            {
                rejectInput.Clear();
                rejectItem.Clear();
                this.AjaxAlertAndRedirect("作廢電子發票開立完成!!", VirtualPathUtility.ToAbsolute("~/EIVO/CancelInvoice.aspx"));
            }
            else
            {
                this.AjaxAlert("作廢電子發票開立錯誤!!");
            }
        }

        #region SaveToDB()
        bool SaveToDB()
        {
            try
            {
                var mgr = dsInv.CreateDataManager();
                var invoice = mgr.EntityList.Where(i => rejectID.Contains(i.InvoiceID)).OrderByDescending(i => i.InvoiceID);
                
                foreach (var item in invoice)
                {
                    isCSC = item.GroupMark.Equals("01");
                    var cancelItem = new InvoiceCancellation
                    {
                        InvoiceID = item.InvoiceID,
                        CancellationNo = String.Format("{0}{1:00000000}", item.TrackCode, item.No),
                        Remark = inputValues[item.InvoiceID],
                        ReturnTaxDocumentNo = "",
                        CancelDate = DateTime.ParseExact(String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                    };

                    var doc = new DerivedDocument
                    {
                        CDS_Document = new CDS_Document
                        {
                            DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                            DocDate = DateTime.Now,
                            DocumentOwner = new DocumentOwner
                            {
                                OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                            }
                        },
                        SourceID = cancelItem.InvoiceID
                    };

                    mgr.GetTable<InvoiceCancellation>().InsertOnSubmit(cancelItem);
                    mgr.GetTable<DerivedDocument>().InsertOnSubmit(doc);
                }
                mgr.SubmitChanges();

                ThreadPool.QueueUserWorkItem(state =>
                {
                    createXMLCancelInvoices(rejectID.ToList());
                });
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        #endregion

        #region "CancelInvoiceData To XML"
        private static void createXMLCancelInvoices(List<int> docids)
        {
            try
            {
                String folderPath = isCSC ? Settings.Default.CSCGPUploadInvoiceCancellationFolder : Settings.Default.SOGOUploadInvoiceCancellationFolder;
                folderPath.CheckStoredPath();
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    if (docids.Count() > 0)
                    {
                        foreach (int id in docids)
                        {
                            var items = mgr.GetTable<CDS_Document>().Where(d => d.DerivedDocument.SourceID.Equals(id));
                            String fileName = Path.Combine(folderPath, String.Format("{0}_CancelInvoice.xml", DateTime.Now.ToString("yyyyMMddHHmmssf")));
                            items.CreateCancelInvoiceRoot().ConvertToXml().Save(fileName);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion
    }    
}