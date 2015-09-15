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
    public class EmailFilterRuleController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();

        private int pageSize = 10;
        //
        // GET: /EmailFilterRule/

        public ActionResult Index(string type, string condition, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            var emailfilterrule = from e in db.EmailFilterRule select e;

            if (!String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(condition))
            {
                switch (type)
                {
                    case "2":
                        emailfilterrule = emailfilterrule.Where(e => e.MsgReceivedBy.ToUpper().Contains(condition.ToUpper()));
                        break;
                    case "3":
                        emailfilterrule = emailfilterrule.Where(e => e.MsgSubject.ToUpper().Contains(condition.ToUpper()));
                        break;
                    case "4":
                        emailfilterrule = emailfilterrule.Where(e => e.MsgBody.ToUpper().Contains(condition.ToUpper()));
                        break;
                    case "1":
                    default:
                        emailfilterrule = emailfilterrule.Where(e => e.MsgFrom.ToUpper().Contains(condition.ToUpper()));
                        break;
                }
            }

            emailfilterrule = emailfilterrule.OrderBy(e => e.Id);

            ViewBag.NumberMax = db.EmailFilterRule.Count();
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Type = type == null ? "1" : type;
            ViewBag.Condition = condition;

            return View(emailfilterrule.ToPagedList(currentPage, pageSize));
        }

        public JsonResult FilterDetail(int id)
        {
            EEmailFilterRule emailfilterrule = (id > 0) ? db.EmailFilterRule.Find(id) : new EEmailFilterRule();

            return Json(emailfilterrule, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /EmailFilterRule/Index
        [ActionName("Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDetail(EEmailFilterRule emailfilterrule)
        {
            if (ModelState.IsValid)
            {
                if (emailfilterrule.Id == 0)
                {
                    db.EmailFilterRule.Add(emailfilterrule);                    
                }
                else if (emailfilterrule.Id < 0)
                {
                    emailfilterrule.Id = 0 - emailfilterrule.Id;
                    emailfilterrule = db.EmailFilterRule.Find(emailfilterrule.Id);                    
                    db.EmailFilterRule.Remove(emailfilterrule);
                }
                else
                {
                    db.Entry(emailfilterrule).State = EntityState.Modified;                    
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(db.EmailFilterRule.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}