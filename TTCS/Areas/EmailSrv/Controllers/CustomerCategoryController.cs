using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using TTCS.Areas.EmailSrv.Models;
using PagedList;

namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class CustomerCategoryController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();
        private int pageSize = 10;

        public ActionResult Index(int page = 0)
        {
            int currentPage = page < 1 ? 1 : page;
            IQueryable<ECustomerCategory> cc = db.CustomerCategory.OrderBy(c => c.ID);

            ViewBag.NumberMax = cc.Count();
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Desc = 0;//(Desc == 1) ? 0 : 1;

            return View(cc.ToPagedList(currentPage, pageSize));
        }


        public ActionResult Details(int id)
        {
            ECustomerCategory ECustomerCategory = db.CustomerCategory.Find(id);
            if (ECustomerCategory == null)
            {
                return HttpNotFound();
            }
            return View(ECustomerCategory);
        }

        
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(ECustomerCategory ECustomerCategory)
        {
            if (ModelState.IsValid)
            {
                ECustomerCategory.ID = ECustomerCategory.ID;
                db.CustomerCategory.Add(ECustomerCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerCategory = new SelectList(db.CustomerCategory, "ID", "Desc",
                (db.CustomerCategory.FirstOrDefault() == null ? null : db.CustomerCategory.FirstOrDefault().ID.ToString()));

            return View(ECustomerCategory);
        }

        //
        // GET: /EmailSrv/CustomersManagement/Edit/5

        public ActionResult Edit(int id)
        {
            ECustomerCategory ECustomerCategory = db.CustomerCategory.Find(id);
            if (ECustomerCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerCategory = new SelectList(db.CustomerCategory, "ID", "Desc",
                (db.CustomerCategory.FirstOrDefault() == null ? null : db.CustomerCategory.FirstOrDefault().ID.ToString()));
            return View(ECustomerCategory);
        }

        //
        // POST: /EmailSrv/CustomersManagement/Edit/5

        [HttpPost]
        public ActionResult Edit(ECustomerCategory ECustomerCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ECustomerCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ECustomerCategory);
        }

        //
        // GET: /EmailSrv/CustomersManagement/Delete/5

        public ActionResult Delete(int id)
        {
            ECustomerCategory ECustomerCategory = db.CustomerCategory.Find(id);
            if (ECustomerCategory == null)
            {
                return HttpNotFound();
            }
            return View(ECustomerCategory);
        }

        //
        // POST: /EmailSrv/CustomersManagement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            int flag = 0;
            string msg = "";

            try
            {
                flag = -1;
                ECustomerCategory ECustomerCategory = db.CustomerCategory.Find(id);

                flag = -2;
                db.CustomerCategory.Remove(ECustomerCategory);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (flag == -1)
                {
                    msg = "指定分類不存在, 例外說明:" + ex.Message;
                }
                else if (flag == -2)
                {
                    msg = "無法刪除分類，例外說明:" + ex.Message;
                }

                ViewBag.msg = msg;
                return View("DeleteOk");
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
