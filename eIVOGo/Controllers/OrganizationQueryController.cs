using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Business.Helper;
using eIVOGo.Helper;
using eIVOGo.Models;
using Model.DataEntity;
using Model.Security.MembershipManagement;

namespace eIVOGo.Controllers
{
    public class OrganizationQueryController : SampleController<InvoiceItem>
    {
        protected ModelSourceInquiry<Organization> createModelInquiry()
        {
            UserProfileMember userProfile = WebPageUtility.UserProfile;

            return (new InquireOrganizationReceiptNo { CurrentController = this })
                .Append(new InquireOrganizationStatus { CurrentController = this })
                .Append(new InquireCompanyName { CurrentController = this });

        }
        // GET: OrganizationQuery
        public ActionResult Index()
        {
            ModelSource<Organization> models = new ModelSource<Organization>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();

            return View(models.Inquiry);
        }

        public ActionResult Inquire()
        {
            //ViewBag.HasQuery = true;
            //ViewBag.PrintAction = "PrintResult";
            ModelSource<Organization> models = new ModelSource<Organization>();
            TempData.SetModelSource(models);
            models.Items = models.Items.Where(o => o.OrganizationStatus != null);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            return View("InquiryResult", models.Inquiry);
        }

        public ActionResult InquireCompany(int? pageIndex)
        {
            //ViewBag.HasQuery = true;
            //ViewBag.PrintAction = "PrintResult";
            ModelSource<Organization> models = new ModelSource<Organization>();
            TempData.SetModelSource(models);
            models.Items = models.Items.Where(o => o.OrganizationStatus != null);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            if (pageIndex.HasValue)
            {
                ViewBag.PageIndex = pageIndex - 1;
                return View("~/Views/Module/CompanyList.ascx", models.Items);
            }
            else
            {
                ViewBag.PageIndex = 0;
                return View("~/Views/OrganizationQuery/Module/CompanyResult.ascx", models.Inquiry);
            }
        }


        public ActionResult GridPage(int index, int size)
        {
            //ViewBag.HasQuery = true;
            ModelSource<Organization> models = new ModelSource<Organization>();
            TempData.SetModelSource(models);
            models.Inquiry = createModelInquiry();
            models.BuildQuery();

            if (index > 0)
                index--;
            else
                index = 0;

            return View(models.Items.OrderByDescending(d => d.ReceiptNo)
                .Skip(index * size).Take(size)
                .ToArray());
        }

        // POST: OrganizationQuery/Create
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

        // GET: OrganizationQuery/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrganizationQuery/Edit/5
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

        // GET: OrganizationQuery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrganizationQuery/Delete/5
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
