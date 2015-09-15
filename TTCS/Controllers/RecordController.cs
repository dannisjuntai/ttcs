using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using TTCS.Models;
using System.Net.Mail;
using System.IO;
using System.Web.Configuration;
using System.Configuration;
using System.Threading;

using TTCS.App_Start;

namespace TTCS.Controllers
{
    public class RecordController : Controller
    {
        private kskyEntities db = new kskyEntities();

        //
        // GET: /Record/

        public ActionResult Index()
        {
            var records = db.Records.Include(r => r.Customers).Include(r => r.PhoneRecord).Include(r => r.ServiceItem);
            return View(records.ToList());
        }

        //
        // GET: /Record/Details/5

        public ActionResult Details(int id = 0)
        {
            Records records = db.Records.Find(id);
            if (records == null)
            {
                return HttpNotFound();
            }
            return View(records);
        }

        //
        // GET: /Record/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CName");
            ViewBag.RID = new SelectList(db.PhoneRecord, "RID", "PhoneNum");
            ViewBag.ServiceGroupID = new SelectList(db.ServiceItem, "GroupID", "ItemDesc");
            return View();
        }

        //
        // POST: /Record/Create

        [HttpPost]
        public ActionResult Create(Records records)
        {
            if (ModelState.IsValid)
            {
                db.Records.Add(records);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CName", records.CustomerID);
            ViewBag.RID = new SelectList(db.PhoneRecord, "RID", "PhoneNum", records.RID);
            ViewBag.ServiceGroupID = new SelectList(db.ServiceItem, "GroupID", "ItemDesc", records.ServiceGroupID);
            return View(records);
        }

        //
        // GET: /Record/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Records records = db.Records.Find(id);
            if (records == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CName", records.CustomerID);
            ViewBag.RID = new SelectList(db.PhoneRecord, "RID", "PhoneNum", records.RID);
            ViewBag.ServiceGroupID = new SelectList(db.ServiceItem, "GroupID", "ItemDesc", records.ServiceGroupID);
            return View(records);
        }

        //
        // POST: /Record/Edit/5
        [HttpPost]
        public ActionResult Edit(Records records)
        {
            if (ModelState.IsValid)
            {
                db.Entry(records).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CName", records.CustomerID);
            ViewBag.RID = new SelectList(db.PhoneRecord, "RID", "PhoneNum", records.RID);
            ViewBag.ServiceGroupID = new SelectList(db.ServiceItem, "GroupID", "ItemDesc", records.ServiceGroupID);
            return View(records);
        }

        //
        // GET: /Record/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Records records = db.Records.Find(id);
            if (records == null)
            {
                return HttpNotFound();
            }
            return View(records);
        }

        //
        // POST: /Record/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            Records records = db.Records.Find(id);
            db.Records.Remove(records);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 案件查詢用
        /// </summary>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Query()
        {
            string name = String.IsNullOrEmpty(Request.Form["Name"]) ? String.Empty : Request.Form["Name"];
            string tel = String.IsNullOrEmpty(Request.Form["Tel"]) ? String.Empty : Request.Form["Tel"];
            string email = String.IsNullOrEmpty(Request.Form["Email"]) ? String.Empty : Request.Form["Email"];
            string ID2 = String.IsNullOrEmpty(Request.Form["ID2"]) ? String.Empty : Request.Form["ID2"];
            string RID = String.IsNullOrEmpty(Request.Form["RID"]) ? String.Empty : Request.Form["RID"];
            string Subject = String.IsNullOrEmpty(Request.Form["Subject"]) ? String.Empty : Request.Form["Subject"];
            int type = Int32.TryParse(Request.Form["Type"], out type) ? type : 0;
            int status = Int32.TryParse(Request.Form["Status"], out status) ? status : 0 ;
            string start_incoming = String.IsNullOrEmpty(Request.Form["IncomingDTStart"]) ? String.Empty : Request.Form["IncomingDTStart"].ToString();
            string end_incoming = String.IsNullOrEmpty(Request.Form["IncomingDTEnd"]) ? String.Empty : Request.Form["IncomingDTEnd"].ToString();
            int Approach = Int32.TryParse(Request.Form["Approach"], out Approach) ? Approach : 0;
            string Agent = String.IsNullOrEmpty(Request.Form["Agent"]) ? String.Empty : Request.Form["Agent"];

            DateTime start_dt = (DateTime.TryParse(start_incoming, out start_dt) == true) ? start_dt : SqlDateTime.MinValue.Value;
            DateTime end_dt = (DateTime.TryParse(end_incoming, out end_dt) == true) ? end_dt : SqlDateTime.MaxValue.Value;

            //int i;

            IQueryable<Records> records =
                from r in db.Records
                orderby r.RID descending
                select r ;

            //i = records.Count();
            if (!String.IsNullOrEmpty(name))
            {
                records =
                    from r in records
                    where 
                        r.Customers.CName.Contains(name)
                        || r.Customers.EName.Contains(name)
                        || r.Customers.Contact1_Name.Contains(name)
                        || r.Customers.Contact2_Name.Contains(name)
                        || r.Customers.Contact3_Name.Contains(name)
                    select r;
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(ID2))
            {
                records = 
                    from r in records
                    where
                        r.Customers.ID2.Contains(ID2)
                    select r;
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(tel))
            {
                records = 
                    from r in records
                    where
                        r.Customers.Contact1_Tel1.Contains(tel)
                        || r.Customers.Contact1_Tel1.Contains(tel)
                        || r.Customers.Contact1_Tel2.Contains(tel)
                        || r.Customers.Contact2_Tel1.Contains(tel)
                        || r.Customers.Contact2_Tel2.Contains(tel)
                        || r.Customers.Contact3_Tel1.Contains(tel)
                        || r.Customers.Contact3_Tel2.Contains(tel)
                        || ((r.RecordType.ID == 1 || r.RecordType.ID == 2) && (r.PhoneRecord.PhoneNum.Contains(tel)))
                    select r;        
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(email))
            {
                records = 
                    from r in records
                    where
                        r.Customers.Contact1_Email1.Contains(email)
                        || r.Customers.Contact1_Email2.Contains(email)
                        || r.Customers.Contact2_Email1.Contains(email)
                        || r.Customers.Contact2_Email2.Contains(email)
                        || r.Customers.Contact3_Email1.Contains(email)
                        || r.Customers.Contact3_Email2.Contains(email)
                        || ((r.RecordType.ID == 3) && (r.EmailRecord.FirstOrDefault().EmailRawHeader.MsgFrom.Contains(email)))
                    select r;
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(RID))
            {
                records = 
                    from r in records
                    where
                        r.RID.Contains(RID)
                    select r;
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(Subject))
            {
                records = 
                    from r in records
                    where
                        r.RecordType.ID == 3
                        && r.EmailRecord.Where(e => e.RID == r.RID).FirstOrDefault().EmailRawHeader.MsgSubject.Contains(Subject)
                    select r;
            }
            //i = records.Count();
            if (type != 0)
            {
                records = 
                    from r in records
                    where
                        r.TypeID == type
                    select r;
            }
            //i = records.Count();
            if (status != 0)
            {
                records =
                    from r in records
                    where
                        r.StatusID == status
                    select r;
            }
            //i = records.Count();
            if (start_dt != DateTime.MinValue)
            {
                records = 
                    from r in records
                    where
                        r.IncomingDT >= start_dt
                    select r;
            }
            //i = records.Count();
            if (end_dt != DateTime.MaxValue)
            {
                records =
                    from r in records
                    where
                        r.IncomingDT <= end_dt
                    select r;
            }
            //i = records.Count();
            if (Approach != 0)
            {
                records =
                    from r in records
                    where
                        r.CloseApproachID == Approach
                    select r;
            }
            //i = records.Count();
            if (!String.IsNullOrEmpty(Agent))
            {
                records =
                    from r in records
                    where
                        r.Agent.AgentName.Contains(Agent)
                    select r;
            }

            List<Object> record_list = new List<object>();
            foreach (Records record in records)
            {
                record_list.Add(RecordToObject(record));
            }

            return Content(JsonConvert.SerializeObject(record_list), Def.JsonMimeType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Show()
        {
            ViewBag.CustomerCategory = new SelectList(db.CustomerCategory, "ID", "Desc", 
                (db.CustomerCategory.FirstOrDefault() == null ? null : db.CustomerCategory.FirstOrDefault().ID.ToString()));
            ViewBag.MailContacts = new SelectList(db.EmailContacts, "Id", "ContactEmail",
                (db.EmailContacts.FirstOrDefault() == null? null: db.EmailContacts.FirstOrDefault().Id.ToString()));
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult JsonInfo(string id = "")
        {
            Records record = db.Records.Find(id);

            // 郵件案件
            if (record.TypeID == 3)
            {
                // 狀態由 "未處理" 變為 "處理中"
                string strKey = id;
                if (id.Contains("_B"))
                    strKey = id.Substring(0, id.IndexOf("_B"));

                foreach (var recordModified in from rs in db.Records where rs.RID.Contains(strKey) select rs)
                {                    
                    if (recordModified.StatusID == 1)
                    {
                        recordModified.StatusID = 2;
                        db.Entry(recordModified).State = EntityState.Modified;
                    }                    
                }
                foreach (var erecord in db.EmailRecord.Where(e => e.RID == id))
                {
                    if (erecord.UnRead != null && erecord.UnRead.Value > 0)
                    {
                        erecord.UnRead = 0;
                        db.Entry(erecord).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
            var objRet = JsonConvert.SerializeObject(RecordToObject(record));

            //return Content(objRet);
            return Content(objRet, Def.JsonMimeType);
        }

        /// <summary>
        /// 案件列表用
        /// </summary>
        /// <param name="agent_id"></param> 
        /// <param name="status_id"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AjaxJsonList(string agent_id)
        {
            // Amount of per page
            int pp = 20;
            int page = String.IsNullOrEmpty(Request.Form["page"])? 0: int.Parse(Request.Form["page"].ToString()) - 1;
            int status_id = String.IsNullOrEmpty(Request.Form["status"]) ? 1 : int.Parse(Request.Form["status"].ToString());
            int total_count = 0;

            IQueryable<Records> records =
                db.Records.Where(r => r.StatusID == status_id && r.AgentID == agent_id);

            total_count = records.Count();
            IQueryable<Records> records_page =
                    records.OrderBy(r => r.RID).Skip(page * pp).Take(pp);

            List<Object> record_list = new List<object>();
            foreach (Records record in records_page)
            {
                record_list.Add(RecordToObject(record));
            }

            var result = new {
                total_page = (total_count > 0) ? Math.Ceiling((float)total_count / (float)pp) : 0,
                list = record_list
            };

            //Dynamic要用Json.NET才會正確的序列化
            return Content(JsonConvert.SerializeObject(result), Def.JsonMimeType);
        }


        /// <summary>
        /// 案件瀏覽下方相關案件列表
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="limited"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AjaxInfoList(Guid customer_id, int limited)
        {
            IQueryable<Records> records =
                db.Records.Where(r => r.CustomerID == customer_id)
                .Take(limited).OrderByDescending(r => r.IncomingDT);

            List<Object> record_list = new List<object>();
            foreach (Records record in records)
            {
                record_list.Add(RecordToObject(record));
            }

            //Dynamic要用Json.NET才會正確的序列化
            return Content(JsonConvert.SerializeObject(record_list), Def.JsonMimeType);
        }

        /// <summary>
        /// Record轉出給Browser之前的去循環參考
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private dynamic RecordToObject(Records record)
        {
            //這邊會試情況不同產生符合telphoneRecord或EmailRecord的record
            //不太好的做法, 耦合性太高了
            dynamic ret_record = new ExpandoObject();
            //if(record.CustomerID != null)
            //    record.Customers = db.Customers.Where(c => c.ID == record.CustomerID.Value).FirstOrDefault();
            ret_record.RID = record.RID;
            ret_record.CustomerID = (record.Customers != null) ? record.Customers.ID.ToString() :  null;
            ret_record.TypeID = record.TypeID;
            ret_record.AgentID = record.AgentID;
            ret_record.StatusID = record.StatusID;
            //ret_record.Comment = HttpUtility.HtmlEncode(record.Comment).Replace("\n", "<br />");
            ret_record.Comment = record.Comment;
            ret_record.ServiceGroupID = record.ServiceGroupID;
            ret_record.ServiceItemID = record.ServiceItemID;
            ret_record.IncomingDT = record.IncomingDT;
            ret_record.FinishDT = record.FinishDT;
            ret_record.GroupID = record.GroupID;
            ret_record.CloseApproachID = record.CloseApproachID;
            List<dynamic> closeApproachList = new List<dynamic>();
            foreach (var approach in db.RecordCloseApproach)
            {
                closeApproachList.Add(new
                {
                    ApproachID = approach.ApproachID,
                    ApproachName = approach.ApproachName
                });
            }
            ret_record.CloseApproachList = closeApproachList;

            if (record.TypeID == 1 || record.TypeID == 2)
            {
                if (record.PhoneRecord != null)
                {
                    ret_record.PhoneNum = record.PhoneRecord.PhoneNum;
                    ret_record.QueueName = record.PhoneRecord.QueueName;
                    ret_record.IndexID = record.PhoneRecord.IndexID;
                }
            }
            #region 郵件案件資訊
            else if (record.TypeID == 3)
            {
                ret_record.EmailRecordErrMsg = "";      // 錯誤訊息
                ret_record.MsgUnRead = "0";             // 郵件是否被讀取過, 檢視是否有新回信
                ret_record.MsgSubjectModifiedBy = "";   // 郵件主旨是否被修改過, 僅能被更改一次
                ret_record.EOrder = "0";                // 郵件順序, 配合郵件主旨僅能在新案件時修改, 若已有回信, 則無法修改
                ret_record.MsgFrom = "";                // 郵件來源
                ret_record.MsgSubject = "";             // 郵件主旨
                ret_record.ExpireOn = "";               // 逾期時間
                ret_record.AssignCount = "0";           // 轉派次數
                ret_record.MsgReceivedOn = "";          // 郵件寄送時間
                ret_record.CurrAgent = "";              // 目前客服人員ID
                ret_record.EmailRecordID = "";          // 欲顯示之郵件ID, 要顯示最新一筆郵件
                ret_record.MsgBody = "";                // 郵件內容
                ret_record.MsgAttachments = "";         // 郵件附件
                ret_record.ProcessStatus = "";          // 郵件處理狀況
                ret_record.SendTo = "";                 // 郵件寄送目的地
                ret_record.IsDraft = "";                // 是否已有草稿
                ret_record.Cc = "";                     // 郵件副本
                ret_record.Bcc = "";                    // 郵件密件副本
                ret_record.MailSig = "1";               // 是否夾帶簽名檔                

                EmailRecord emailrecordfirst = record.EmailRecord.OrderBy(r => r.OrderNo).FirstOrDefault();
                if (emailrecordfirst == null)
                {
                    // 不正常
                    ret_record.EmailRecordErrMsg = String.Format("EmailRecord Not Found[RID={0}]", record.RID);
                }
                else
                {
                    int nCount = record.EmailRecord.Count();
                    #region 新增郵件案件
                    if (emailrecordfirst.RawHeaderId == null || emailrecordfirst.RawHeaderId.Value < 1)
                    {
                        // 還未寄出
                        if (nCount == 1)
                        {
                            EmailSavedBody esavebody2 = db.EmailSavedBody.Find(emailrecordfirst.SaveCntIn);
                            if (esavebody2 == null)
                            {
                                ret_record.EmailRecordErrMsg = String.Format("EmailSavedBody2 Not Found[EID={0}]", emailrecordfirst.Id);
                            }
                            else
                            {
                                bool bOK = false;
                                List<SelectListItem> attachments = new List<SelectListItem>();
                                string strBody = "", strErr = "", strAttachments = "";
                                bOK = BuildUpEmailBodyAndAttachmentsFromDraft(emailrecordfirst, ref strBody, ref attachments, ref strAttachments, ref strErr);
                                if (!bOK)
                                {
                                    // 不正常
                                    ret_record.EmailRecordErrMsg =
                                        String.Format("EmailBody Error[EID={0}, INFO={1}]",
                                                            emailrecordfirst.Id, strErr);
                                }
                                else
                                {
                                    if (esavebody2.FileName != null)
                                        ret_record.MsgSubject = esavebody2.FileName;

                                    if (emailrecordfirst.ExpireOn != null)
                                        ret_record.ExpireOn = emailrecordfirst.ExpireOn.Value;

                                    if (emailrecordfirst.AssignCount != null)
                                        ret_record.AssignCount = emailrecordfirst.AssignCount.Value.ToString();

                                    if (emailrecordfirst.CurrAgentId != null)
                                        ret_record.CurrAgent = emailrecordfirst.CurrAgentId;

                                    ret_record.EmailRecordID = emailrecordfirst.Id;
                                    ret_record.MsgBody = strBody.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\\n", "<br />");                 // 郵件內容
                                    ret_record.MsgAttachments = strAttachments;         // 郵件附件
                                    ret_record.ProcessStatus = emailrecordfirst.ProcessStatus;
                                    ret_record.MsgFrom = esavebody2.SendTo;
                                    ret_record.IsDraft = "1";
                                    ret_record.Cc = esavebody2.Cc;
                                    ret_record.Bcc = esavebody2.Bcc;
                                    if (esavebody2.MailSignature != null)
                                        ret_record.MailSig = esavebody2.MailSignature.Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            emailrecordfirst = record.EmailRecord.Where(e => e.OrderNo != 0).OrderBy(r => r.OrderNo).FirstOrDefault();
                        }
                    }
                    #endregion

                    if (emailrecordfirst == null)
                    {
                        // 不正常
                        ret_record.EmailRecordErrMsg = String.Format("EmailRecord2 Not Found[RID={0}]", record.RID);
                    }
                    else if(emailrecordfirst.RawHeaderId != null && emailrecordfirst.RawHeaderId.Value > 0)
                    {
                        EmailRawHeader originalrawheader = emailrecordfirst.EmailRawHeader;
                        if (originalrawheader == null)
                        {
                            // 不正常
                            ret_record.EmailRecordErrMsg = String.Format("EmailRawHeader Not Found[RawID={0}]", emailrecordfirst.RawHeaderId);
                        }
                        else
                        {
                            string originalMsgFrom = originalrawheader.MsgFrom;
                            EmailRecord emailrecordlast = record.EmailRecord.OrderByDescending(r => r.OrderNo)
                                                            .Where(r => r.EmailRawHeader.MsgFrom.Contains(originalMsgFrom)).FirstOrDefault();
                            if (emailrecordlast == null)
                            {
                                // 不正常
                                ret_record.EmailRecordErrMsg =
                                    String.Format("EmailRawHeader Not Found[RawID={0}, MsgFrom={1}]",
                                                        emailrecordfirst.RawHeaderId, originalMsgFrom);
                            }
                            else
                            {
                                bool bOK = false;
                                List<SelectListItem> attachments = new List<SelectListItem>();
                                string strBody = "", strErr = "", strAttachments = "";
                                if (emailrecordlast.SaveCntIn != null && emailrecordlast.SaveCntIn.Length > 0)
                                {
                                    bOK = BuildUpEmailBodyAndAttachmentsFromDraft(emailrecordlast, ref strBody, ref attachments, ref strAttachments, ref strErr);
                                    ret_record.IsDraft = "1";
                                    if (bOK)
                                    {
                                        EmailSavedBody esavebody = emailrecordlast.EmailSavedBody;
                                        if (esavebody != null)
                                        {
                                            if (esavebody.SendTo != null)
                                                ret_record.SendTo = esavebody.SendTo;

                                            if (esavebody.Cc != null)
                                                ret_record.Cc = esavebody.Cc;

                                            if (esavebody.Bcc != null)
                                                ret_record.Bcc = esavebody.Bcc;

                                            if (esavebody.MailSignature != null)
                                                ret_record.MailSig = esavebody.MailSignature.Value.ToString();

                                            if (emailrecordfirst.MsgSubjectModifiedBy != null)
                                                ret_record.MsgSubject = esavebody.FileName;
                                        }
                                    }
                                }
                                else
                                {
                                    bOK = BuildUpEmailBodyAndAttachmentsFromRawHeader(emailrecordlast, ref strBody, ref attachments, ref strAttachments, ref strErr);
                                    ret_record.IsDraft = "0";
                                }

                                if (!bOK)
                                {
                                    // 不正常
                                    ret_record.EmailRecordErrMsg =
                                        String.Format("EmailBody Error[EID={0}, INFO={1}]",
                                                            emailrecordlast.Id, strErr);
                                }
                                else
                                {
                                    EmailRawHeader erawheaderlast = emailrecordlast.EmailRawHeader;
                                    if (erawheaderlast == null)
                                    {
                                        // 不正常
                                        ret_record.EmailRecordErrMsg =
                                            String.Format("EmailRawHeader Not Found[EID={0}, RawID={1}]",
                                                                emailrecordlast.Id, emailrecordlast.RawHeaderId);
                                    }
                                    else
                                    {
                                        if (emailrecordlast.UnRead != null)
                                            ret_record.MsgUnRead = emailrecordlast.UnRead.Value.ToString();

                                        if (emailrecordfirst.MsgSubjectModifiedBy != null)
                                        {
                                            var ag = db.Agent.Find(emailrecordlast.MsgSubjectModifiedBy);
                                            if (ag != null)
                                                ret_record.MsgSubjectModifiedBy = ag.AgentName;
                                        }

                                        if(((string)ret_record.MsgSubject).Length < 1)
                                        {
                                            ret_record.MsgSubject = erawheaderlast.MsgSubject;
                                        }
                                        ret_record.MsgFrom = erawheaderlast.MsgFrom;
                                        

                                        if (emailrecordlast.ExpireOn != null)
                                            ret_record.ExpireOn = emailrecordlast.ExpireOn.Value;

                                        if (emailrecordlast.AssignCount != null)
                                            ret_record.AssignCount = emailrecordlast.AssignCount.Value.ToString();

                                        ret_record.MsgReceivedOn = erawheaderlast.MsgSentOn;

                                        if (emailrecordlast.CurrAgentId != null)
                                            ret_record.CurrAgent = emailrecordlast.CurrAgentId;

                                        ret_record.EmailRecordID = emailrecordlast.Id.ToString();

                                        ret_record.MsgBody = strBody;//.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\\n", "<br />"); ;

                                        if (strAttachments.Length > 0)
                                            ret_record.MsgAttachments = strAttachments.Substring(1);

                                        if (emailrecordlast.ProcessStatus != null)
                                            ret_record.ProcessStatus = emailrecordlast.ProcessStatus;

                                        if (emailrecordlast.EmailRawHeader != null && emailrecordlast.EmailRawHeader.MsgCc != null)
                                            ret_record.Cc = emailrecordlast.EmailRawHeader.MsgCc;

                                        ret_record.EOrder = emailrecordlast.OrderNo.ToString();

                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            else
            {
                throw new Exception("TypeID of record is malformed.");
            }

            /* Additional Data */
            ret_record.CustomerName = (record.Customers != null)?record.Customers.CName:null;
            ret_record.AgentName = (record.Agent != null) ? record.Agent.AgentName : null;
            ret_record.TypeName = (record.RecordType != null)?record.RecordType.Name:null;
            ret_record.StatusName = (record.RecordStatus != null)?record.RecordStatus.Name:null;

            IQueryable<RecordServiceMap> maps = db.RecordServiceMap.Where(m => m.RID == record.RID);
            List<dynamic> serviceItemList = new List<dynamic>();
            List<string> serviceName = new List<string>();
            foreach (RecordServiceMap map in maps)
            {
                serviceItemList.Add(new {
                    GroupID = map.GroupID,
                    ItemID = map.ItemID
                });
                IQueryable<ServiceItem> item = db.ServiceItem.Where(s => s.GroupID == map.GroupID && s.ItemID == map.ItemID);
                serviceName.Add(item.First().ItemDesc);
            }
            ret_record.ServiceItemList = serviceItemList;
            ret_record.ServiceItemName = String.Join("，", serviceName);

            return ret_record;
        }

        public ActionResult Download(int id = 0)
        {
            EmailRawBody emailrawbody = db.EmailRawBody.Find(id);
            if (emailrawbody == null)
            {
                return HttpNotFound();
            }

            return File(emailrawbody.BinaryCnt, emailrawbody.CntMedia, emailrawbody.FileName);
        }

        // 重組郵件內容及其附件(原始郵件)
        private bool BuildUpEmailBodyAndAttachmentsFromRawHeader(EmailRecord erecord, ref string strBody, ref List<SelectListItem> attachments, ref string attachlist, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                EmailRawHeader erawheader = erecord.EmailRawHeader;
                if (erawheader == null)
                {
                    // 不正常
                    errMsg = String.Format("RawHeader Not Found [ERID={0}]", erecord.Id);
                    return false;
                }                

                if (erawheader.EmailRawBody.Count() < 1)
                {
                    // 不正常
                    errMsg = String.Format("RawBody Not Found [RawID={0}]", erawheader.Id);
                    return false;
                }

                string curpath = Server.MapPath("~");
                if (curpath[curpath.Length - 1] == '\\')
                    curpath = curpath.Substring(0, curpath.Length - 1);

                string mailbox = String.Format("{0}\\inbox\\{1}", curpath, erecord.RID);
                
                List<SelectListItem> cids = new List<SelectListItem>();
                attachments.Clear();
                attachlist = "";
                string strCnt = "", strCntTmp = "";

                foreach (var body in erawheader.EmailRawBody.OrderBy(rb => rb.PlaceHolder))
                {
                    // 郵件內容區塊
                    if (body.FileName.Length < 1)
                    {
                        // 內容區塊為純文字編碼
                        if (body.CntMedia.IndexOf("plain") >= 0)
                        {
                            // 取不到HTML編碼方式的內容區塊時, 則取出純文字內容
                            if (strCntTmp.Length > 0)
                                strCnt += strCntTmp;

                            // 取出內容 (字串形式)
                            strCntTmp = System.Text.Encoding.GetEncoding(body.CodePage).GetString(body.BinaryCnt);
                            strCntTmp = strCntTmp.Replace("\r\n", "<br />").Replace("\n", "<br />");
                            continue;
                        }

                        // 取出內容 (HTML形式)
                        strCnt += System.Text.Encoding.GetEncoding(body.CodePage).GetString(body.BinaryCnt);
                        strCntTmp = "";
                    }

                    // 郵件檔案區塊
                    else
                    {
                        // 郵件附檔 
                        if (!body.FileName.Contains('|'))
                        {
                            attachlist += "|" + body.Id.ToString() + ":" + body.FileName;
                            attachments.Add(new SelectListItem() { Text = body.FileName, Value = body.Id.ToString() });
                        }
                        // 郵件中內嵌資訊(圖片或檔案)
                        else
                        {
                            string[] cid_filename = body.FileName.Split('|');

                            // 內嵌資訊不是圖片
                            if (!body.CntMedia.Contains("image"))
                            {
                                attachlist += "|" + body.Id.ToString() + ":" + cid_filename[1];
                                attachments.Add(new SelectListItem() { Text = cid_filename[1], Value = body.Id.ToString() });
                                continue;
                            }

                            // 先預備存放郵件圖片的資料夾
                            if (!Directory.Exists(mailbox))
                            {
                                Directory.CreateDirectory(mailbox);
                            }

                            // 原始內嵌圖片沒有檔案名稱, 系統取亂數作為檔案名稱
                            if (cid_filename[1].Contains("(no name)"))
                            {
                                Random RGen = new Random();
                                string fname = RGen.Next(100000, 9999999).ToString();
                                cid_filename[1] = fname + "." + body.CntMedia.Substring(body.CntMedia.IndexOf("/") + 1);
                            }

                            if (!cid_filename[1].Contains("."))
                            {
                                cid_filename[1] += "." + body.CntMedia.Substring(body.CntMedia.IndexOf("/") + 1);
                            }

                            // 圖檔名稱包含路徑
                            if (cid_filename[1].IndexOf('\\') != -1)
                            {
                                cid_filename[1] = cid_filename[1].Substring(cid_filename[1].LastIndexOf('\\') + 1);
                            }

                            // 圖檔名稱包含非法字元
                            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                            {
                                cid_filename[1] = cid_filename[1].Replace(c, '_');
                            }

                            // 暫存圖片檔案名稱格式: RawHeaderID_CID_FileName
                            string attname = String.Format("{0}_{1}_{2}", body.RawHeaderId, cid_filename[0].Replace(':', '-'), cid_filename[1]);

                            if (!System.IO.File.Exists(String.Format("{0}\\{1}", mailbox, attname)))
                            {
                                BinaryWriter writer = new BinaryWriter(new FileStream(String.Format("{0}\\{1}", mailbox, attname), FileMode.CreateNew, FileAccess.Write, FileShare.None));
                                writer.Write(body.BinaryCnt);
                                writer.Close();
                            }
                            cids.Add(new SelectListItem() { Text = cid_filename[0], Value = Url.Content(String.Format("~/inbox/{0}/{1}", erecord.RID, attname.Replace("\\", "/"))) });
                        }
                    }
                }
                if (strCntTmp.Length > 0)
                    strCnt += strCntTmp;

                strBody = strCnt;

                // 將郵件內容中的圖片, 取代成系統暫存位置
                foreach (SelectListItem cidatt in cids)
                {
                    strBody = strBody.Replace("cid:" + cidatt.Text, cidatt.Value);
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                attachments.Clear();
                bRet = false;
            }
            return bRet;
        }

        // 重組郵件內容及其附件(草稿)
        private bool BuildUpEmailBodyAndAttachmentsFromDraft(EmailRecord erecord, ref string strBody, ref List<SelectListItem> attachments, ref string attachlist, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                EmailSavedBody esavedbody = erecord.EmailSavedBody;
                if (esavedbody == null)
                {
                    // 不正常
                    errMsg = String.Format("SavedBody Not Found [ERID={0}]", erecord.Id);
                    return false;
                }

                List<SelectListItem> cids = new List<SelectListItem>();
                attachments.Clear();
                attachlist = "";

                if (esavedbody.BodyCnt != null)
                    strBody = System.Text.Encoding.GetEncoding(esavedbody.CodePage.Value).GetString(esavedbody.BodyCnt);

                var esavedattach = db.EmailSavedAttachment.Where(a => a.EmailRecordId == erecord.Id);
                foreach (var attach in esavedattach)
                {
                    attachlist += "|" + attach.Id.ToString() + ":" + attach.FileName;
                    attachments.Add(new SelectListItem() { Text = attach.FileName, Value = attach.Id.ToString() });
                }
            }
            catch (Exception ex)
            {
                errMsg = Helper.FetchExceptionMessage(ex);
                attachments.Clear();
                bRet = false;
            }
            return bRet;
        }

        // 重組郵件內容及其附件(寄件備份)
        private bool BuildUpEmailBodyAndAttachmentsFromSentBox(EmailSentBox esentbox, ref string strBody, ref List<SelectListItem> attachments, ref string attachlist, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                EmailRecord erecord = db.EmailRecord.Where(er => er.RID == esentbox.RID && er.OrderNo == esentbox.OrderNo).FirstOrDefault();
                if(erecord == null)
                {
                    errMsg = String.Format("ERecord Not Found[RID={0}, Order={1}]", esentbox.RID, esentbox.OrderNo);
                    return false;
                }

                List<SelectListItem> cids = new List<SelectListItem>();
                attachments.Clear();
                attachlist = "";

                strBody = System.Text.Encoding.GetEncoding(esentbox.CodePage.Value).GetString(esentbox.BodyCnt);
                
                var esentattach = db.EmailSentAttachment.Where(a => a.EmailRecordId == erecord.Id);
                foreach (var attach in esentattach)
                {
                    attachlist += "|" + attach.Id.ToString() + ":" + attach.FileName;
                    attachments.Add(new SelectListItem() { Text = attach.FileName, Value = attach.Id.ToString() });
                }
            }
            catch (Exception ex)
            {
                errMsg = Helper.FetchExceptionMessage(ex);
                attachments.Clear();
                bRet = false;
            }
            return bRet;
        }

        #region 讀取郵件信箱資訊
        public static string MailSender_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailSender_Enterprise"];
        public static string MailSeanderName_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailSeanderName_Enterprise"];
        public static string MailPassword_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailPassword_Enterprise"];
        public static string MailSmtp_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailSmtp_Enterprise"];
        public static string MailPort_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailPort_Enterprise"];
        public static string MailSSL_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailSSL_Enterprise"];

        public static string MailSender_Personal = System.Configuration.ConfigurationManager.AppSettings["MailSender_Personal"];
        public static string MailSeanderName_Personal = System.Configuration.ConfigurationManager.AppSettings["MailSeanderName_Personal"];
        public static string MailPassword_Personal = System.Configuration.ConfigurationManager.AppSettings["MailPassword_Personal"];
        public static string MailSmtp_Personal = System.Configuration.ConfigurationManager.AppSettings["MailSmtp_Personal"];
        public static string MailPort_Personal = System.Configuration.ConfigurationManager.AppSettings["MailPort_Personal"];
        public static string MailSSL_Personal = System.Configuration.ConfigurationManager.AppSettings["MailSSL_Personal"];
        #endregion

        private string AddMsgHeaderDefault(ref MailMessage mail, EmailRecord erecord, string SendTo, string Cc, string Bcc, ref string Subject, ref string MailTo, ref int nForwardCount)
        {
            if (erecord.RID == null || erecord.RID.Length < 1)
                return String.Format("RID Not Found[EID={0}]", erecord.Id);

            #region 主旨及收件者, 若回信或轉寄, 則添加額外資訊
            // 郵件案件: 新增(第一次寄送)
            if (erecord.RawHeaderId == null || erecord.RawHeaderId.Value < 1)
            {
                EmailSavedBody esavebody = db.EmailSavedBody.Find(erecord.SaveCntIn);
                if (esavebody == null)
                    return String.Format("EmailSavedBody Not Found[SaveCntIn={0}]", erecord.SaveCntIn);

                //Subject = esavebody.FileName;  // 主旨                
                //MailTo = esavebody.SendTo;     // 收件者            
                // 主旨中沒有案件編號
                if (!Subject.Contains(erecord.RID))
                {
                    Subject += "[" + erecord.RID + "]";
                }
                //Subject += "[" + erecord.RID + "]";
                MailTo = SendTo;
            }
            // 郵件案件: 回覆或轉寄
            else
            {
                EmailRawHeader erawheader = db.EmailRawHeader.Find(erecord.RawHeaderId);
                if (erawheader == null)
                    return String.Format("EmailRawHeader Not Found[RawHeaderId={0}]", erecord.RawHeaderId);


                //Subject = erawheader.MsgSubject;


                // 回覆
                if (SendTo == null || SendTo.Length < 1)
                {
                    // 主旨中沒有案件編號
                    if (!Subject.Contains(erecord.RID))
                    {
                        Subject += "[" + erecord.RID + "]";
                    }
                    Subject = "Re：" + Subject;

                    // 收件者
                    MailTo = erawheader.MsgFrom;
                    int nStartPos = MailTo.IndexOf('<');
                    int nEndPos = MailTo.LastIndexOf('>');

                    if (nStartPos < 0)
                        nStartPos = -1;

                    if (nEndPos < 0)
                        nEndPos = MailTo.Length;

                    MailTo = MailTo.Substring(nStartPos + 1, nEndPos - nStartPos - 1);
                }
                // 轉寄
                else
                {
                    string strRIDKey = erecord.RID;
                    if (strRIDKey.Contains("_B"))
                        strRIDKey = strRIDKey.Substring(0, strRIDKey.IndexOf("_B"));

                    // 主旨中沒有案件編號
                    if (!Subject.Contains(strRIDKey))
                    {
                        nForwardCount++;
                        Subject += String.Format("[{0}_B{1:00}]", strRIDKey, nForwardCount);
                    }
                    else
                    {
                        nForwardCount++;
                        Subject = Subject.Replace(erecord.RID, String.Format("{0}_B{1:00}", strRIDKey, nForwardCount));
                    }
                    Subject = "Fw：" + Subject;

                    // 收件者
                    MailTo = SendTo;
                }

                // 額外資訊
                // this is for gmail
                mail.Headers.Add("In-Reply-To", erawheader.MsgId);
            }
            #endregion

            mail.To.Add(MailTo);
            mail.Subject = Subject;

            if (Cc != null && Cc.Length > 0)
            {
                var arrCc = Cc.Split(';');
                foreach (var cc in arrCc)
                {
                    int nStartPos = cc.IndexOf('<');
                    int nEndPos = cc.IndexOf('>');
                    
                    if(nStartPos < 0)
                        nStartPos = 0;
                    else
                        nStartPos = nStartPos + 1;

                    if(nEndPos < 0)
                        nEndPos = cc.Length;
                    
                    mail.CC.Add(cc.Substring(nStartPos, nEndPos-nStartPos));
                }
            }
            if (Bcc != null && Bcc.Length > 0)
            {
                var arrBcc = Bcc.Split(';');
                foreach (var bcc in arrBcc)
                {
                    int nStartPos = bcc.IndexOf('<');
                    int nEndPos = bcc.IndexOf('>');
                    if (nStartPos < 0)
                        nStartPos = 0;
                    else
                        nStartPos = nStartPos + 1;

                    if (nEndPos < 0)
                        nEndPos = bcc.Length;

                    mail.Bcc.Add(bcc.Substring(nStartPos, nEndPos - nStartPos));
                }
            }

            return "";
        }

        private string CreateMailBody(ref MailMessage message, string body)
        {
            try
            {
                int nStartPos = 0, nEndPos = 0;

                string img = "<img", src = "src=\"";

                string strContent = body;

                List<LinkedResource> resourceList = new List<LinkedResource>();

                while (nStartPos != -1)
                {
                    nStartPos = body.IndexOf(img, nEndPos);

                    if (nStartPos < 0)
                        continue;

                    nEndPos = body.IndexOf(">", nStartPos);

                    // Html tags are Not match
                    if (nEndPos < 0)
                        continue;

                    nStartPos = body.IndexOf(src, nStartPos);
                    nEndPos = body.IndexOf("\"", nStartPos + src.Length);

                    // Cannot Found "src" attribute
                    if (nStartPos > nEndPos || nEndPos < 0 || nStartPos < 0)
                        continue;

                    string att = body.Substring(nStartPos + src.Length, nEndPos - (nStartPos + src.Length));

                    if (att.StartsWith("data:"))
                    {
                        int cntTypeStartPos = "data:".Length;
                        int cntTypeEndPos = att.IndexOf(";");

                        if (cntTypeStartPos > cntTypeEndPos || cntTypeEndPos < 0)
                            continue;

                        string cntType = att.Substring(cntTypeStartPos, cntTypeEndPos - cntTypeStartPos);

                        int nDataPos = att.IndexOf("base64,");

                        if (nDataPos < 0)
                            continue;

                        nDataPos += "base64,".Length;

                        byte[] newfile = Convert.FromBase64String(att.Substring(nDataPos));
                        Random RGen = new Random();
                        string fname = RGen.Next(100000, 9999999).ToString();
                        string savepath = Server.MapPath("~");
                        if (!savepath.EndsWith("\\"))
                            savepath += "\\";
                        savepath += "inbox\\temp\\" + fname + "." + cntType.Substring(cntType.IndexOf("/") + 1);

                        if (System.IO.File.Exists(savepath))
                            System.IO.File.Delete(savepath);

                        if(!Directory.Exists(savepath.Substring(0, savepath.LastIndexOf("\\"))))
                            Directory.CreateDirectory(savepath.Substring(0, savepath.LastIndexOf("\\")));

                        System.IO.File.WriteAllBytes(savepath, newfile);

                        LinkedResource objLinkedRes = new LinkedResource(savepath);
                        objLinkedRes.ContentId = fname;
                        objLinkedRes.ContentType = new System.Net.Mime.ContentType(cntType);

                        resourceList.Add(objLinkedRes);

                        strContent = strContent.Replace(att, "cid:" + fname);
                    }
                    else
                    {
                        if (!att.StartsWith("http") && !att.ToLower().StartsWith("wlmailhtml:") && !att.StartsWith("cid:"))
                        {
                            string path = Server.MapPath(att);
                            if (System.IO.File.Exists(path))
                            {
                                string contentid = att.Substring(att.LastIndexOf("/") + 1);
                                contentid = contentid.Substring(0, contentid.LastIndexOf("."));
                                contentid = contentid.Substring(0, contentid.LastIndexOf("_"));

                                LinkedResource objLinkedRes = new LinkedResource(path);
                                objLinkedRes.ContentId = contentid;
                                var mime = att.Substring(att.LastIndexOf(".") + 1);
                                if (mime.ToUpper().Contains("JPE") || mime.ToUpper().Contains("JPG"))
                                    mime = "jpeg";

                                objLinkedRes.ContentType = new System.Net.Mime.ContentType("image/" + mime);

                                resourceList.Add(objLinkedRes);

                                strContent = strContent.Replace(att, "cid:" + contentid);
                            }
                        }
                    }
                }

                AlternateView objHTLMAltView = AlternateView.CreateAlternateViewFromString(
                    strContent,System.Text.Encoding.GetEncoding("utf-8"),
                    new System.Net.Mime.ContentType("text/html").MediaType);

                foreach (LinkedResource res in resourceList)
                    objHTLMAltView.LinkedResources.Add(res);


                message.AlternateViews.Add(objHTLMAltView);
                message.IsBodyHtml = true;

                return "";
            }
            catch (Exception ex)
            {
                return Helper.FetchExceptionMessage(ex);
            }
        }

        private bool AppendMailSignature(Records record, EmailRecord erecord, ref string body, ref bool isEnterpriseGroup, ref string errMsg)
        {
            try
            {
                #region 找出需夾帶的簽名檔群組
                // 郵件案件:新增(第一次寄送)
                if (erecord.RawHeaderId == null || erecord.RawHeaderId.Value < 1)
                {
                    // 找出Agent資料
                    Agent agent = db.Agent.Find(record.AgentID);
                    if (agent == null)
                    {
                        errMsg = String.Format("Agent Not Found[AgentID={0}]", record.AgentID);
                        return false;
                    }

                    // 找出Agent的郵件群族
                    MailMember mailmember = db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID).FirstOrDefault();
                    if (mailmember != null && 
                        (MailSender_Enterprise.ToUpper().Contains(db.MailGroup.Find(mailmember.GroupID).Mailbox.ToUpper()) ||
                        db.MailGroup.Find(mailmember.GroupID).Mailbox.ToUpper().Contains(MailSender_Enterprise.ToUpper())))
                        isEnterpriseGroup = true;
                    else
                        isEnterpriseGroup = false;
                }
                // 郵件案件: 回覆或轉寄
                else
                {
                    // 找出原始郵件
                    EmailRawHeader erawheader = db.EmailRawHeader.Find(erecord.RawHeaderId);
                    if (erawheader == null)
                    {
                        errMsg = String.Format("EmailRawHeader Not Found[RawHeaderId={0}]", erecord.RawHeaderId);
                        return false;
                    }
                    if (erawheader.MsgTo.Contains(MailSender_Enterprise) || MailSender_Enterprise.Contains(erawheader.MsgTo))
                        isEnterpriseGroup = true;

                    // 找出Agent資料
                    Agent agent = db.Agent.Find(record.AgentID);
                    if (agent == null)
                    {
                        errMsg = String.Format("Agent Not Found2[AgentID={0}]", record.AgentID);
                        return false;
                    }
                    bool isEnterprise = false;
                    bool isPersonal = false;
                    foreach (var mm in db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID))
                    {
                        if (mm != null && 
                            (MailSender_Enterprise.ToUpper().Contains(db.MailGroup.Find(mm.GroupID).Mailbox.ToUpper()) ||
                            db.MailGroup.Find(mm.GroupID).Mailbox.ToUpper().Contains(MailSender_Enterprise.ToUpper())))
                            isEnterprise = true;

                        if (mm != null && 
                            (MailSender_Personal.ToUpper().Contains(db.MailGroup.Find(mm.GroupID).Mailbox.ToUpper()) ||
                            db.MailGroup.Find(mm.GroupID).Mailbox.ToUpper().Contains(MailSender_Personal.ToUpper())))
                            isPersonal = true;
                    }

                    if (isEnterprise && isPersonal)
                    {
                    }
                    else
                    {
                        if (isEnterprise)
                            isEnterpriseGroup = true;

                        if (isPersonal)
                            isEnterpriseGroup = false;
                    }
                }
                #endregion

                if (isEnterpriseGroup)
                {
                    EmailReplyCan erc = db.EmailReplyCan.Find(2);
                    erc.TmpContent = System.Text.Encoding.GetEncoding("utf-8").GetString(erc.TempCnt);
                    var variableAgent = db.EmailScheduleSetting.Where(s => s.Name == "AgentID").FirstOrDefault();
                    if (variableAgent != null)
                    {
                        string VarAgentId = variableAgent.Value;
                        if (VarAgentId != null && VarAgentId.Length > 0)
                        {
                            string agentName = record.AgentID;
                            var agent = db.Agent.Where(a => a.AgentID == record.AgentID).FirstOrDefault();
                            if (agent != null)
                                agentName = agent.AgentName;

                            erc.TmpContent = erc.TmpContent.Replace(VarAgentId, agentName);
                        }
                    }
                    if (body.IndexOf("<div id=\"ttcsoriginal\">") > 0)
                        body = body.Replace("<div id=\"ttcsoriginal\">", "<br />" + erc.TmpContent + "<div>");
                    else
                        body += "<br />" + erc.TmpContent;
                }
                else
                {
                    EmailReplyCan erc = db.EmailReplyCan.Find(1);
                    erc.TmpContent = System.Text.Encoding.GetEncoding("utf-8").GetString(erc.TempCnt);
                    var variableAgent = db.EmailScheduleSetting.Where(s => s.Name == "AgentID").FirstOrDefault();
                    if (variableAgent != null)
                    {
                        string VarAgentId = variableAgent.Value;
                        if (VarAgentId != null && VarAgentId.Length > 0)
                        {
                            string agentName = record.AgentID;
                            var agent = db.Agent.Where(a => a.AgentID == record.AgentID).FirstOrDefault();
                            if (agent != null)
                                agentName = agent.AgentName;

                            erc.TmpContent = erc.TmpContent.Replace(VarAgentId, agentName);
                        }
                    }

                    if (body.IndexOf("<div id=\"ttcsoriginal\">") > 0)
                        body = body.Replace("<div id=\"ttcsoriginal\">", "<br />" + erc.TmpContent + "<div>");
                    else
                        body += erc.TmpContent;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        private string CreateMailAttachment(ref MailMessage message, EmailRecord erecord)
        {
            string err = "";
            try
            {
                foreach(var attch in db.EmailSavedAttachment.Where(a => a.EmailRecordId == erecord.Id))
                {
                    String filename = attch.FileName.Trim();
                    var attachment = new Attachment(new MemoryStream(attch.CntBody), new System.Net.Mime.ContentType(attch.CntMedia));
                    attachment.Name = filename;
                    attachment.NameEncoding = System.Text.Encoding.GetEncoding("BIG5");
                    message.Attachments.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return err;
        }

        private MailMessage CreateMailMessage(Records record, EmailRecord erecord, string SendTo, string Cc, string Bcc, string MailSign,
                                                ref string Subject, ref string MailTo, ref string MsgReplyBody, ref int nForwardCount,
                                                ref bool isEnterpriseGroup, ref string errMsg, ref string SentBody)
        {
            MailMessage mail = new MailMessage();

            errMsg = AddMsgHeaderDefault(ref mail, erecord, SendTo, Cc, Bcc, ref Subject, ref MailTo, ref nForwardCount);
            if (errMsg.Length > 0)
                return null;

            //bool isEnterpriseGroup = false;
            if (MailSign == "1" && !AppendMailSignature(record, erecord, ref MsgReplyBody, ref isEnterpriseGroup, ref errMsg))
                return null;

            if (isEnterpriseGroup)
                mail.From = new MailAddress(MailSender_Enterprise, MailSeanderName_Enterprise);
            else
                mail.From = new MailAddress(MailSender_Personal, MailSeanderName_Personal);

            SentBody = MsgReplyBody.Replace("&lt;", "<")
                                   .Replace("&gt;", ">")
                                   .Replace("\\n", "<br />")
                                   .Replace("&amp;lt;", "<")
                                   .Replace("&amp;gt;", ">");
            errMsg = CreateMailBody(ref mail, MsgReplyBody.Replace("&lt;", "<")
                                                            .Replace("&gt;", ">")
                                                            .Replace("\\n", "<br />")
                                                            .Replace("&amp;lt;", "<")
                                                            .Replace("&amp;gt;", ">"));
            if (errMsg.Length > 0)
                return null;

            return mail;
        }

        private bool SendMail(bool isEnterpriseGroup, MailMessage mail, ref string errMsg)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                if (isEnterpriseGroup)
                {
                    smtp.Host = MailSmtp_Enterprise;
                    smtp.Port = Convert.ToInt32(MailPort_Enterprise);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(MailSender_Enterprise, MailPassword_Enterprise);
                    smtp.EnableSsl = MailSSL_Enterprise.Equals("1");
                    smtp.Send(mail);
                }
                else
                {
                    smtp.Host = MailSmtp_Personal;
                    smtp.Port = Convert.ToInt32(MailPort_Personal);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(MailSender_Personal, MailPassword_Personal);
                    smtp.EnableSsl = MailSSL_Personal.Equals("1");
                    //smtp.ServicePoint.MaxIdleTime = 1;
                    smtp.Timeout = 30000000;
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        private bool BackupSentMail(EmailRecord erecord, string Subject, string MailTo, string Cc, string Bcc, string MsgReplyBody, ref string errMsg)
        {
            try
            {
                Records record = db.Records.Find(erecord.RID);
                if (record == null)
                {
                    errMsg = String.Format("BackupSentMail: Record Not Found[EID={0}, RawID={1}]", erecord.Id, erecord.RawHeaderId);
                    return false;
                }

                EmailSentBox emailsentbox = new EmailSentBox();
                emailsentbox.RID = erecord.RID;
                emailsentbox.OrderNo = erecord.OrderNo.Value;

                emailsentbox.MsgSubject = Subject;
                emailsentbox.CodePage = System.Text.Encoding.GetEncoding("utf-8").CodePage;
                emailsentbox.BodyCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes(MsgReplyBody);
                emailsentbox.SendTo = MailTo;
                emailsentbox.SentOn = DateTime.Now;
                emailsentbox.SentBy = record.AgentID;// db.Agent.Find(AgentID).AgentName;
                emailsentbox.Cc = Cc;
                emailsentbox.Bcc = Bcc;
                db.EmailSentBox.Add(emailsentbox);
                
                EmailSavedBody esavebody = db.EmailSavedBody.Find(erecord.SaveCntIn);
                if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
                {
                    if(esavebody != null)
                        db.EmailSavedBody.Remove(esavebody);
                }
                else
                {
                    if (esavebody != null)
                    {
                        esavebody.FileName = Subject;
                        esavebody.CodePage = System.Text.Encoding.GetEncoding("utf-8").CodePage;
                        esavebody.BodyCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes(MsgReplyBody);
                        esavebody.SendTo = MailTo;
                        esavebody.Cc = Cc;
                        esavebody.Bcc = Bcc;
                    }
                }
                foreach(var esaveatt in db.EmailSavedAttachment.Where(sa => sa.EmailRecordId == erecord.Id))
                {
                    EmailSentAttachment esentattach = new EmailSentAttachment();
                    esentattach.FileName = esaveatt.FileName;
                    esentattach.CntMedia = esaveatt.CntMedia;
                    esentattach.CntBody = esaveatt.CntBody;
                    esentattach.EmailRecordId = erecord.Id;

                    db.EmailSentAttachment.Add(esentattach);
                    if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
                    {
                        db.EmailSavedAttachment.Remove(esaveatt);
                    }
                }

                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    errMsg += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errMsg += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                errMsg = String.Format("[{0}] {1}", erecord.RID, errMsg);
                return false;
            }
            catch (Exception ex)
            {
                errMsg = String.Format("[{0}] {1}", erecord.RID, ex.Message);
                return false;
            }
            return true;
        }

        [HttpPost]
        public ActionResult AjaxReplyEmailRecord(string RID, int EID, string SendTo, string Cc, string Bcc, string MailSig, string ProcessStatus, string MsgReplyBody, string MsgSubject)
        {
            string result = "0";

            try
            {
                // 找出案件
                Records record = db.Records.Find(RID);
                if (record == null)
                    return Json(new { result = String.Format("Record Not Found[RID={0}]", RID) }, JsonRequestBehavior.AllowGet);

                // 找出郵件案件
                EmailRecord erecord = db.EmailRecord.Find(EID);
                if (erecord == null)
                    return Json(new { result = String.Format("EmailRecord Not Found[RID={0}, EID={1}]", RID, EID) }, JsonRequestBehavior.AllowGet);

                if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
                {
                    if (erecord.EmailRawHeader.MsgSubject != MsgSubject)
                    {
                        erecord.MsgSubjectModifiedBy = record.AgentID;
                    }
                }

                int nForwardCount = 0;          // 轉寄次數
                if (erecord.ForwardCount != null)
                    nForwardCount = erecord.ForwardCount.Value;

                string Subject = MsgSubject;
                string MailTo = "";

                bool isEnterpriseGroup = true;
                // 建立郵件物件
                string SentBody = "";
                MailMessage mail = CreateMailMessage(record, erecord, SendTo, Cc, Bcc, MailSig,
                                                    ref Subject, ref MailTo, ref MsgReplyBody, ref nForwardCount, ref isEnterpriseGroup, ref result, ref SentBody);
                if (mail == null)
                    return Json(new { result = String.Format("CreateMailMessage:{0}", result) }, JsonRequestBehavior.AllowGet);

                // 夾帶郵件附檔
                result = CreateMailAttachment(ref mail, erecord);
                if (result.Length > 0)
                    return Json(new { result = String.Format("CreateMailAttachment:{0}", result) }, JsonRequestBehavior.AllowGet);

                // 寄送郵件
                if (!SendMail(isEnterpriseGroup, mail, ref result))
                    return Json(new { result = String.Format("SendMail:{0}", result) }, JsonRequestBehavior.AllowGet);

                // 郵件備份
                if (!BackupSentMail(erecord, Subject, MailTo, Cc, Bcc, MsgReplyBody, ref result))
                    return Json(new { result = String.Format("BackupSentMail:{0}", result) }, JsonRequestBehavior.AllowGet);

                // 更新郵件案件資訊
                if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
                    erecord.SaveCntIn = null;

                erecord.ProcessStatus = ProcessStatus;
                erecord.ReplyOn = DateTime.Now;
                erecord.ForwardCount = nForwardCount;
                db.Entry(erecord).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            var ret = new
            {
                result = result
            };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        public ActionResult AjaxUpdateEmailRecord(Records record, int eid, string SendTo, string MsgReplyBody,
                                                string ProcessStatus, string Cc, string Bcc, string MailSig, string MsgSubject, ServiceItem[] serviceItemList)
        {
            string flag_success = "-1";
            string errMsg = "";
            try
            {
                // 找出案件
                var recordmodified = db.Records.Find(record.RID);
                if (recordmodified == null)
                {
                    errMsg = String.Format("Record Not Found[RID={0}]", record.RID);
                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        errMsg = String.Format("ModelState Is Not Valid[RID={0}]", record.RID);
                    }
                    else
                    {
                        if (record.StatusID == null)
                        {
                            errMsg = String.Format("Record Status Is NULL[RID={0}]", record.RID);
                        }
                        else
                        {
                            // 結案
                            if (record.StatusID == 3)
                            {
                                int OriginalStatusID = recordmodified.StatusID.Value;

                                recordmodified.CustomerID = record.CustomerID;
                                recordmodified.CloseApproachID = record.CloseApproachID;
                                recordmodified.Comment = record.Comment;
                                recordmodified.ServiceGroupID = record.ServiceGroupID;
                                recordmodified.ServiceItemID = record.ServiceItemID;
                                recordmodified.StatusID = record.StatusID;
                                if (OriginalStatusID != 3)
                                {
                                    recordmodified.StatusID = 3;
                                    recordmodified.FinishDT = DateTime.Now;
                                }
                                db.Entry(recordmodified).State = EntityState.Modified;

                                IQueryable<RecordServiceMap> maps =
                                            db.RecordServiceMap.Where(m => m.RID == recordmodified.RID);
                                foreach (RecordServiceMap map in maps)
                                {
                                    db.RecordServiceMap.Remove(map);
                                }

                                foreach (ServiceItem item in serviceItemList)
                                {
                                    RecordServiceMap map = new RecordServiceMap();
                                    map.RID = recordmodified.RID;
                                    map.GroupID = item.GroupID;
                                    map.ItemID = item.ItemID;
                                    if (ModelState.IsValid)
                                    {
                                        db.RecordServiceMap.Add(map);
                                    }
                                }

                                string strRID = record.RID;
                                if (strRID.Contains("_B"))
                                    strRID = strRID.Substring(0, strRID.IndexOf("_B"));

                                // 相關案件通通結案
                                foreach (var r in db.Records.Where(r => r.RID.Contains(strRID) && r.RID != recordmodified.RID))
                                {
                                    if (r.StatusID != 3)
                                    {
                                        r.StatusID = 3;
                                        r.FinishDT = DateTime.Now;
                                    }
                                    db.Entry(r).State = EntityState.Modified;

                                    IQueryable<RecordServiceMap> maps2 =
                                            db.RecordServiceMap.Where(m => m.RID == r.RID);
                                    foreach (RecordServiceMap map in maps2)
                                    {
                                        db.RecordServiceMap.Remove(map);
                                    }

                                    foreach (ServiceItem item in serviceItemList)
                                    {
                                        RecordServiceMap map = new RecordServiceMap();
                                        map.RID = r.RID;
                                        map.GroupID = item.GroupID;
                                        map.ItemID = item.ItemID;
                                        if (ModelState.IsValid)
                                        {
                                            db.RecordServiceMap.Add(map);
                                        }
                                    }
                                }

                                // 刪除所有相關案件草稿
                                foreach (var er in db.EmailRecord.Where(r => r.RID.Contains(strRID) && r.SaveCntIn != null && r.SaveCntIn.Length > 0))
                                {
                                    // 如為新增案件, 不能刪
                                    if (er.RawHeaderId != null && er.RawHeaderId.Value > 0)
                                    {
                                        EmailSavedBody esavebody = db.EmailSavedBody.Find(er.SaveCntIn);
                                        if (esavebody != null)
                                            db.EmailSavedBody.Remove(esavebody);

                                        var esaveattachs = db.EmailSentAttachment.Where(ea => ea.EmailRecordId == er.Id);
                                        foreach (var esaveatt in esaveattachs)
                                            db.EmailSentAttachment.Remove(esaveatt);

                                        er.SaveCntIn = null;
                                    }
                                    er.ProcessStatus = null;
                                    db.Entry(er).State = EntityState.Modified;
                                }
                                
                                db.SaveChanges();
                                flag_success = "0";
                            }
                            // 一般儲存
                            else
                            {
                                recordmodified.CustomerID = record.CustomerID;
                                recordmodified.CloseApproachID = record.CloseApproachID;
                                recordmodified.Comment = record.Comment;
                                db.Entry(recordmodified).State = EntityState.Modified;

                                if (serviceItemList != null)
                                {
                                    IQueryable<RecordServiceMap> maps =
                                                db.RecordServiceMap.Where(m => m.RID == recordmodified.RID);
                                    foreach (RecordServiceMap map in maps)
                                    {
                                        db.RecordServiceMap.Remove(map);
                                    }

                                    foreach (ServiceItem item in serviceItemList)
                                    {
                                        RecordServiceMap map = new RecordServiceMap();
                                        map.RID = recordmodified.RID;
                                        map.GroupID = item.GroupID;
                                        map.ItemID = item.ItemID;
                                        if (ModelState.IsValid)
                                        {
                                            db.RecordServiceMap.Add(map);
                                        }
                                    }
                                }
                                string strRID = record.RID;
                                if (strRID.Contains("_B"))
                                    strRID = strRID.Substring(0, strRID.IndexOf("_B"));

                                // 相關案件通通結案
                                foreach (var r in db.Records.Where(r => r.RID.Contains(strRID) && r.RID != recordmodified.RID))
                                {
                                    if (serviceItemList != null)
                                    {
                                        IQueryable<RecordServiceMap> maps2 =
                                                db.RecordServiceMap.Where(m => m.RID == r.RID);
                                        foreach (RecordServiceMap map in maps2)
                                        {
                                            db.RecordServiceMap.Remove(map);
                                        }

                                        foreach (ServiceItem item in serviceItemList)
                                        {
                                            RecordServiceMap map = new RecordServiceMap();
                                            map.RID = r.RID;
                                            map.GroupID = item.GroupID;
                                            map.ItemID = item.ItemID;
                                            if (ModelState.IsValid)
                                            {
                                                db.RecordServiceMap.Add(map);
                                            }
                                        }
                                    }
                                }

                                foreach (var er in db.EmailRecord.Where(r => r.RID.Contains(strRID) && r.Id != eid))
                                {
                                    er.ProcessStatus = ProcessStatus;
                                    db.Entry(er).State = EntityState.Modified;
                                }

                                EmailRecord erecord = db.EmailRecord.Find(eid);
                                if (erecord == null)
                                {
                                    errMsg = String.Format("EmailRecord Not Found[EID={0}]", eid);
                                }
                                else
                                {
                                    erecord.ProcessStatus = ProcessStatus;

                                    if (SendTo != "-1")
                                    {
                                        EmailSavedBody esavebody = db.EmailSavedBody.Find(erecord.SaveCntIn);

                                        bool bFirstTime = false;
                                        // 第一次儲存
                                        if (esavebody == null)
                                        {
                                            erecord.SaveCntIn = String.Format("{0}{1}", erecord.RID, erecord.OrderNo);
                                            db.Entry(erecord).State = EntityState.Modified;

                                            esavebody = new EmailSavedBody();
                                            esavebody.Id = erecord.SaveCntIn;
                                            bFirstTime = true;
                                        }

                                        esavebody.SendTo = SendTo;
                                        esavebody.BodyCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes(MsgReplyBody);
                                        esavebody.CodePage = System.Text.Encoding.GetEncoding("utf-8").CodePage;
                                        esavebody.Cc = Cc;
                                        esavebody.Bcc = Bcc;
                                        esavebody.FileName = MsgSubject;    // 權宜之計, 存放郵件主旨

                                        if (MailSig.Contains("1"))
                                            esavebody.MailSignature = 1;
                                        else
                                            esavebody.MailSignature = 0;

                                        //郵件主旨
                                        if (erecord.OrderNo == 0)
                                        {
                                            // 進來郵件
                                            if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
                                            {
                                                EmailRawHeader erawheader = db.EmailRawHeader.Find(erecord.RawHeaderId);
                                                if (erawheader != null)
                                                {
                                                    // 主旨有變動
                                                    if (erawheader.MsgSubject != MsgSubject)
                                                    {
                                                        erecord.MsgSubjectModifiedBy = record.AgentID;
                                                        db.Entry(erecord).State = EntityState.Modified;
                                                    }
                                                }
                                            }
                                        }

                                        // 第一次儲存
                                        if (bFirstTime)
                                            db.EmailSavedBody.Add(esavebody);
                                        else
                                            db.Entry(esavebody).State = EntityState.Modified;
                                    }

                                    db.SaveChanges();
                                    flag_success = "0";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = Helpers.Error.FetchExceptionMessage(ex);
            }
            finally
            {
               
            }

            var ret = new
            {
                result = flag_success,
                err = errMsg
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AjaxRejectForEmailRecord(int eid, string Reason)
        {
            int flag_success = -1;
            string errMsg = "";
            if (ModelState.IsValid)
            {
                EmailRecord erecord = db.EmailRecord.Find(eid);

                if (erecord == null)
                {
                    errMsg = String.Format("EmailRecord Not Found[ERID={0}]", eid);
                }
                else
                {
                    Records record = db.Records.Find(erecord.RID);
                    if (record == null)
                    {
                        errMsg = String.Format("Record Not Found[RID={0}]", erecord.RID);
                    }
                    else
                    {
                        try
                        {
                            erecord.CurrAgentId = record.AgentID;
                            erecord.RejectReason = Reason;
                            db.Entry(erecord).State = EntityState.Modified;

                            string strKey = record.RID;
                            if (strKey.Contains("_B"))
                                strKey = strKey.Substring(0, strKey.IndexOf("_B"));

                            // Record Update
                            foreach (var recordModified in from rs in db.Records where rs.RID.Contains(strKey) select rs)
                            {
                                recordModified.StatusID = 1;
                                recordModified.AgentID = null;
                                db.Entry(recordModified).State = EntityState.Modified;
                            }

                            // EmailRecord Update
                            foreach (var erecordModified in from ers in db.EmailRecord where ers.RID.Contains(strKey) && ers.Id != eid select ers)
                            {
                                erecord.CurrAgentId = null;
                                erecord.RejectReason = null;
                                db.Entry(erecord).State = EntityState.Modified;
                            }

                            db.SaveChanges();
                            flag_success = 0;
                        }
                        catch (Exception ex)
                        {
                            errMsg = String.Format("System Error[{0}]", ex.Message);
                        }
                    }
                }
            }
            else
            {
                errMsg = String.Format("ModelState Is Not Valid[EID={0}, Reason={1}]", eid, Reason);
            }

            var ret = new
            {
                result = flag_success,
                err = errMsg
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _PartialOpenPopupReply(string agentId)
        {
            var emailreplycans = db.EmailReplyCan.Where(e => e.Id != 1 && e.Id != 2);
            foreach (var erc in emailreplycans)
            {
                erc.TmpContent = System.Text.Encoding.GetEncoding("utf-8").GetString(erc.TempCnt);
                var variableAgent = db.EmailScheduleSetting.Where(s => s.Name == "AgentID").FirstOrDefault();
                if (variableAgent != null)
                {
                    string VarAgentId = variableAgent.Value;
                    if (VarAgentId != null && VarAgentId.Length > 0)
                    {
                        string agentName = agentId;
                        var agent = db.Agent.Where(a => a.AgentID == agentId).FirstOrDefault();
                        if (agent != null)
                            agentName = agent.AgentName;

                        erc.TmpContent = erc.TmpContent.Replace(VarAgentId, agentName);
                    }
                }
            }
            return PartialView("~/Views/Record/_PartialOpenPopupReply.cshtml", emailreplycans.ToList());
        }
        public ActionResult _PartialOpenPopupContacts()
        {
            return PartialView("~/Views/Record/_PartialOpenPopupContacts.cshtml", db.EmailContacts.ToList());
        }

        public ActionResult _PartialOpenPopupReject()
        {
            return PartialView("~/Views/Record/_PartialOpenPopupReject.cshtml", db.EmailRejectReason.ToList());
        }

        // 相關案件清單
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult _PartialOpenPopupForwards(string id)
        {
            string strKey = id;
            if (id.Contains("_B"))
                strKey = id.Substring(0, id.IndexOf("_B"));

            var emailrecords = db.EmailRecord.Where(e => e.RID.Contains(strKey) && e.RawHeaderId != null && e.RawHeaderId.Value > 0);

            ViewBag.SentBoxes = db.EmailSentBox.Where(e => e.RID.Contains(strKey)).ToList();

            return PartialView("~/Views/Record/_PartialOpenPopupForwards.cshtml", emailrecords.ToList());
        }

        // 相關案件之詳細郵件內容
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult _PartialOpenPopupForwardDetail(int id, string source)
        {
            // 原始郵件
            if (source == "0")
            {
                EmailRecord erecord = db.EmailRecord.Find(id);
                if (erecord == null)
                {
                    return HttpNotFound();
                }

                EmailRawHeader erawheader = db.EmailRawHeader.Find(erecord.RawHeaderId);
                if (erawheader == null)
                {
                    return HttpNotFound();
                }
                string body = "", attachlist = "", errMsg = "";
                List<SelectListItem> attachments = new List<SelectListItem>();

                BuildUpEmailBodyAndAttachmentsFromRawHeader(erecord, ref body, ref attachments, ref attachlist, ref errMsg);
                    //BuildUpEmailBodyAndAttachmentsFromDraft(erecord, ref body, ref attachments, ref attachlist, ref errMsg);
                
                ViewBag.MsgReplyBody = body;
                ViewBag.Attachments = attachments;

                return PartialView("~/Views/Record/_PartialOpenPopupForwardDetail.cshtml", erawheader);
            }
            // 寄件備份
            else if (source == "1")
            {
                EmailSentBox esentbox = db.EmailSentBox.Find(id);
                if (esentbox == null)
                {
                    return HttpNotFound();
                }
                EmailRawHeader erawheader = new EmailRawHeader();

                erawheader.MsgFrom = esentbox.SendTo;
                erawheader.MsgSubject = esentbox.MsgSubject;
                erawheader.MsgSentOn = esentbox.SentOn.Value;

                string body = "", attachlist = "", errMsg = "";
                List<SelectListItem> attachments = new List<SelectListItem>();

                BuildUpEmailBodyAndAttachmentsFromSentBox(esentbox, ref body, ref attachments, ref attachlist, ref errMsg);

                ViewBag.MsgReplyBody = body.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\\n", "<br />");
                ViewBag.Attachments = attachments;

                return PartialView("~/Views/Record/_PartialOpenPopupForwardDetail.cshtml", erawheader);
            }

            return PartialView("~/Views/Record/_PartialOpenPopupForwardDetail.cshtml", new EmailRawHeader());
        }

        public ActionResult AjaxGetMaxRequestLen()
        {
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;

            var ret = new
            {
                result = section.MaxRequestLength
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult _UploadFiles(int id)
        {
            string result = "";
            string filename = "";
            if (Request.IsAjaxRequest())
            {
                int attachCount = db.EmailSavedAttachment.Count(a => a.EmailRecordId == id);
                
                if (Request.Files.Count == 1)
                {
                    foreach (string fName in Request.Files)
                    {
                        var f = Request.Files[fName] as HttpPostedFileBase;

                        string mimeType = f.ContentType;
                        Stream fileStream = f.InputStream;
                        string fileName = Path.GetFileName(f.FileName);
                        int fileLength = f.ContentLength;
                        if (fileLength < 1)
                        {
                            result = "File Size Cannot be 0!!";
                        }
                        else
                        {
                            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                            if (fileLength > section.MaxRequestLength*1000)
                            {
                                result = String.Format("超出檔案最大上傳容量: {0} KB", section.MaxRequestLength);
                            }
                            else
                            {
                                byte[] fileData = new byte[fileLength];
                                fileStream.Read(fileData, 0, fileLength);

                                EmailSavedAttachment emailsavedattach =
                                    db.EmailSavedAttachment.Where(a => a.EmailRecordId == id &&
                                                                        a.FileName.ToUpper().Equals(fileName.ToUpper())).FirstOrDefault();

                                if (emailsavedattach == null)
                                {
                                    if (attachCount >= 10)
                                    {
                                        result = "夾帶檔案數量已達上限";
                                    }
                                    else
                                    {
                                        emailsavedattach = new EmailSavedAttachment();
                                        emailsavedattach.EmailRecordId = Convert.ToInt32(id);
                                        emailsavedattach.FileName = fileName;
                                        emailsavedattach.CntMedia = mimeType;
                                        emailsavedattach.CntBody = fileData;
                                        db.EmailSavedAttachment.Add(emailsavedattach);

                                    }
                                }
                                else
                                {
                                    emailsavedattach.CntMedia = mimeType;
                                    emailsavedattach.CntBody = fileData;
                                    db.Entry(emailsavedattach).State = EntityState.Modified;
                                    db.SaveChanges();
                                }

                                try
                                {
                                    db.SaveChanges();
                                    var esa = db.EmailSavedAttachment.Where(a => a.EmailRecordId == id);
                                    filename = "";
                                    if (esa != null)
                                    {
                                        foreach (var a in esa)
                                        {
                                            filename += "|" + a.Id + ":" + a.FileName;
                                        }
                                    }

                                    if (filename.Length > 0)
                                        filename = filename.Substring(1);
                                }
                                catch (Exception ex)
                                {
                                    result = ex.Message;
                                }
                            }
                        }
                    }
                }
                else
                {
                    result = "File count should be 1 but it is equal to: " + Request.Files.Count;
                }
            }
            else
            {
                result = "There is no file in this request.";
            }
            return Json(new { ResultError = result, NewFileName = filename });
        }

        public ActionResult DownloadFile(string id)
        {
            EmailSavedAttachment emailsavedattach = db.EmailSavedAttachment.Find(Convert.ToInt32(id));
            if (emailsavedattach == null)
            {
                return HttpNotFound();
            }

            return File(emailsavedattach.CntBody, emailsavedattach.CntMedia, emailsavedattach.FileName);
        }

        public ActionResult DeleteFile(int id)
        {
            EmailSavedAttachment emailsavedattach = db.EmailSavedAttachment.Find(id);

            if (emailsavedattach == null)
            {
                return HttpNotFound();
            }
            string filename = "";
            string result = "";
            int nEmailRecordID = emailsavedattach.EmailRecordId;
            try
            {
                db.EmailSavedAttachment.Remove(emailsavedattach);
                db.SaveChanges();

                var esa = db.EmailSavedAttachment.Where(a => a.EmailRecordId == nEmailRecordID);
                filename = "";
                if (esa != null)
                {
                    foreach (var a in esa)
                    {
                        filename += "|" + a.Id + ":" + a.FileName;
                    }
                }

                if (filename.Length > 0)
                    filename = filename.Substring(1);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(new { ResultError = result, NewFileName = filename }, JsonRequestBehavior.AllowGet);
        }
                
        public ActionResult AjaxEmailRecordCreate(string agentId)
        {
            string errMsg = "", RID = "", EID = "";

            string today = DateTime.Now.ToString("yyyyMMdd");
            string type = today + "2";            
            try
            {
                string strGroupID = "";
                var agent = db.Agent.Find(agentId);
                if (agent != null)
                {
                    var mm = db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID);
                    if (mm != null)
                        strGroupID = mm.FirstOrDefault().GroupID;
                }

                var record = db.Records.Where(d => d.RID.StartsWith(type) && !d.RID.Contains("_B")).Max(d => d.RID);
                if (record == null)
                {
                    RID = type + "0001";
                }
                else
                {
                    RID = (Convert.ToInt64(record) + 1).ToString();
                }
                // 先存, 避免被搶先
                Records recordNew = new Records();
                recordNew.RID = RID;
                recordNew.TypeID = 3;
                recordNew.AgentID = agentId;
                recordNew.StatusID = 2;
                recordNew.IncomingDT = DateTime.Now;
                recordNew.GroupID = strGroupID;

                db.Records.Add(recordNew);
                db.SaveChanges();

                var expirelen = db.EmailScheduleSetting.Where(s => s.Name == "ExpireLen").FirstOrDefault();

                    int nExpireLen = 4;
                    if (expirelen != null)
                    {
                        nExpireLen = Convert.ToInt32(expirelen.Value);
                    }

                EmailRecord erecord = new EmailRecord();
                erecord.RID = RID;
                erecord.ExpireOn = DateTime.Now.AddHours(nExpireLen);
                erecord.IncomeOn = DateTime.Now;
                erecord.AssignOn = DateTime.Now;
                erecord.InitAgentId = agentId;
                erecord.CurrAgentId = agentId;
                erecord.AssignCount = 0;
                erecord.OrderNo = 0;
                erecord.SaveCntIn = String.Format("{0}{1}", erecord.RID, erecord.OrderNo);
                db.EmailRecord.Add(erecord);

                EmailSavedBody esavebody = db.EmailSavedBody.Find(erecord.SaveCntIn);
                if (esavebody == null)
                {
                    esavebody = new EmailSavedBody();
                    esavebody.Id = erecord.SaveCntIn;
                    db.EmailSavedBody.Add(esavebody);
                }
                else
                {
                    esavebody.Bcc = "";
                    esavebody.BodyCnt = null;
                    esavebody.Cc = "";
                    esavebody.CodePage = null;
                    esavebody.FileName = "";
                    esavebody.MailSignature = 0;
                    esavebody.SendTo = "";
                }
                
                db.SaveChanges();
                
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                /*Records record = db.Records.Find(RID);
                if (record != null)
                {
                    var erecordlist = db.EmailRecord.Where(er => er.RID == RID);
                    foreach (EmailRecord erecord in erecordlist)
                    {
                        if (erecord.SaveCntIn != null && erecord.SaveCntIn.Length > 0)
                        {
                            EmailSavedBody esb = db.EmailSavedBody.Find(erecord.SaveCntIn);
                            if (esb != null)
                                db.EmailSavedBody.Remove(esb);

                            db.EmailRecord.Remove(erecord);
                        }
                    }
                    db.Records.Remove(record);

                    db.SaveChanges();
                }*/
            }
            var ret = new
            {
                result = errMsg,
                RID = RID,
                EID = EID
            };

            if(errMsg.Length < 1)
                return RedirectToAction("JsonInfo", new { id = RID });
            else
                return Json(ret, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult AjaxCreate(Records id)
        {
            int flag_success = -1;
            int error_count = 0;
            string RID = "None"; 
            string today;   
            dynamic ret;

            string ex_msg = "";

            today = DateTime.Now.ToString("yyyyMMdd");
            //區分郵件電話
            string type = today + "1";
            var curr_RID = new object();
            try
            {
                curr_RID = db.Records.Where(d => d.RID.StartsWith(type) && !d.RID.Contains("_B")).Max(d => d.RID);

                if (curr_RID != null)
                {
                    RID = (Convert.ToInt64(curr_RID) + 1).ToString();
                }
                else
                {
                    RID = type + "0001";
                }
                id.RID = RID;

                /*
                string group_id = null;
                IQueryable<MailMember> mailmember =
                    from m in db.MailMember
                    join a in db.Agent on m.CTILoginID equals a.CTILoginID into o_agent
                    from a in o_agent.DefaultIfEmpty()
                    where a.AgentID == id.AgentID
                    select m;

                if (mailmember.Count() < 1)
                {
                    ret = new {
                        result = -1,
                        RID = RID,
                        msg = "建立案件時找尋不到客服群組",
                    };
                    return Json(ret, JsonRequestBehavior.AllowGet);
                }
                id.GroupID = mailmember.First().GroupID;
                */

                if (ModelState.IsValid)
                {
                    db.Records.Add(id);
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
                                ex_msg += string.Format("Retry {0} failure", Def.MaxRetry);
                                throw ex;
                            }
                            error_count++;
                            Thread.Sleep(Def.RetryWait);
                        }
                    }
                    flag_success = 0;
                }
                else
                {
                    ex_msg += Helpers.Error.GetModelCheckString(ModelState.Values);
                    flag_success = -4;
                }
            }
            catch (Exception ex)
            {
                ex_msg += Helpers.Error.FetchExceptionMessage(ex);
                flag_success = -3;
            }

            ret = new
            {
                result = flag_success,
                RID = RID,
                msg = ex_msg
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
            //return Json(ret, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult AjaxUpdate(Records records, ServiceItem[] serviceItemList)
        {
            int flag_success = -1;
            string err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(records).State = EntityState.Modified;
                }

                IQueryable<RecordServiceMap> maps =
                    db.RecordServiceMap.Where(m => m.RID == records.RID);
                foreach (RecordServiceMap map in maps)
                {
                    db.RecordServiceMap.Remove(map);
                }

                if (serviceItemList != null)
                {

                    List<RecordServiceMap> addedMaps = new List<RecordServiceMap>();
                    foreach (ServiceItem item in serviceItemList)
                    {
                        RecordServiceMap map = new RecordServiceMap();
                        map.RID = records.RID;
                        map.GroupID = item.GroupID;
                        map.ItemID = item.ItemID;

                        //20150108, Clyde, De-dup duplicated ServiceItem. This should be fix in front-end.
                        if (addedMaps.Where(m => m.RID == map.RID && m.GroupID == map.GroupID && m.ItemID == map.ItemID).Count() > 0)
                        {
                            continue;
                        }

                        addedMaps.Add(map);

                        if (ModelState.IsValid)
                        {
                            db.RecordServiceMap.Add(map);
                        }
                    }
                }
                db.SaveChanges();
                flag_success = 0;
            }
            catch (Exception ex)
            {
                err = Helpers.Error.FetchExceptionMessage(ex);
                flag_success = -1;
            }

            var ret = new
            {
                result = flag_success,
                msg = err
            };

            return Content(JsonConvert.SerializeObject(ret), Def.JsonMimeType);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
