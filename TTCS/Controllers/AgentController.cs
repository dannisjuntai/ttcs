using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Models;

using TTCS.App_Start;
using Newtonsoft.Json;

namespace TTCS.Controllers
{
    public class AgentController : Controller
    {
        private kskyEntities db = new kskyEntities();

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetAgentIDByCTI(string id)
        {
            IQueryable<Agent> agents = 
                from a in db.Agent
                where a.CTILoginID == id
                select a;

            string agent_id = agents.First().AgentID;
            string login_id = agents.First().CTILoginID;
            IQueryable<Object> groups =
                from m in db.MailMember
                join g in db.MailGroup on m.GroupID equals g.GroupID into o_group
                from g in o_group.DefaultIfEmpty()
                where m.CTILoginID == login_id
                select new { 
                    GroupID = g.GroupID,
                    GroupName = g.GroupName
                };

            var ret = new {
                agent_id = agent_id,
                agent_group = groups
            };
            
            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }

        //
        // GET: /Agent/

        public ActionResult Index()
        {
            return View(db.Agent.ToList());
        }

        //
        // GET: /Agent/Details/5

        public ActionResult Details(string id = null)
        {
            Agent agent = db.Agent.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        //
        // GET: /Agent/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Agent/Create

        [HttpPost]
        public ActionResult Create(Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Agent.Add(agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agent);
        }

        //
        // GET: /Agent/Edit/5

        public ActionResult Edit(string id = null)
        {
            Agent agent = db.Agent.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        //
        // POST: /Agent/Edit/5

        [HttpPost]
        public ActionResult Edit(Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        //
        // GET: /Agent/Delete/5

        public ActionResult Delete(string id = null)
        {
            Agent agent = db.Agent.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        //
        // POST: /Agent/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Agent agent = db.Agent.Find(id);
            db.Agent.Remove(agent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}