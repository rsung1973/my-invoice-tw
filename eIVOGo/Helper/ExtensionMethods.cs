using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.UI;
using eIVOGo.template;
using Model.DataEntity;
using Uxnet.Web.WebUI;
using Model.Security.MembershipManagement;
using DataAccessLayer.basis;
using Utility;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Model.Locale;
using System.Web.Mvc;

namespace eIVOGo.Helper
{
    public static partial class ExtensionMethods
    {
        public static int[] GetKeyValue(this String keyValue)
        {
            return keyValue.Split(',').Select(s => int.Parse(s)).ToArray();
        }

        public static String[] GetItemSelection(this HttpRequest request)
        {
            return request.Form.GetValues("chkItem");
        }

        public static void SetTransferMessage(this Page page, String message)
        {
            page.Items[base_page.PAGE_ALERT_ITEM_KEY] = message;
        }

        public static PopupModalMessage AttachWaitingMessage(this Button button,String message, bool autoAttach)
        {
            PopupModalMessage modal = button.Page.Items["waitingMsg"] as PopupModalMessage;
            if (modal == null)
            {
                modal = (PopupModalMessage)button.Page.LoadControl("~/Module/UI/PopupModalMessage.ascx");
                modal.ID = "waitingMsg";
                modal.InitializeAsUserControl(button.Page);
                button.Parent.Controls.Add(modal);
                button.Page.Items["waitingMsg"] = modal;
                modal.Message = message;
            }

            if (autoAttach)
            {
                button.Attributes["onclick"] = modal.GetClientTriggerScript();
            }

            return modal;
        }

        public static bool OrganizationValueCheck(this Organization dataItem,Control control)
        {
            if (String.IsNullOrEmpty(dataItem.CompanyName))
            {
                //檢查名稱
                WebMessageBox.AjaxAlert(control, "請輸入公司名稱!!");
                return false;
            }
            if (String.IsNullOrEmpty(dataItem.ReceiptNo))
            {
                //檢查名稱
                WebMessageBox.AjaxAlert(control, "請輸入公司統編!!");
                return false;
            }
            if (String.IsNullOrEmpty(dataItem.Addr))
            {
                //檢查名稱
                WebMessageBox.AjaxAlert(control, "請輸入公司地址!!");
                return false;
            }
            if (String.IsNullOrEmpty(dataItem.Phone))
            {
                //檢查名稱
                WebMessageBox.AjaxAlert(control, "請輸入公司電話!!");
                return false;
            }

            Regex reg = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

            if (!reg.IsMatch(dataItem.ContactEmail))
            {
                //檢查email
                WebMessageBox.AjaxAlert(control,"電子信箱尚未輸入或輸入錯誤!!");
                return false;
            }
            return true;
        }

        public static void EnqueueInvoicePrint(this UserProfileMember userProfile, GenericManager<EIVOEntityDataContext> mgr, IEnumerable<int> invoiceID)
        {
            foreach (var id in invoiceID)
            {
                var item = mgr.GetTable<InvoiceItem>().Where(i => i.InvoiceID == id).FirstOrDefault();
                if (item != null && item.InvoicePrintQueue == null)
                {
                    item.InvoicePrintQueue = new InvoicePrintQueue
                    {
                        SubmitDate = DateTime.Now,
                        UID = userProfile.UID
                    };
                }
            }
            mgr.SubmitChanges();
        }

        public static bool EnqueueDocumentPrint(this UserProfileMember userProfile, GenericManager<EIVOEntityDataContext> mgr, IEnumerable<int> docID)
        {
            bool result = false;
            foreach (var id in docID)
            {
                var item = mgr.GetTable<CDS_Document>().Where(i => i.DocID == id).FirstOrDefault();
                if (item != null && item.DocumentPrintQueue == null
                    && (item.InvoiceItem == null || item.IsPrintableInvoice()))
                {
                    item.DocumentPrintQueue=new DocumentPrintQueue
                    {
                        SubmitDate = DateTime.Now,
                        UID = userProfile.UID
                    };
                    result = true;
                }
            }
            if (result)
                mgr.SubmitChanges();
            return result;
        }

        public static String CreateContentAsPDF(this HttpServerUtility Server, String relativePath, double timeOutInMinute)
        {
            String path = Server.MapPath("~/temp");
            path.CheckStoredPath();

            Guid uniqueID = Guid.NewGuid();
            String saveTo = Path.Combine(path, String.Format("{0}.htm", uniqueID));
            String pdfFile = Path.Combine(path, String.Format("{0}.pdf", uniqueID));
            String tempHtml = Path.Combine(Logger.LogDailyPath, String.Format("{0}.htm", uniqueID));

            using (StreamWriter sw = new StreamWriter(tempHtml))
            {
                Server.Execute(relativePath, sw, true);
                sw.Flush();
                sw.Close();
            }
            File.Move(tempHtml, saveTo);

            //convertHtmlToPDF(saveTo, pdfFile, timeOutInMinute);

            saveTo.ConvertHtmlToPDF(pdfFile, timeOutInMinute);

            if (File.Exists(pdfFile))
            {
                File.Delete(saveTo);

                bool checking = true;
                while (checking)
                {
                    try
                    {
                        using (var fs = File.OpenRead(pdfFile))
                        {
                            fs.Close();
                            checking = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
                return pdfFile;
            }

            return null;
        }

        public static byte[] EncryptString(this String EncryptContent, byte[] key)
        {
            AesCryptoServiceProvider aesCSP = new AesCryptoServiceProvider();
            aesCSP.Key = key;
            aesCSP.GenerateIV();

            byte[] inBlock = Encoding.Default.GetBytes(EncryptContent);
            ICryptoTransform xfrm = aesCSP.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            return outBlock;
        }

        public static bool ParseDate(this String dateStr, out DateTime? dateValue)
        {
            dateValue = null;
            if (!String.IsNullOrEmpty(dateStr))
            {
                DateTime dateVal;
                if (DateTime.TryParse(dateStr,out dateVal))
                {
                    dateValue = dateVal;
                    return true;
                }
            }
            return false;
        }

        public static bool IsPrintableInvoice(this CDS_Document item)
        {
            return item.InvoiceItem != null 
                && (!item.InvoiceItem.InvoiceBuyer.IsB2C() 
                    || ((item.InvoiceItem.PrintMark == "Y" || (item.InvoiceItem.PrintMark == "N" && item.InvoiceItem.InvoiceWinningNumber != null))
                        && !item.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice)));
        }

        public static void SetModelSource<TEntity>(this TempDataDictionary tempData, ModelSource<TEntity> models)
            where TEntity : class,new()
        {
            tempData["modelSource"] = models;
        }

        public static ModelSource<TEntity> GetModelSource<TEntity>(this TempDataDictionary tempData)
            where TEntity : class,new()
        {
            return (ModelSource<TEntity>)tempData["modelSource"];
        }

        public static GenericManager<EIVOEntityDataContext> GetGenericModelSource(this TempDataDictionary tempData)
        {
            return (GenericManager<EIVOEntityDataContext>)tempData["modelSource"];
        }


        public static ModelSource<TEntity> InvokeModelSource<TEntity>(this TempDataDictionary tempData)
            where TEntity : class,new()
        {
            GenericManager<EIVOEntityDataContext> models = tempData.GetGenericModelSource();
            if (models == null)
            {
                models = new ModelSource<TEntity>();
                tempData.SetModelSource<TEntity>((ModelSource<TEntity>)models);
                return (ModelSource<TEntity>)models;
            }
            else if (models is ModelSource<TEntity>)
            {
                return (ModelSource<TEntity>)models;
            }
            else
            {
                return new ModelSource<TEntity>(models);
            }
        }


        public static void DownloadCsv<TEntity>(this ViewUserControl control, Func<IEnumerable<TEntity>, IEnumerable<object>> getCsvResult)
            where TEntity : class,new()
        {
            var models = control.TempData.GetModelSource<TEntity>();
            //control.Response.CsvDownload(getCsvResult(models.Items), null, Encoding.GetEncoding(950), "text/csv");
            control.Response.CsvDownload(getCsvResult(models.Items), null, "text/csv");
        }

        public static string RenderViewToString<T>(this Controller controller, string viewPath, T model)
        {
            controller.ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(controller.ControllerContext, viewPath);
                var dataDict = new ViewDataDictionary<T>(model);
                var viewContext = new ViewContext(controller.ControllerContext, view, dataDict,
                                            new TempDataDictionary(), writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

        public static bool CheckSystemCompany(this UserProfileMember _userProfile)
        {
            return _userProfile.CurrentUserRole.OrganizationCategory.CategoryID == (int)Naming.CategoryID.COMP_SYS;
        }

    }

    public enum QueryType
    {
        電子發票,
        電子折讓單,
        作廢電子發票,
        作廢電子折讓單,
        中獎發票
    }
}