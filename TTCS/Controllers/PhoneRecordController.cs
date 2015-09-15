using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using TTCS.Models;
using TTCS.App_Start;
using System.Data.Entity.Validation;
using Newtonsoft.Json;

namespace TTCS.Controllers
{
    public class PhoneRecordController : Controller
    {
        private kskyEntities db = new kskyEntities();

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AjaxQueryByIndex()
        {
            int flag = -1;
            string qs = String.IsNullOrEmpty(Request.QueryString["qs"]) ? "" : Request.QueryString["qs"].ToString();
            string RID = null;
            string CustomerID = null;
            string message = "";

            try
            {
                IQueryable<PhoneRecord> phoneRecords = db.PhoneRecord.Where(p => false);

                try
                {
                    
                    phoneRecords = db.PhoneRecord.Where(p => p.IndexID == qs);
                }
                catch (Exception ex)
                {
                    return Content(JsonConvert.SerializeObject(new { result = flag, RID = RID, msg = Helper.FetchExceptionMessage(ex)}));
                }

                flag = phoneRecords.Count();

                if (flag > 0)
                {
                    RID = phoneRecords.First().RID;
                    CustomerID = phoneRecords.First().Records.CustomerID.ToString();
                }
            }
            catch (Exception exp)
            {
                flag = -2;
                message = Helper.FetchExceptionMessage(exp);
            }

            var ret = new
            {
                result = flag,
                RID = RID,
                CustomerID = CustomerID,
                msg = message
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }

        [HttpPost]
        public ActionResult AjaxCreate(PhoneRecord id)
        {
            int flag_success = -1;
            int error_count = 0;
            string info = "";
            id.PhoneNum = String.IsNullOrEmpty(id.PhoneNum) ? "" : id.PhoneNum;
            id.QueueName = String.IsNullOrEmpty(id.QueueName) ? "" : id.QueueName;
            id.IndexID = String.IsNullOrEmpty(id.IndexID) ? "" : id.IndexID;

            try
            {
                if (ModelState.IsValid)
                {
                    db.PhoneRecord.Add(id);
                    db.SaveChanges();

                    while (true)
                    {
                        try
                        {
                            db.SaveChanges();
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (error_count >= Def.MaxRetry)
                            {
                                info += string.Format("Retry {0} failure", Def.MaxRetry);
                                throw ex;
                            }
                            error_count++;
                            Thread.Sleep(Def.RetryWait);
                        }
                    }
                    flag_success = 0;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string err = "";
                foreach (DbEntityValidationResult result in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in result.ValidationErrors)
                    {
                        err += error.ErrorMessage + "|\n";
                    }
                }
                info += ex.Message + "|\n|" + err + "|\n|";
            }
            catch (Exception ex)
            {
                info += Helper.FetchExceptionMessage(ex);
            }


            var ret = new
            {
                result = flag_success,
                info = info
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }

        #region unused func
        /*
        //
        // GET: /PhoneRecord/

        public ActionResult Index()
        {
            var phonerecord = db.PhoneRecord.Include(p => p.Records);
            return View(phonerecord.ToList());
        }

        //
        // GET: /PhoneRecord/Details/5

        public ActionResult Details(string id = null)
        {
            PhoneRecord phonerecord = db.PhoneRecord.Find(id);
            if (phonerecord == null)
            {
                return HttpNotFound();
            }
            return View(phonerecord);
        }

        //
        // GET: /PhoneRecord/Create

        public ActionResult Create()
        {
            ViewBag.RID = new SelectList(db.Records, "RID", "AgentID");
            return View();
        }

        //
        // POST: /PhoneRecord/Create

        [HttpPost]
        public ActionResult Create(PhoneRecord phonerecord)
        {
            if (ModelState.IsValid)
            {
                db.PhoneRecord.Add(phonerecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RID = new SelectList(db.Records, "RID", "AgentID", phonerecord.RID);
            return View(phonerecord);
        }

        //
        // GET: /PhoneRecord/Edit/5

        public ActionResult Edit(string id = null)
        {
            PhoneRecord phonerecord = db.PhoneRecord.Find(id);
            if (phonerecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.RID = new SelectList(db.Records, "RID", "AgentID", phonerecord.RID);
            return View(phonerecord);
        }

        //
        // POST: /PhoneRecord/Edit/5

        [HttpPost]
        public ActionResult Edit(PhoneRecord phonerecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phonerecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RID = new SelectList(db.Records, "RID", "AgentID", phonerecord.RID);
            return View(phonerecord);
        }

        //
        // GET: /PhoneRecord/Delete/5

        public ActionResult Delete(string id = null)
        {
            PhoneRecord phonerecord = db.PhoneRecord.Find(id);
            if (phonerecord == null)
            {
                return HttpNotFound();
            }
            return View(phonerecord);
        }

        //
        // POST: /PhoneRecord/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            PhoneRecord phonerecord = db.PhoneRecord.Find(id);
            db.PhoneRecord.Remove(phonerecord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         */
        #endregion


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}