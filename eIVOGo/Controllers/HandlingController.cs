using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model.DataEntity;
using eIVOGo.Helper;
using Utility;
using Model.Locale;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Controllers
{
    public class HandlingController : SampleController<InvoiceItem>
    {
        protected UserProfileMember _userProfile;

        public HandlingController() : base()
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Hello,World!!";
            return View();
        }

        // GET: Handling
        public ActionResult DisableCompany(int companyID)
        {
            updateCompanyStatus(companyID, Naming.MemberStatusDefinition.Mark_To_Delete);
            return View("Index");
        }

        private void updateCompanyStatus(int companyID,Naming.MemberStatusDefinition status)
        {
            if (!_userProfile.CheckSystemCompany())
            {
                ViewBag.Message = "使用者非系統管理公司!!";
                return;
            }

            ViewBag.Message = "資料不存在!!";

            using (ModelSource<Organization> models = new ModelSource<Organization>())
            {
                OrganizationStatus item = models.GetTable<Organization>().Where(o => o.CompanyID == companyID)
                    .Select(o => o.OrganizationStatus).FirstOrDefault();
                if (item != null)
                {
                    item.CurrentLevel = (int)status;
                    models.SubmitChanges();
                    ViewBag.Message = "資料已更新!!";
                }
            }
        }

        public ActionResult EnableCompany(int companyID)
        {
            updateCompanyStatus(companyID, Naming.MemberStatusDefinition.Checked);
            return View("Index");
        }

        public ActionResult ApplyRelationship(int companyID)
        {
            var item = models.GetTable<Organization>().Where(o => o.CompanyID == companyID).FirstOrDefault();
            if (item == null)
            {
                ViewBag.Message = "資料不存在!!";
            }
            else
            {
                if(!item.IsEnterpriseGroupMember())
                {
                    models.GetTable<EnterpriseGroupMember>().InsertOnSubmit(
                        new EnterpriseGroupMember
                        {
                            EnterpriseID = (int)Naming.EnterpriseGroup.網際優勢股份有限公司,
                            CompanyID = item.CompanyID
                        });
                    models.SubmitChanges();
                    ViewBag.Message = "設定完成!!";
                }
                else
                {
                    ViewBag.Message = "設開立人已是B2B營業人!!";
                }
            }

            return View("Index");
        }
        // GET: Handling/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Handling/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Handling/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Handling/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Handling/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Handling/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
