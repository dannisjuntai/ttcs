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
    public class CustomersManagementController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();
        private int pageSize = 10;

        //
        // GET: /EmailSrv/CustomersManagement/

        public ActionResult Index(string type, string condition, string sortOrder, int Desc = 0, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            IQueryable<ECustomers> customers = db.Customers;
            if (!String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(condition))
            {
                switch (type)
                {
                    case "2":
                        customers = customers.Where(c =>
                           c.Contact1_Tel1.Contains(condition) || c.Contact1_Tel2.Contains(condition) ||
                           c.Contact2_Tel1.Contains(condition) || c.Contact2_Tel2.Contains(condition) ||
                           c.Contact3_Tel1.Contains(condition) || c.Contact3_Tel2.Contains(condition));
                        break;
                    case "3":
                    customers = customers.Where(c =>
                            c.Contact1_Email1.Contains(condition) || c.Contact1_Email2.Contains(condition) ||
                            c.Contact2_Email1.Contains(condition) || c.Contact2_Email2.Contains(condition) ||
                            c.Contact3_Email1.Contains(condition) || c.Contact3_Email2.Contains(condition));
                        break;
                    case "1":
                    default:
                        customers = customers.Where(c => c.ID2.ToUpper().Contains(condition.ToUpper()));
                        break;
                }
            }
            customers = customers.OrderBy(c => c.CName);

            //Response.AddHeader("Refresh", "60");

            #region 網頁物件準備
            ViewBag.NumberMax = customers.Count();
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Desc = (Desc == 1) ? 0 : 1;
            #endregion

            return View(customers.ToPagedList(currentPage, pageSize));
        }

        //
        // GET: /EmailSrv/CustomersManagement/Details/5

        public ActionResult Details(Guid id = default(Guid))
        {
            ECustomers ecustomers = db.Customers.Find(id);
            if (ecustomers == null)
            {
                return HttpNotFound();
            }
            return View(ecustomers);
        }

        //
        // GET: /EmailSrv/CustomersManagement/Create

        public ActionResult Create()
        {
            ViewBag.CustomerCategory = new SelectList(db.CustomerCategory, "ID", "Desc",
                (db.CustomerCategory.FirstOrDefault() == null ? null : db.CustomerCategory.FirstOrDefault().ID.ToString()));
            return View();
        }

        //
        // POST: /EmailSrv/CustomersManagement/Create

        [HttpPost]
        public ActionResult Create(ECustomers ecustomers)
        {
            if (ModelState.IsValid)
            {
                ecustomers.ID = Guid.NewGuid();
                db.Customers.Add(ecustomers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ecustomers);
        }

        //
        // GET: /EmailSrv/CustomersManagement/Edit/5

        public ActionResult Edit(Guid id = default(Guid))
        {
            ECustomers ecustomers = db.Customers.Find(id);
            if (ecustomers == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerCategory = new SelectList(db.CustomerCategory, "ID", "Desc",
                (db.CustomerCategory.FirstOrDefault() == null ? null : db.CustomerCategory.FirstOrDefault().ID.ToString()));
            return View(ecustomers);
        }

        //
        // POST: /EmailSrv/CustomersManagement/Edit/5

        [HttpPost]
        public ActionResult Edit(ECustomers ecustomers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ecustomers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ecustomers);
        }

        //
        // GET: /EmailSrv/CustomersManagement/Delete/5

        public ActionResult Delete(Guid id = default(Guid))
        {
            ECustomers ecustomers = db.Customers.Find(id);
            if (ecustomers == null)
            {
                return HttpNotFound();
            }
            return View(ecustomers);
        }

        //
        // POST: /EmailSrv/CustomersManagement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            int flag = 0;
            string msg = "";

            try
            {
                flag = -1;
                ECustomers ecustomers = db.Customers.Find(id);

                flag = -2;
                db.Customers.Remove(ecustomers);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (flag == -1)
                {
                    msg = "指定客戶不存在, 例外說明:" + ex.Message;
                }
                else if (flag == -2)
                {
                    msg = "無法刪除客戶，該客戶已有關聯案件，例外說明:" + ex.Message;
                }

                ViewBag.msg = msg;
                return View("DeleteOk");
            }

            return RedirectToAction("Index");
        }

        private bool SearchAndSort(string fieldname, string condition, string orderby, ref IQueryable<ERecords> records, int Desc = 0)
        {
            #region 查詢
            if (!String.IsNullOrEmpty(fieldname) && !String.IsNullOrEmpty(condition))
            {
                switch (fieldname)
                {
                    // 寄件者
                    case "2":
                        records = records.Where(e => e.EmailRecord.OrderByDescending(r => r.OrderNo).FirstOrDefault().EmailRawHeader.MsgFrom.ToUpper().Contains(condition.ToUpper()));
                        break;

                    // 客服人員
                    case "1":
                    default:
                        records = records.Where(e => e.AgentID != null && e.AgentID.Length > 0 && e.EAgent.AgentName.ToUpper().Contains(condition.ToUpper()));
                        break;
                }
            }
            #endregion

            #region 標題欄的排序
            if (!String.IsNullOrEmpty(orderby))
            {
                switch (orderby)
                {
                    // 寄件者
                    case "MsgFrom":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().EmailRawHeader.MsgFrom) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().EmailRawHeader.MsgFrom);
                        break;

                    // 郵件主旨
                    case "MsgSubject":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().EmailRawHeader.MsgSubject) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().EmailRawHeader.MsgSubject);
                        break;

                    // 進件時間
                    case "IncomeOn":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().IncomeOn) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().IncomeOn);
                        break;

                    // 逾期時間
                    case "ExpireOn":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().ExpireOn) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().ExpireOn);
                        break;

                    // 轉派次數
                    case "AssignCount":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().AssignCount) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().AssignCount);
                        break;

                    // 客服人員
                    case "AgentID":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.AgentID) : records.OrderBy(r => r.AgentID);
                        break;

                    // 案件狀態
                    case "EmailStatus":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.RecordStatus.Name) : records.OrderBy(r => r.RecordStatus.Name);
                        break;

                    // 是否為垃圾郵件
                    case "Garbage":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().Garbage) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().Garbage);
                        break;

                    // 處理狀況
                    case "ProcessStatus":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().ProcessStatus) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().ProcessStatus);
                        break;

                    // 退回原因
                    case "RejectReason":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().RejectReason) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().RejectReason);
                        break;

                    // 退回人員
                    case "CurrAgentId":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().RejectReason).ThenByDescending((r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().CurrAgentId)) : records.OrderBy(r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().RejectReason).ThenBy((r => r.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().CurrAgentId));
                        break;

                    // 案件編號
                    case "RID":
                    default:
                        records = (Desc == 1) ? records.OrderByDescending(r => r.RID) : records.OrderBy(r => r.RID);
                        break;
                }
            }
            else
            {
                records = (Desc == 1) ? records.OrderBy(r => r.RID) : records.OrderByDescending(r => r.RID);
            }
            #endregion

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}