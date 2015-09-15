using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Areas.EmailSrv.Models;

using PagedList;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class EmailContactsController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();

        private int pageSize = 10;
        //
        // GET: /EmailSrv/EmailContacts/

        public ActionResult Index(string type, string condition, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            var emailcontacts = from e in db.EmailContacts select e;

            if (!String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(condition))
            {
                switch (type)
                {
                    case "2":
                        emailcontacts = emailcontacts.Where(e => e.ContactName.ToUpper().Contains(condition.ToUpper()));
                        break;
                    case "3":
                        emailcontacts = emailcontacts.Where(e => e.ContactEmail.ToUpper().Contains(condition.ToUpper()));
                        break;
                    case "1":
                    default:
                        emailcontacts = emailcontacts.Where(e => e.ContactGroup.ToUpper().Contains(condition.ToUpper()));
                        break;
                }
            }
            emailcontacts = emailcontacts.OrderBy(e => e.Id);

            ViewBag.NumberMax = db.EmailContacts.Count();
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Type = type != null? type : "1";
            ViewBag.Condition = condition;

            return View(emailcontacts.ToPagedList(currentPage, pageSize));
        }

        public JsonResult ContactDetail(int id)
        {
            EEmailContacts emailcontacts = (id > 0) ? db.EmailContacts.Find(id) : new EEmailContacts();

            return Json(emailcontacts, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /EmailSrv/EmailContacts/Index
        [ActionName("Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDetail(EEmailContacts emailcontacts)
        {
            //if (ModelState.IsValid)
            {
                if (emailcontacts.Id == 0)
                {
                    db.EmailContacts.Add(emailcontacts);
                }
                else if (emailcontacts.Id < 0)
                {
                    emailcontacts.Id = 0 - emailcontacts.Id;
                    emailcontacts = db.EmailContacts.Find(emailcontacts.Id);
                    db.EmailContacts.Remove(emailcontacts);
                }
                else
                {
                    db.Entry(emailcontacts).State = EntityState.Modified;
                }

                db.SaveChanges();
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