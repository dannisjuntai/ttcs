using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class CustomerController : Controller
    {
        private kskyEntities db = new kskyEntities();

        #region AJAX Action
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AjaxNewGuid()
        {
            Guid new_guid = Guid.NewGuid();
            dynamic ret;
            int result = 0;
            string message = "";

            try
            {
                while (db.Customers.Where(c => c.ID == new_guid).Count() != 0)
                {
                    new_guid = Guid.NewGuid();
                }
            }
            catch (Exception ex)
            {
                message = Helpers.Error.FetchExceptionMessage(ex);
            }

            ret = Helpers.Ajax.GetAjaxRet(result, message);
            ret.guid = new_guid;
            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }


        [HttpPost]
        public ActionResult AjaxCreate(Customers customer, string[] agent_groups)
        {
            int flag = 0;
            string msg = "";
            try
            {
                flag = -1;
                if (CheckDuplicate(customer, agent_groups) == true)
                {
                    flag = -2;
                    //因錯誤訊息與AjaxUpdate 一樣，增加 Create 區別 dannis 2015/8/20 
                    msg += "Create Customer telphone or email is duplication\n";
                }
                else
                {
                    flag = -3;
                    customer.ModifiedDT = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        flag = -4;
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        flag = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                msg += Helpers.Error.FetchExceptionMessage(ex);
            }

            var ret = new
            {
                result = flag,
                msg = msg
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }


        private bool CheckDuplicate(Customers customer, string[] agent_groups)
        {
            Guid id = customer.ID;
            string tel_11 = customer.Contact1_Tel1;
            string tel_12 = customer.Contact1_Tel2;
            string email_11 = customer.Contact1_Email1;
            string email_12 = customer.Contact1_Email2;
            string tel_21 = customer.Contact2_Tel1;
            string tel_22 = customer.Contact2_Tel2;
            string email_21 = customer.Contact2_Email1;
            string email_22 = customer.Contact2_Email2;
            string tel_31 = customer.Contact3_Tel1;
            string tel_32 = customer.Contact3_Tel2;
            string email_31 = customer.Contact3_Email1;
            string email_32 = customer.Contact3_Email2;

            int count =
                (from c in db.Customers
                where
                 c.ID != id &&
                 agent_groups.Contains(c.GroupID) &&
                ((string.IsNullOrEmpty(tel_11) ? false : (c.Contact1_Tel1 == tel_11 || c.Contact1_Tel2 == tel_11 || c.Contact2_Tel1 == tel_11 || c.Contact2_Tel2 == tel_11 || c.Contact3_Tel1 == tel_11 || c.Contact3_Tel2 == tel_11)) ||
                (string.IsNullOrEmpty(tel_12) ? false : (c.Contact1_Tel1 == tel_12 || c.Contact1_Tel2 == tel_12 || c.Contact2_Tel1 == tel_12 || c.Contact2_Tel2 == tel_12 || c.Contact3_Tel1 == tel_12 || c.Contact3_Tel2 == tel_12)) ||
                (string.IsNullOrEmpty(tel_21) ? false : (c.Contact1_Tel1 == tel_21 || c.Contact1_Tel2 == tel_21 || c.Contact2_Tel1 == tel_21 || c.Contact2_Tel2 == tel_21 || c.Contact3_Tel1 == tel_21 || c.Contact3_Tel2 == tel_21)) ||
                (string.IsNullOrEmpty(tel_22) ? false : (c.Contact1_Tel1 == tel_22 || c.Contact1_Tel2 == tel_22 || c.Contact2_Tel1 == tel_22 || c.Contact2_Tel2 == tel_22 || c.Contact3_Tel1 == tel_22 || c.Contact3_Tel2 == tel_22)) ||
                (string.IsNullOrEmpty(tel_31) ? false : (c.Contact1_Tel1 == tel_31 || c.Contact1_Tel2 == tel_31 || c.Contact2_Tel1 == tel_31 || c.Contact2_Tel2 == tel_31 || c.Contact3_Tel1 == tel_31 || c.Contact3_Tel2 == tel_31)) ||
                (string.IsNullOrEmpty(tel_32) ? false : (c.Contact1_Tel1 == tel_32 || c.Contact1_Tel2 == tel_32 || c.Contact2_Tel1 == tel_32 || c.Contact2_Tel2 == tel_32 || c.Contact3_Tel1 == tel_32 || c.Contact3_Tel2 == tel_32)) ||
                (string.IsNullOrEmpty(email_11) ? false : (c.Contact1_Email1 == email_11 || c.Contact1_Tel2 == email_11 || c.Contact2_Email1 == email_11 || c.Contact2_Tel2 == email_11 || c.Contact3_Email1 == email_11 || c.Contact3_Tel2 == email_11)) ||
                (string.IsNullOrEmpty(email_12) ? false : (c.Contact1_Email1 == email_12 || c.Contact1_Tel2 == email_12 || c.Contact2_Email1 == email_12 || c.Contact2_Tel2 == email_12 || c.Contact3_Email1 == email_12 || c.Contact3_Tel2 == email_12)) ||
                (string.IsNullOrEmpty(email_21) ? false : (c.Contact1_Email1 == email_21 || c.Contact1_Tel2 == email_21 || c.Contact2_Email1 == email_21 || c.Contact2_Tel2 == email_21 || c.Contact3_Email1 == email_21 || c.Contact3_Tel2 == email_21)) ||
                (string.IsNullOrEmpty(email_22) ? false : (c.Contact1_Email1 == email_22 || c.Contact1_Tel2 == email_22 || c.Contact2_Email1 == email_22 || c.Contact2_Tel2 == email_22 || c.Contact3_Email1 == email_22 || c.Contact3_Tel2 == email_22)) ||
                (string.IsNullOrEmpty(email_31) ? false : (c.Contact1_Email1 == email_31 || c.Contact1_Tel2 == email_31 || c.Contact2_Email1 == email_31 || c.Contact2_Tel2 == email_31 || c.Contact3_Email1 == email_31 || c.Contact3_Tel2 == email_31)) ||
                (string.IsNullOrEmpty(email_32) ? false : (c.Contact1_Email1 == email_32 || c.Contact1_Tel2 == email_32 || c.Contact2_Email1 == email_32 || c.Contact2_Tel2 == email_32 || c.Contact3_Email1 == email_32 || c.Contact3_Tel2 == email_32)))
                select c).Count();

            if (count > 0)
            {
                return true;
            }

            return false;
        }

        [HttpPost]
        public ActionResult AjaxUpdate(Customers customer, string[] agent_groups)
        {
            int flag = 0;
            string msg = "";
            try
            {
                if (string.IsNullOrEmpty(customer.CName))
                {
                    flag = -3;
                    msg += "Customer chinese name is null\n";
                }
                else
                {
                    if (CheckDuplicate(customer, agent_groups) == true)
                    {
                        flag = -2;
                        //增加顯示客戶名稱 dannis 2015/8/20 
                        msg += string.Format("{0}Update Customer telphone or email is duplication\n", customer.CName);
                    }
                    else
                    {
                        customer.ModifiedDT = DateTime.Now;
                        if (ModelState.IsValid)
                        {
                            flag = -1;
                            db.Entry(customer).State = EntityState.Modified;
                            db.SaveChanges();
                            flag = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg += Helper.FetchExceptionMessage(ex);
            }

            var ret = new {
                result = flag,
                msg = msg
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }

        /// <summary>
        /// 客戶搜尋用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost]
        public ActionResult AjaxSearch(string search_text, string search_type, string[] agent_groups)
        {
            search_text = String.IsNullOrEmpty(search_text) ? "" : search_text;
            search_type = String.IsNullOrEmpty(search_type) ? "Name" : search_type;

            IQueryable<Customers> customers =
                from c in db.Customers
                where
                agent_groups.Contains(c.GroupID)
                &&
                ((search_type != "Name") ? true : (
                    c.CName.Contains(search_text)
                    || c.EName.Contains(search_text)
                    || c.Contact1_Name.Contains(search_text)
                    || c.Contact2_Name.Contains(search_text)
                    || c.Contact3_Name.Contains(search_text)
                ))
                &&
                ((search_type != "ID2") ? true : (c.ID2.Contains(search_text)))
                &&
                ((search_type != "Tel") ? true : (
                    c.Contact1_Tel1.Contains(search_text)
                    || c.Contact1_Tel1.Contains(search_text)
                    || c.Contact1_Tel2.Contains(search_text)
                    || c.Contact2_Tel1.Contains(search_text)
                    || c.Contact2_Tel2.Contains(search_text)
                    || c.Contact3_Tel1.Contains(search_text)
                    || c.Contact3_Tel2.Contains(search_text)
                ))
                &&
                ((search_type != "Email") ? true : (
                    c.Contact1_Email1.Contains(search_text)
                    || c.Contact1_Email2.Contains(search_text)
                    || c.Contact2_Email1.Contains(search_text)
                    || c.Contact2_Email2.Contains(search_text)
                    || c.Contact3_Email1.Contains(search_text)
                    || c.Contact3_Email2.Contains(search_text)
                ))                
                select c;

            // Construct list for data to prevent recursive reference.
            List<Object> ret_customers = new List<object>();
            foreach (Customers customer in customers)
            {
                ret_customers.Add(CustomerToObject(customer));
            }

            return Json(ret_customers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isEnterprise"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost]
        public ActionResult AjaxIncomingSearch(string number, string[] agent_groups)
        {
            List<Object> ret_customers = null;
            IQueryable<Customers> customers;
            dynamic ret_final;
            string ret_final_string;
            try
            {
                if (String.IsNullOrEmpty(number) || number.Trim() == "0")
                {
                    dynamic ret = Helper.GetAjaxRet(0, "Number is 0, null or empty");
                    ret.ret = new String[0];
                    string ret_content = JsonConvert.SerializeObject(ret);
                    return Content(ret_content, Def.JsonMimeType);
                }

                customers =
                    from c in db.Customers
                    where
                    agent_groups.Contains(c.GroupID)
                    && (
                        c.Contact1_Tel1.Contains(number) ||
                        c.Contact1_Tel2.Contains(number) ||
                        c.Contact2_Tel1.Contains(number) ||
                        c.Contact2_Tel2.Contains(number) ||
                        c.Contact3_Tel1.Contains(number) ||
                        c.Contact3_Tel2.Contains(number))
                    select c;
            }
            catch (Exception ex)
            {
                dynamic ret = Helper.GetAjaxRet(-2, Helper.FetchExceptionMessage(ex));
                ret.ret = new String[0];
                string ret_content = JsonConvert.SerializeObject(ret);
                return Content(ret_content, Def.JsonMimeType);
            }
            
            try
            {
                ret_customers = new List<object>();
                foreach (Customers customer in customers)
                {
                    ret_customers.Add(CustomerToObject(customer));
                }
            }
            catch (Exception ex)
            {
                dynamic ret = Helper.GetAjaxRet(-3, Helper.FetchExceptionMessage(ex));
                ret.ret = new String[0];
                string ret_content = JsonConvert.SerializeObject(ret);
                return Content(ret_content, Def.JsonMimeType);
            }

            try
            {
                ret_final = Helper.GetAjaxRet(0, "");
                ret_final.ret = ret_customers;
                ret_final_string = JsonConvert.SerializeObject(ret_final);
            }
            catch (Exception ex)
            {
                dynamic ret = Helper.GetAjaxRet(-4, Helper.FetchExceptionMessage(ex));
                ret.ret = new String[0];
                string ret_content = JsonConvert.SerializeObject(ret);
                return Content(ret_content, Def.JsonMimeType);
            }

            return Content(ret_final_string, Def.JsonMimeType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult JsonInfo(Guid id = default(Guid))
        {
            Customers customer = db.Customers.Find(id);

            return Json(CustomerToObject(customer), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Object CustomerToObject(Customers customer)
        {
            var ret = new
            {
                ID = customer.ID.ToString(),
                CName = customer.CName,
                EName = customer.EName,
                Category = customer.Category,
                ID2 = customer.ID2,
                Region = customer.Region,
                City = customer.City,
                Address = customer.Address,
                GroupID = customer.GroupID,
                Comment = customer.Comment,
                Marketer = customer.Marketer,
                AgentID1 = customer.AgentID1,
                AgentID2 = customer.AgentID2,
                Contact1_Name = customer.Contact1_Name,
                Contact1_Title = customer.Contact1_Title,
                Contact1_Tel1 = customer.Contact1_Tel1,
                Contact1_Tel2 = customer.Contact1_Tel2,
                Contact1_Email1 = customer.Contact1_Email1,
                Contact1_Email2 = customer.Contact1_Email2,
                Contact2_Name = customer.Contact2_Name,
                Contact2_Title = customer.Contact2_Title,
                Contact2_Tel1 = customer.Contact2_Tel1,
                Contact2_Tel2 = customer.Contact2_Tel2,
                Contact2_Email1 = customer.Contact2_Email1,
                Contact2_Email2 = customer.Contact2_Email2,
                Contact3_Name = customer.Contact3_Name,
                Contact3_Title = customer.Contact3_Title,
                Contact3_Tel1 = customer.Contact3_Tel1,
                Contact3_Tel2 = customer.Contact3_Tel2,
                Contact3_Email1 = customer.Contact3_Email1,
                Contact3_Email2 = customer.Contact3_Email2,

                //additional properties
                CategoryName = customer.CustomerCategory == null ? null : customer.CustomerCategory.Desc,
            };
            return ret;
        }

        /*
        public ActionResult JsonSearch()
        {
            NameValueCollection http_params = this.Request.Params;
            string id = http_params["ID"];
            string name = http_params["Name"];
            string tel = http_params["Tel"];

            IQueryable<Customers> customers =
                from c in db.Customers
                where
                    //(!string.IsNullOrEmpty(id) ? c.ID.ToString().Contains(id) : true) &
                (!string.IsNullOrEmpty(name) ? (c.CName.Contains(name)) : true) &
                (!string.IsNullOrEmpty(tel) ? (
                    c.OfficeTel.Contains(tel) ||
                    c.company.ComTel1.Contains(tel) ||
                    c.company.ComTel2.Contains(tel) ||
                    c.company.ComTel3.Contains(tel) ||
                    c.company.ComFax.Contains(tel) ||
                    c.HomeTel.Contains(tel) ||
                    c.OfficeTel.Contains(tel) ||
                    c.Mobile1.Contains(tel) ||
                    c.Mobile2.Contains(tel) ||
                    c.Mobile3.Contains(tel)) : true)
                select c;

            // Construct list for data to prevent recursive reference.
            List<Object> ret_customers = new List<object>();
            foreach (customer customer in customers)
            {
                ret_customers.Add(customer_to_object(customer));
            }

            return Json(ret_customers, JsonRequestBehavior.AllowGet);
        }
         * */
#endregion

        #region Management Section
        /*
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id = default(Guid))
        {
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Customers customers)
        {
            if (ModelState.IsValid)
            {
                customers.ID = Guid.NewGuid();
                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id = default(Guid))
        {
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id = default(Guid))
        {
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         * */
        #endregion


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}