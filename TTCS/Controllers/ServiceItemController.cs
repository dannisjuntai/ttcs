using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Models;
using Newtonsoft.Json;

using TTCS.App_Start;

namespace TTCS.Controllers
{
    public class ServiceItemController : Controller
    {
        private kskyEntities db = new kskyEntities();

        #region Unused
        /*
        //
        // GET: /ServiceItem/
        public ActionResult Index()
        {
            return View(db.ServiceGroup.ToList());
        }

        //
        // GET: /ServiceItem/Details/5

        public ActionResult Details(int id = 0)
        {
            ServiceGroup servicegroup = db.ServiceGroup.Find(id);
            if (servicegroup == null)
            {
                return HttpNotFound();
            }
            return View(servicegroup);
        }

        //
        // GET: /ServiceItem/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ServiceItem/Create

        [HttpPost]
        public ActionResult Create(ServiceGroup servicegroup)
        {
            if (ModelState.IsValid)
            {
                db.ServiceGroup.Add(servicegroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicegroup);
        }

        //
        // GET: /ServiceItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ServiceGroup servicegroup = db.ServiceGroup.Find(id);
            if (servicegroup == null)
            {
                return HttpNotFound();
            }
            return View(servicegroup);
        }

        //
        // POST: /ServiceItem/Edit/5

        [HttpPost]
        public ActionResult Edit(ServiceGroup servicegroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicegroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicegroup);
        }

        //
        // GET: /ServiceItem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ServiceGroup servicegroup = db.ServiceGroup.Find(id);
            if (servicegroup == null)
            {
                return HttpNotFound();
            }
            return View(servicegroup);
        }

        //
        // POST: /ServiceItem/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceGroup servicegroup = db.ServiceGroup.Find(id);
            db.ServiceGroup.Remove(servicegroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
#endregion

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult JsonServiceTree(string id)
        {
            List<ServiceGroup> service_list =
                (from s in db.ServiceGroup
                join a in db.Agent on s.DeptID equals a.DeptID into o_agent
                from a in o_agent.DefaultIfEmpty()
                where
                    a.AgentID == id
                select s).ToList();
            //return Content(JsonConvert.SerializeObject(service_list), Def.JsonMimeType);
            return Json(ServiceToObject(service_list), JsonRequestBehavior.AllowGet);
        }

        private List<Object> ServiceToObject(List<ServiceGroup> group_list)
        {
            List<Object> ret_groups = new List<object>();

            foreach (ServiceGroup sg in group_list)
            {
                List<Object> ret_items = new List<object>();
                foreach (ServiceItem si in sg.ServiceItem)
                {
                    ret_items.Add(
                        new {
                            title = si.ItemDesc,
                            group_id = si.GroupID,
                            item_id = si.ItemID
                        }
                    );
                }
                ret_groups.Add(
                    new { 
                        title = sg.GroupDesc,
                        hideCheckbox = true,
                        children = ret_items
                    }
                );
            }

            return ret_groups;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}