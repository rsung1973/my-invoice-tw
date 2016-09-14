using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Helper;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.EIVO
{
    public partial class ExportInvoiceItem : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;
        protected int _counter = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected virtual void btnExport_Click(object sender, EventArgs e)
        {
            switch (rbType.SelectedIndex)
            {
                case 0:
                    downloadInvoices();
                    break;
                case 1:
                    downloadAllowances();
                    break;
                case 2:
                    downloadInvoiceCancellations();
                    break;
                case 3:
                    downloadAllowanceCancellations();
                    break;
            }
        }

        protected void downloadAllowanceCancellations()
        {
            var mgr = dsEntity.CreateDataManager();
            var table = mgr.GetTable<DocumentDispatch>();
            var items = buildAllowanceCancellationQuery(table);


            if (items.Count() > 0)
            {
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode("傳送大平台作廢折讓資料.zip")));

                //using (Stream invStream = zipData(items.Select(d => d.CDS_Document.InvoiceAllowance.BuildB0201()), a => String.Format("{0}.xml", a.CancelAllowanceNumber)))
                //{
                //    invStream.CopyTo(Response.OutputStream);
                //    invStream.Close();
                //}
                writeZipData(items.Select(d => d.CDS_Document.InvoiceAllowance.BuildB0201()), a => String.Format("B0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, _counter++), Response.OutputStream);

                var logs = mgr.GetTable<DocumentDownloadLog>();
                foreach (var item in items)
                {
                    logs.InsertOnSubmit(new DocumentDownloadLog
                    {
                        DocID = item.DocID,
                        TypeID = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                        DownloadDate = DateTime.Now,
                        UID = _userProfile.UID
                    });
                }

                table.DeleteAllOnSubmit(items);
                mgr.SubmitChanges();

            }
            else
            {
                this.AjaxAlert("沒有資料可供下載!!");
            }
        }

        protected IQueryable<DocumentDispatch> buildAllowanceCancellationQuery(System.Data.Linq.Table<DocumentDispatch> table)
        {
            var items = table.Where(d => d.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && d.TypeID == (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation);

            if (DateFrom.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate < DateTo.DateTimeValue.AddDays(1));
            }
            return items;
        }

        protected void downloadInvoiceCancellations()
        {
            var mgr = dsEntity.CreateDataManager();
            var table = mgr.GetTable<DocumentDispatch>();
            var items = buildInvoiceCancellationQuery(table);


            if (items.Count() > 0)
            {
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode("傳送大平台作廢發票資料.zip")));

                //using (Stream invStream = zipData(items.Select(d => d.CDS_Document.InvoiceItem.BuildA0201()), a => String.Format("{0}.xml", a.CancelInvoiceNumber)))
                //{
                //    invStream.CopyTo(Response.OutputStream);
                //    invStream.Close();
                //}
                writeZipData(items.Select(d => d.CDS_Document.InvoiceItem.BuildA0201()), a => String.Format("A0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, _counter++), Response.OutputStream);

                var logs = mgr.GetTable<DocumentDownloadLog>();
                foreach (var item in items)
                {
                    logs.InsertOnSubmit(new DocumentDownloadLog
                    {
                        DocID = item.DocID,
                        TypeID = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                        DownloadDate = DateTime.Now,
                        UID = _userProfile.UID
                    });
                }



                table.DeleteAllOnSubmit(items);
                mgr.SubmitChanges();

            }
            else
            {
                this.AjaxAlert("沒有資料可供下載!!");
            }

        }

        protected IQueryable<DocumentDispatch> buildInvoiceCancellationQuery(System.Data.Linq.Table<DocumentDispatch> table)
        {
            var items = table.Where(d => d.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && d.TypeID == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation);

            if (DateFrom.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceItem.InvoiceCancellation.CancelDate >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceItem.InvoiceCancellation.CancelDate < DateTo.DateTimeValue.AddDays(1));
            }
            return items;
        }

        protected void downloadAllowances()
        {
            var mgr = dsEntity.CreateDataManager();
            var table = mgr.GetTable<DocumentDispatch>();
            var items = buildAllowanceQuery(table);


            if (items.Count() > 0)
            {
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode("傳送大平台發票折讓資料.zip")));

                //using (Stream invStream = zipData(items.Select(d => d.CDS_Document.InvoiceAllowance.BuildB0101()), a => String.Format("{0}.xml", a.Main.AllowanceNumber)))
                //{
                //    invStream.CopyTo(Response.OutputStream);
                //    invStream.Close();
                //}

                writeZipData(items.Select(d => d.CDS_Document.InvoiceAllowance.BuildB0101()), a => String.Format("B0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, _counter++), Response.OutputStream);

                var logs = mgr.GetTable<DocumentDownloadLog>();
                foreach (var item in items)
                {
                    logs.InsertOnSubmit(new DocumentDownloadLog
                    {
                        DocID = item.DocID,
                        TypeID = (int)Naming.DocumentTypeDefinition.E_Allowance,
                        DownloadDate = DateTime.Now,
                        UID = _userProfile.UID
                    });
                }



                table.DeleteAllOnSubmit(items);
                mgr.SubmitChanges();

            }
            else
            {
                this.AjaxAlert("沒有資料可供下載!!");
            }

        }

        protected IQueryable<DocumentDispatch> buildAllowanceQuery(System.Data.Linq.Table<DocumentDispatch> table)
        {
            var items = table.Where(d => d.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && d.TypeID == (int)Naming.DocumentTypeDefinition.E_Allowance);

            if (DateFrom.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceAllowance.AllowanceDate >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceAllowance.AllowanceDate < DateTo.DateTimeValue.AddDays(1));
            }
            return items;
        }

        protected void downloadInvoices()
        {
            var mgr = dsEntity.CreateDataManager();
            var table = mgr.GetTable<DocumentDispatch>();
            var items = buildInvoiceQuery(table);


            if (items.Count() > 0)
            {
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode("傳送大平台發票資料.zip")));

                //using (Stream invStream = zipData(items.Select(d => d.CDS_Document.InvoiceItem.BuildA0101()), a => String.Format("{0}.xml", a.Main.InvoiceNumber)))
                //{
                //    invStream.CopyTo(Response.OutputStream);
                //    invStream.Close();
                //}
                writeZipData(items.Select(d => d.CDS_Document.InvoiceItem.BuildA0101()), a => String.Format("A0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, _counter++), Response.OutputStream);

                var logs = mgr.GetTable<DocumentDownloadLog>();
                foreach (var item in items)
                {
                    logs.InsertOnSubmit(new DocumentDownloadLog
                    {
                        DocID = item.DocID,
                        TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice,
                        DownloadDate = DateTime.Now,
                        UID = _userProfile.UID
                    });
                }

                table.DeleteAllOnSubmit(items);
                mgr.SubmitChanges();

            }
            else
            {
                this.AjaxAlert("沒有資料可供下載!!");
            }


        }

        protected IQueryable<DocumentDispatch> buildInvoiceQuery(System.Data.Linq.Table<DocumentDispatch> table)
        {
            var items = table.Where(d => d.CDS_Document.DocumentOwner.OwnerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID && d.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice);

            if (DateFrom.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceItem.InvoiceDate >= DateFrom.DateTimeValue);
            }
            if (DateTo.HasValue)
            {
                items = items.Where(d => d.CDS_Document.InvoiceItem.InvoiceDate < DateTo.DateTimeValue.AddDays(1));
            }
            return items;
        }

        //private Stream zipData<T>(IEnumerable<T> items, Func<T, String> getFileName)
        //{
        //    MemoryStream ms = new MemoryStream(4096);
        //    using (Package package =
        //                    Package.Open(ms, FileMode.Create))
        //    {
        //        foreach (var item in items)
        //        {
        //            // Add the Document part to the Package

        //            PackagePart imagePart =
        //                package.CreatePart(PackUriHelper.CreatePartUri(
        //                      new Uri(getFileName(item), UriKind.Relative)),
        //                               System.Net.Mime.MediaTypeNames.Text.Xml);

        //            // Copy the data to the Document Part
        //            try
        //            {
        //                item.ConvertToXml().Save(imagePart.GetStream());
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.Error(ex);
        //            }

        //        }
        //        package.Close();
        //    }

        //    ms.Seek(0, SeekOrigin.Begin);
        //    return ms;
        //}

        private void writeZipData<T>(IEnumerable<T> items, Func<T, String> getFileName, Stream outputStream)
        {
            List<String> files = new List<string>();
            foreach (var item in items)
            {
                try
                {
                    String fileName = Path.Combine(Logger.LogDailyPath, getFileName(item));
                    item.ConvertToXml().Save(fileName);
                    files.Add(fileName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            using (Ionic.Zip.ZipFile zip =
                            new Ionic.Zip.ZipFile())
            {
                zip.AddFiles(files, "");
                zip.Save(outputStream);
            }

            foreach (var file in files)
            {
                File.Delete(file);
            }

        }
    }
}