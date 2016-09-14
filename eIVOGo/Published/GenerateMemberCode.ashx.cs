using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO.Packaging;
using System.IO;
using Model.InvoiceManagement;
using Model.DataEntity;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;
using Utility;

namespace eIVOGo.Published
{
    /// <summary>
    /// Summary description for GenerateMemberCode
    /// </summary>
    public class GenerateMemberCode : GetQRCode
    {

        public override void ProcessRequest(HttpContext context)
        {
            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;
            int companyID;
            if (!String.IsNullOrEmpty(Request["companyID"]) && int.TryParse(Request["companyID"], out companyID))
            {
                int count = 100;
                if (!String.IsNullOrEmpty(Request["count"]))
                    int.TryParse(Request["count"], out count);

                _dpi = 300f;
                initialize(Request);

                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", context.Server.UrlEncode("會員條碼卡.zip")));

                String zipFile = createMemberCodePackage(companyID, count);
                if (zipFile != null)
                {
                    using (FileStream fs = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
                    {
                        fs.CopyTo(Response.OutputStream);
                        fs.Close();
                    }
                }
            }
        }

        private String createMemberCodePackage(int companyID, int count)
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var orgItem = mgr.GetTable<Organization>().Where(o => o.CompanyID == companyID).FirstOrDefault();
                if (orgItem == null)
                    return null;

                var table = mgr.GetTable<MemberCode>();

                using (SHA1 hashAlg = SHA1.Create())
                {
                    String fileName = Path.Combine(Logger.LogDailyPath, String.Format("{0}({1:HHmmssffff}).zip", orgItem.ReceiptNo, DateTime.Now));
                    using (Package package =
                                    Package.Open(fileName,FileMode.Create))
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            // Add the Document part to the Package

                            MemberCode codeItem = new MemberCode
                            {
                                CompanyID = orgItem.CompanyID,
                                CreateTime = DateTime.Now,
                                UUID = Guid.NewGuid()
                            };

                            codeItem.HashID = Convert.ToBase64String(
                                hashAlg.ComputeHash(Encoding.Default.GetBytes(
                                    String.Format("{0}{1:yyyyMMdd}{2}", orgItem.ReceiptNo, codeItem.CreateTime, codeItem.UUID))));

                            table.InsertOnSubmit(codeItem);
                            mgr.SubmitChanges();

                            PackagePart imagePart =
                                package.CreatePart(PackUriHelper.CreatePartUri(
                                      new Uri(String.Format("{0:0000}.jpg", i), UriKind.Relative)),
                                               System.Net.Mime.MediaTypeNames.Image.Jpeg);

                            // Copy the data to the Document Part
                            try
                            {
                                Bitmap img = createQRCode(codeItem.HashID, _encoding, _scale, _version, _errorCorrect);
                                img.SetResolution(_dpi, _dpi);
                                img.Save(imagePart.GetStream(), ImageFormat.Jpeg);
                                img.Dispose();
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                            
                            // Add a Package Relationship to the Document Part
                            //package.CreateRelationship(imagePart.Uri,
                            //                           TargetMode.Internal,
                            //                           PackageRelationshipType);
                        }

                        package.Close();
                    }
                    return fileName;
                }
            }
        }
    }
}