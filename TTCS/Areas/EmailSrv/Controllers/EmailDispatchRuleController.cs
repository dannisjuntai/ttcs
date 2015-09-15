using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TTCS.Areas.EmailSrv.Models;

namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class EmailDispatchRuleController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();
        private int pageSize = 10;
        private string userGroupId;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            userGroupId = CommonUtilities.GetUserGrupID(User);
        }

        public JsonResult DispatchDetail(int id)
        {
            EEmailDispatchRule emaildispatchrule = (id > 0) ? db.EmailDispatchRule.Find(id) : new EEmailDispatchRule();

            var retobj = new 
            { 
                Id = emaildispatchrule.Id,
                ConditionType = emaildispatchrule.ConditionType,
                CustomerId = (emaildispatchrule.CustomerId == null) ? "" : emaildispatchrule.CustomerId.ToString(),
                Condition = emaildispatchrule.Condition,
                AgentId1 = String.IsNullOrEmpty(emaildispatchrule.AgentId1) ? "" : emaildispatchrule.AgentId1,
                AgentId2 = String.IsNullOrEmpty(emaildispatchrule.AgentId2) ? "" : emaildispatchrule.AgentId2,
                AgentId3 = String.IsNullOrEmpty(emaildispatchrule.AgentId3) ? "" : emaildispatchrule.AgentId3,
                Active = emaildispatchrule.AutoDispatch
                
            };
            return Json(retobj, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /EmailFilterRule/Index
        [ActionName("Index")]
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDetail(EEmailDispatchRule emaildispatchrule)
        {
            if (emaildispatchrule.Id < 0)
            {
                emaildispatchrule.Id = 0 - emaildispatchrule.Id;
                emaildispatchrule = db.EmailDispatchRule.Find(emaildispatchrule.Id);
                db.EmailDispatchRule.Remove(emaildispatchrule);
                db.SaveChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    emaildispatchrule.ModifyOn = System.DateTime.Now;
                    if (emaildispatchrule.ConditionType.Value == 1)
                    {
                        if (emaildispatchrule.Id == 0)
                        {
                            EEmailDispatchRule found =
                               db.EmailDispatchRule.FirstOrDefault(d => d.CustomerId == emaildispatchrule.CustomerId);

                            if (found != null)
                            {                 
                                return RedirectToAction("Index", new { cid = emaildispatchrule.CustomerId, err = 0 });
                            }
                        }
                        emaildispatchrule.Condition = "";
                    }
                    else if (emaildispatchrule.ConditionType.Value == 1)
                    {
                        if (emaildispatchrule.Id == 0)
                        {
                            string msgfrom = emaildispatchrule.Condition;
                            ECustomers found =
                                db.Customers.FirstOrDefault(
                                    d => d.Contact1_Email1.Contains(msgfrom) ||
                                         d.Contact1_Email2.Contains(msgfrom) ||
                                         d.Contact2_Email1.Contains(msgfrom) ||
                                         d.Contact2_Email2.Contains(msgfrom) ||
                                         d.Contact3_Email1.Contains(msgfrom) ||
                                         d.Contact3_Email2.Contains(msgfrom));

                            if (found != null)
                            {                                
                                return RedirectToAction("Index", new { cid = found.ID, err = 1 });
                            }
                        }
                        emaildispatchrule.CustomerId = null;
                    }
                    else if (emaildispatchrule.ConditionType.Value > 1)
                    {
                        emaildispatchrule.CustomerId = null;
                    }

                    if (emaildispatchrule.Id == 0)
                    {
                        db.EmailDispatchRule.Add(emaildispatchrule);
                    }
                    else if (emaildispatchrule.Id < 0)
                    {
                        emaildispatchrule.Id = 0 - emaildispatchrule.Id;
                        emaildispatchrule = db.EmailDispatchRule.Find(emaildispatchrule.Id);
                        db.EmailDispatchRule.Remove(emaildispatchrule);
                    }
                    else
                    {
                        db.Entry(emaildispatchrule).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("Index", new { err = -1 });
                }
            }
            return RedirectToAction("Index");
        }

        private IEnumerable<EEmailDispatchRule> GetIndexObject(System.Guid? cid, int? err)
        {
            List<EEmailDispatchRule> result = new List<EEmailDispatchRule>();
            var emaildispatchrules = db.EmailDispatchRule.ToList();
            
            foreach (var emaildispatch in emaildispatchrules)
            {
                if (emaildispatch.CustomerId == null)
                {
                    emaildispatch.DispatchCondition = emaildispatch.Condition;
                    emaildispatch.ModifyBy = db.Agent.Find(emaildispatch.ModifyBy).AgentName;
                    result.Add(emaildispatch);
                }
                else
                {
                    if (emaildispatch.Customers != null)
                    {
                        if (userGroupId.ToUpper().Contains("SUPER") || 
                            (emaildispatch.Customers.GroupID != null &&
                             (userGroupId.ToUpper().Contains(emaildispatch.Customers.GroupID.ToUpper()) ||
                              emaildispatch.Customers.GroupID.ToUpper().Contains(userGroupId.ToUpper()))))
                        {
                            emaildispatch.DispatchCondition = emaildispatch.Customers.CName;
                            var agent = db.Agent.Find(emaildispatch.ModifyBy);
                            if(agent != null)
                                emaildispatch.ModifyBy = agent.AgentName;
                                
                            result.Add(emaildispatch);
                        }
                    }
                }
            }

            ViewBag.Customers = 
                new SelectList(db.Customers.Where(c => userGroupId.ToUpper().Contains("SUPER") ||
                                                        (c.GroupID != null && 
                                                         (userGroupId.ToUpper().Contains(c.GroupID.ToUpper()) || 
                                                         c.GroupID.ToUpper().Contains(userGroupId.ToUpper())))), "ID", "CName");
            
            ViewBag.AgentsSuper = CommonUtilities.GetAgentList(db, userGroupId);

            List<SelectListItem> radiochoices = new List<SelectListItem>();
            radiochoices.Add(new SelectListItem() { Text = "客戶", Value = "1" });
            radiochoices.Add(new SelectListItem() { Text = "信箱", Value = "2" });
            radiochoices.Add(new SelectListItem() { Text = "網域", Value = "3" });
            radiochoices.Add(new SelectListItem() { Text = "主旨", Value = "4" });

            ViewBag.RadioChoices = radiochoices;
            ViewBag.NumberMax = emaildispatchrules.Count();

            if (err != null)
            {
                switch (err.Value)
                {
                    case 0:
                        ViewBag.ErrMsg = String.Format("此客戶已有分派條件, 請改用'編輯' [客戶名稱:{0}]", db.Customers.Find(cid).CName);
                        break;
                    case 1:
                        ViewBag.ErrMsg = String.Format("客戶資料中己存有此條件, 請改用'客戶'為分派條件 [客戶名稱:{0}]", db.Customers.Find(cid).CName);
                        break;
                    default:
                        ViewBag.ErrMsg = "請輸入任一條件值!!";
                        break;
                }
            }
            return result;
        }

        //
        // GET: /EmailDispatchRule/
        public ActionResult Index(System.Guid? cid, int? err)
        {
            return View(GetIndexObject(cid, err));
        }

        [HttpPost]
        public JsonResult GetAgentList(System.Guid? cid)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
            items.Add(new KeyValuePair<string, string>("", "--請選擇--"));

            if (cid != null)
            {
                var customer = db.Customers.Find(cid);
                string strGroupID = "";
                if (customer != null && customer.GroupID != null)
                    strGroupID = customer.GroupID;

                var mailmember = db.MailMember.Where(mm => mm.GroupID == strGroupID);
                foreach (var mm in mailmember)
                {
                    var agent = db.Agent.FirstOrDefault(a => a.CTILoginID == mm.CTILoginID);
                    if (agent == null)
                        continue;

                    items.Add(new KeyValuePair<string, string>(agent.AgentID, agent.AgentName));
                }
                return this.Json(items);
            }
            else
            {
                List<SelectListItem> agentList = CommonUtilities.GetAgentList(db, userGroupId);

                var result = "<option value=''>--請選擇--</option>";
                foreach (var agent in agentList)
                {
                    string[] arrOption = agent.Text.Split('|');
                    var optionlist = "";
                    foreach (var option in arrOption)
                    {
                        string[] arrKeyValue = option.Split(':');
                        optionlist += String.Format("<option value='{0}'>{1}</option>", arrKeyValue[0], arrKeyValue[1]);
                    }
                    result += String.Format("<optgroup label='{0}'>{1}</optgroup>", agent.Value, optionlist);
                }
                return this.Json(result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}