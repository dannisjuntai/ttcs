using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Areas.EmailSrv.Models;

using PagedList;
using System.Web.Security;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();
        private int pageSize = 10;
        private string userGroupId;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            userGroupId = CommonUtilities.GetUserGrupID(User);
        }
        //
        // GET: /Customers/

        public ActionResult Index(string type, string condition, int isAssignRule=0, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            IEnumerable<CustomersView> customersview =
                from c in db.Customers
                join d in db.EmailDispatchRule
                on c.ID equals d.CustomerId into cd
                from dview in cd.DefaultIfEmpty()
                select new CustomersView()
                {
                    Customers = c,
                    EmailDispatchRule = dview
                };

            foreach (var customer in customersview)
            {
                if (customer.EmailDispatchRule != null && customer.EmailDispatchRule.AgentId1 != null && customer.EmailDispatchRule.AgentId1.Length > 0)
                {
                    customer.Customers.IsAssignRule = 1;
                }
                else
                {
                    customer.Customers.IsAssignRule = 0;
                }
            }


            if (!String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(condition))
            {
                switch (type)
                {
                    // Email
                    case "2":
                        customersview = customersview.Where(c => (c.Customers.Contact1_Email1 != null && c.Customers.Contact1_Email1.ToUpper().Contains(condition.ToUpper())) ||
                                                          (c.Customers.Contact1_Email2 != null && c.Customers.Contact1_Email2.ToUpper().Contains(condition.ToUpper())) ||
                                                          (c.Customers.Contact2_Email1 != null && c.Customers.Contact2_Email1.ToUpper().Contains(condition.ToUpper())) ||
                                                          (c.Customers.Contact2_Email2 != null && c.Customers.Contact2_Email2.ToUpper().Contains(condition.ToUpper())) ||
                                                          (c.Customers.Contact3_Email1 != null && c.Customers.Contact3_Email1.ToUpper().Contains(condition.ToUpper())) ||
                                                          (c.Customers.Contact3_Email2 != null && c.Customers.Contact3_Email2.ToUpper().Contains(condition.ToUpper())));
                        break;

                    // 電話
                    case "3":
                        customersview = customersview.Where(c => (c.Customers.Contact1_Tel1 != null && c.Customers.Contact1_Tel1.ToUpper().Contains(condition.ToUpper())) ||
                                                         (c.Customers.Contact1_Tel2 != null && c.Customers.Contact1_Tel2.ToUpper().Contains(condition.ToUpper())) ||
                                                         (c.Customers.Contact2_Tel1 != null && c.Customers.Contact2_Tel1.ToUpper().Contains(condition.ToUpper())) ||
                                                         (c.Customers.Contact2_Tel2 != null && c.Customers.Contact2_Tel2.ToUpper().Contains(condition.ToUpper())) ||
                                                         (c.Customers.Contact3_Tel1 != null && c.Customers.Contact3_Tel1.ToUpper().Contains(condition.ToUpper())) ||
                                                         (c.Customers.Contact3_Tel2 != null && c.Customers.Contact3_Tel2.ToUpper().Contains(condition.ToUpper())));
                        break;

                    // 客戶名稱
                    case "1":
                    default:
                        customersview = customersview.Where(c => (c.Customers.CName != null && c.Customers.CName.ToUpper().Contains(condition.ToUpper())) ||
                                                        (c.Customers.EName != null && c.Customers.EName.ToUpper().Contains(condition.ToUpper())));
                        break;
                }
            }
            if (isAssignRule> 0)
            {
                customersview = customersview.Where(c => c.Customers.IsAssignRule > 0);
            }

            #region Authority
            if (!userGroupId.ToUpper().Contains("SUPER"))
            {
                customersview = customersview.Where(c => c.Customers.GroupID != null && 
                                                        (userGroupId.ToUpper().Contains(c.Customers.GroupID.ToUpper()) ||
                                                        c.Customers.GroupID.ToUpper().Contains(userGroupId.ToUpper())));
            }
            #endregion
            customersview = customersview.OrderBy(c => c.Customers.ID);

            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Type = type == null ? "1" : type;
            ViewBag.Condition = condition;
            ViewBag.IsAssignRule = isAssignRule;

            return View(customersview.ToPagedList(currentPage, pageSize));
        }

        public ActionResult _PartialOpenPopup(Guid? id)
        {            
            var emaildispatchrule = db.EmailDispatchRule.FirstOrDefault(e => e.CustomerId == id);
            
            var customer = db.Customers.Find(id);
            string strGroupID = "";
            if (customer != null && customer.GroupID != null)
                strGroupID = customer.GroupID;

            ViewBag.AgentsSuper = CommonUtilities.GetAgentList(db, strGroupID);;
            if (emaildispatchrule != null)
            {
                return PartialView("~/Areas/EmailSrv/Views/Customers/_PartialOpenPopup.cshtml", emaildispatchrule);
            }
            else
            {
                EEmailDispatchRule emaildispatchRule = new EEmailDispatchRule();
                emaildispatchRule.CustomerId = id;
                emaildispatchRule.Customers = db.Customers.Find(id);
                emaildispatchRule.AutoDispatch = true;

                return PartialView("~/Areas/EmailSrv/Views/Customers/_PartialOpenPopup.cshtml", emaildispatchRule);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialOpenPopup(EEmailDispatchRule emaildispatchrule, int DispatchId)
        {            
            if(DispatchId < 1)
            {
                emaildispatchrule.ModifyOn = DateTime.Now;
                emaildispatchrule.ConditionType = 1;
                db.EmailDispatchRule.Add(emaildispatchrule);
                db.SaveChanges();
            }
            else
            {
                emaildispatchrule.Id = DispatchId;
                if ((emaildispatchrule.AgentId1 == null || emaildispatchrule.AgentId1.Length < 1) &&
                    (emaildispatchrule.AgentId2 == null || emaildispatchrule.AgentId2.Length < 1) &&
                    (emaildispatchrule.AgentId3 == null || emaildispatchrule.AgentId3.Length < 1))
                {
                    db.EmailDispatchRule.Remove(db.EmailDispatchRule.Find(emaildispatchrule.Id));
                }
                else
                {
                    emaildispatchrule.ModifyOn = DateTime.Now;
                    db.Entry(emaildispatchrule).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Customers", new { Area="EmailSrv" });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}