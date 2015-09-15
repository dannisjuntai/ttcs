using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Areas.EmailSrv.Models;

using PagedList;
using System.Web.UI;
using System.IO;
using System.Net.Mail;
using System.Web.Security;
using System.Data.Objects.SqlClient;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class EmailAdminController : Controller
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
        // GET: /EmailAdmin/IndexQueue
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult IndexQueue(string type, string condition, string sortOrder, int Desc = 0, int page = 1)
        {   
            int currentPage = page < 1 ? 1 : page;
            //IQueryable<EEmailRecord> emailrecords = from s in db.EmailRecord where s.RID == null && s.UnRead == null orderby s.Id select s;
            IQueryable<EEmailRecord> emailrecords = from s in db.EmailRecord where s.RID == null orderby s.Id select s;

            MailMemberProcessQueue(userGroupId, ref emailrecords);

            #region 網頁物件準備
            
            ViewBag.Condition = condition;
            ViewBag.Order = sortOrder;
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Desc = Desc;
            if (sortOrder != null && sortOrder.Length > 0)
                ViewBag.Desc = (Desc == 1) ? 0 : 1;

            #endregion

            return View(emailrecords.ToPagedList(currentPage, pageSize));
        }

        //
        // GET: /EmailAdmin/IndexWait
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult IndexWait(EmailAdmin_SearchCondition searchcondition, string searchconditionStr, string sortOrder, int Desc = 0, int page = 1)
        {
            if (searchconditionStr != null && searchconditionStr.Length > 0)
                searchcondition = EmailAdmin_SearchCondition.Deserialize(searchconditionStr);

            int currentPage = page < 1 ? 1 : page;
            var records = db.Records.Include(e => e.Customers)
                                    .Include(e => e.EmailRecord)
                                    .Include(e => e.RecordStatus)
                                    .Include(e => e.RecordType)
                                    .Include(e => e.ServiceItem).Where(r => r.TypeID == 3);

            records = records.Where(r => r.AgentID == null || r.AgentID.Length < 1);

            DeadLineProcess(records);
            MailMemberProcessNormal(userGroupId, ref records);
            SearchAndSort(searchcondition, sortOrder, ref records, Desc);

            #region 網頁物件準備
            if (searchcondition.Filter_Record_StatusID == null)
                searchcondition.Filter_Record_StatusID = "";

            if (searchcondition.Filter_Record_TypeID == null)
                searchcondition.Filter_Record_TypeID = "";

            if (searchcondition.type == null)
                searchcondition.type = "1";

            if (searchcondition.condition == null)
                searchcondition.condition = "";

            ViewBag.SearchCondition = searchcondition;
            ViewBag.Order = sortOrder;
            ViewBag.NumberBegin = pageSize * (page - 1);
            ViewBag.Desc = Desc;
            if (sortOrder != null && sortOrder.Length > 0)
                ViewBag.Desc = (Desc == 1) ? 0 : 1;

            ViewBag.Page = page;
            ViewBag.Source = "1";
            #endregion

            var records2 = records.ToPagedList(currentPage, pageSize);
            foreach (var rd in records2)
            {
                string strItemNames = "";
                foreach (var item in db.RecordServiceMap.Where(s => s.RID == rd.RID))
                {
                    int nGroupID = item.GroupID;
                    int nItemID = item.ItemID;
                    strItemNames += "," + db.ServiceItem.FirstOrDefault(si => si.GroupID == nGroupID && si.ItemID == nItemID).ItemDesc;
                }
                if (strItemNames.Length > 0)
                    strItemNames = strItemNames.Substring(1);

                rd.ServiceItemNames = strItemNames;

            }
            return View(records2);
        }

        //
        // GET: /EmailAdmin/
        /*[OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Index(string type, string condition, string sortOrder, int Desc = 0, int page = 1)
        {   
            int currentPage = page < 1 ? 1 : page;
            var records = db.Records.Include(e => e.Customers).Include(e => e.EmailRecord).Include(e => e.RecordStatus).Include(e => e.RecordType).Include(e => e.ServiceItem);
            
            Response.AddHeader("Refresh", "60");

            MailMemberProcess(userGroupId, ref records);
            SearchAndSort(type, condition, sortOrder, ref records, Desc);

            var records2 = records.ToPagedList(currentPage, pageSize);

            DeadLineProcess(ref records2);
            //MailMemberProcess(userGroupId, ref records2);

            //DeadLineProcess(ref records);
            //MailMemberProcess(userGroupId, ref records);

            //SearchAndSort(type, condition, sortOrder, ref records, Desc);

            #region 網頁物件準備            
            ViewBag.NumberMax = db.EmailFilterRule.Count();
            ViewBag.NumberBegin = pageSize * (page - 1);

            ViewBag.Desc = Desc;
            if(sortOrder != null && sortOrder.Length > 0)
                ViewBag.Desc = (Desc == 1) ? 0 : 1;

            ViewBag.Type = type;
            ViewBag.Condition = condition;
            ViewBag.Order = sortOrder;
            ViewBag.Page = page;
            ViewBag.Source = "0";
            #endregion
                        
            //return View(records.ToPagedList(currentPage, pageSize));
            return View(records2);
        }*/

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Index(EmailAdmin_SearchCondition searchcondition, string searchconditionStr, string sortOrder, int Desc = 0, int page = 1)
        {
            if (searchconditionStr != null && searchconditionStr.Length > 0)
            {
                searchcondition = EmailAdmin_SearchCondition.Deserialize(searchconditionStr);
            }

            if (searchcondition.Filter_Record_StatusID == null)
                searchcondition.Filter_Record_StatusID = "1,2";

            if (searchcondition.Filter_Record_StatusID == "0")
                searchcondition.Filter_Record_StatusID = "";

            int currentPage = page < 1 ? 1 : page;
            var records = db.Records.Include(e => e.Customers)
                                    .Include(e => e.EmailRecord)
                                    .Include(e => e.RecordStatus)
                                    .Include(e => e.RecordType)
                                    .Include(e => e.ServiceItem);
            DeadLineProcess(records);
            SearchAndSort(searchcondition, sortOrder, ref records, Desc);
            MailMemberProcessNormal(userGroupId, ref records);

            #region 網頁物件準備
            ViewBag.NumberBegin = pageSize * (page - 1);

            ViewBag.Desc = Desc;
            if (sortOrder != null && sortOrder.Length > 0)
                ViewBag.Desc = (Desc == 1) ? 0 : 1;

            if (searchcondition.Filter_Record_StatusID == null)
                searchcondition.Filter_Record_StatusID = "";

            if (searchcondition.Filter_Record_TypeID == null)
                searchcondition.Filter_Record_TypeID = "";

            if (searchcondition.type == null)
                searchcondition.type = "1";

            if (searchcondition.condition == null)
                searchcondition.condition = "";

            ViewBag.SearchCondition = searchcondition;
            ViewBag.Order = sortOrder;
            ViewBag.Page = page;
            ViewBag.Source = "0";
            #endregion

            var records2 = records.ToPagedList(currentPage, pageSize);
            foreach (var rd in records2)
            {
                string strItemNames = "";
                foreach (var item in db.RecordServiceMap.Where(s => s.RID == rd.RID))
                {
                    int nGroupID = item.GroupID;
                    int nItemID = item.ItemID;
                    strItemNames += "," + db.ServiceItem.FirstOrDefault(si => si.GroupID == nGroupID && si.ItemID == nItemID).ItemDesc;
                }
                if (strItemNames.Length > 0)
                    strItemNames = strItemNames.Substring(1);

                rd.ServiceItemNames = strItemNames;

            }
            return View(records2);
        }

        public JsonResult RecordDetail(int id)
        {
            EEmailRecord erecord = db.EmailRecord.Find(id);
            ERecords record = db.Records.Find(erecord.RID);
            var ServiceMap = new SelectList(from s in db.RecordServiceMap where s.RID == record.RID select s, "GroupID", "ItemID");
            string strItemNames = "";
            foreach (var item in ServiceMap)
            {
                int nGroupID = Convert.ToInt32(item.Value);
                int nItemID = Convert.ToInt32(item.Text);
                strItemNames += "," + db.ServiceItem.FirstOrDefault(si => si.GroupID == nGroupID && si.ItemID == nItemID).ItemDesc;
            }
            if (strItemNames.Length > 0)
                strItemNames = strItemNames.Substring(1);
            var retobj = new
            {
                AgentID = record.AgentID == null ? "" : record.AgentID,
                ExpireOn = erecord.ExpireOn == null? "": erecord.ExpireOn.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                AssignCount = erecord.AssignCount == null? "": erecord.AssignCount.Value.ToString(),
                StatusID = record.StatusID == null ? 0 : record.RecordStatus.ID,
                ServiceGroupID = record.ServiceGroupID == null ? 0 : record.ServiceGroupID,
                ServiceItemID = record.ServiceItemID == null ? 0 : record.ServiceItemID,
                ServiceItemNames = strItemNames

            };
            return Json(retobj, JsonRequestBehavior.AllowGet);
        }

        [ActionName("Detail")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialDetail(string AgentID, string RID, int EID, string Source, string itemIDs, string groupIDs)
        {
            if (ModelState.IsValid && (AgentID.Length > 0 && db.Agent.Find(AgentID).EmailIsLogin == 1))
            {
                string strKey = RID;
                if (RID.Contains("_B"))
                    strKey = RID.Substring(0, RID.IndexOf("_B"));

                EEmailRecord erecordmodified = db.EmailRecord.Find(EID);
                ERecords record = db.Records.Find(RID);
                int nAssignCount = 0;
                if (erecordmodified != null && erecordmodified.AssignCount != null)
                    nAssignCount = erecordmodified.AssignCount.Value;


                var expirelen = db.EmailScheduleSetting.Where(s => s.Name == "ExpireLen").FirstOrDefault();

                int nExpireLen = 4;
                if (expirelen != null)
                    nExpireLen = Convert.ToInt32(expirelen.Value);

                string strGroupID = "";
                var agent = db.Agent.Find(AgentID);
                if (agent != null)
                {
                    var mm = db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID);
                    if (mm != null)
                        strGroupID = mm.FirstOrDefault().GroupID;
                }
                foreach (var recordModified in from rs in db.Records where rs.RID.Contains(strKey) select rs)
                {
                    if (record.StatusID != 3)
                    {
                        #region 郵件的基本資料
                        foreach (var erecord in db.EmailRecord.Where(e => e.RID == recordModified.RID))
                        {
                            // 處理期限 = 現在時間加上4小時
                            erecord.ExpireOn = DateTime.Now.AddHours(nExpireLen);

                            erecord.CurrAgentId = AgentID;
                            erecord.AssignOn = DateTime.Now;

                            // A.轉派的客服人員不同於原先客服人員
                            if (recordModified.AgentID != AgentID)
                            {
                                // 轉派次數加1
                                erecord.AssignCount = nAssignCount + 1;

                            }
                            // 清空 "退回" 的相關資料
                            erecord.RejectReason = "";

                            db.Entry(erecord).State = EntityState.Modified;
                        }
                        #endregion

                        #region 案件基本資料
                        // 指派客服人員
                        recordModified.AgentID = AgentID;
                        recordModified.GroupID = strGroupID;
                        #endregion
                    }

                    if (groupIDs.StartsWith(","))
                        groupIDs = groupIDs.Substring(1);

                    if (itemIDs.StartsWith(","))
                        itemIDs = itemIDs.Substring(1);

                    string[] arrGroupIDs = groupIDs.Split(',');
                    string[] arrItemIDs = itemIDs.Split(',');

                    if (arrGroupIDs.Length == arrItemIDs.Length && groupIDs.Length > 0 && itemIDs.Length > 0)
                    {
                        int nCount = 0;
                        foreach (var sm in db.RecordServiceMap.Where(rs => rs.RID == recordModified.RID))
                        {
                            if (nCount < arrGroupIDs.Length)
                            {
                                sm.GroupID = Convert.ToInt32(arrGroupIDs[nCount]);
                                sm.ItemID = Convert.ToInt32(arrItemIDs[nCount]);

                                db.Entry(sm).State = EntityState.Modified;
                            }
                            else
                            {
                                db.RecordServiceMap.Remove(sm);
                            }
                            nCount++;
                        }

                        for (; nCount < arrGroupIDs.Length; nCount++)
                        {
                            ERecordServiceMap ersm = new ERecordServiceMap();
                            ersm.RID = recordModified.RID;
                            ersm.GroupID = Convert.ToInt32(arrGroupIDs[nCount]);
                            ersm.ItemID = Convert.ToInt32(arrItemIDs[nCount]);
                            db.RecordServiceMap.Add(ersm);
                        }
                    }
                    db.Entry(recordModified).State = EntityState.Modified;
                }


                db.SaveChanges();

                return RedirectToAction("Index");
            }

            if (AgentID.Length < 1)
            {
                ModelState.AddModelError("AgentID", "請選擇客服人員");
            }
            else if (db.Agent.Find(AgentID).EmailIsLogin != 1)
            {
                ModelState.AddModelError("AgentID", "此客服人員, 目前未郵件值機, 無法轉派");
            }

            ViewBag.Source = Source;
            ViewBag.Disabled = 0;
            return View(GetDetailObject(EID));
        }

        public ActionResult DeleteAllGarbage()
        {
            var emailgarbages = db.EmailRecord.Where(e => e.Garbage != null && e.Garbage > 0);
            foreach (var emailrecord in emailgarbages)
            {
                var rawheaderid = emailrecord.RawHeaderId;

                db.EmailRecord.Remove(emailrecord);

                if (rawheaderid == null)
                    continue;

                var rawheader = db.EmailRawHeader.Find(rawheaderid);
                if (rawheader != null)
                {
                    rawheader.Completed = 0;
                    db.Entry(rawheader).State = EntityState.Modified;
                }
            }
            db.SaveChanges();

            return RedirectToAction("IndexQueue");
        }

        [HttpPost]
        public ActionResult _PartialDetailQueue(int Id, string AgentID)
        {
            int nId = Id;
            if (nId > 0 && AgentID.Length > 0 && db.Agent.Find(AgentID).EmailIsLogin == 1)
            {
                var emailrecord = db.EmailRecord.Find(nId);
                if (emailrecord != null)
                {
                    var expirelen = db.EmailScheduleSetting.Where(s => s.Name == "ExpireLen").FirstOrDefault();

                    if (emailrecord.RID == null || emailrecord.RID.Length < 1)
                    {
                        int nExpireLen = 4;
                        if (expirelen != null)
                        {
                            nExpireLen = Convert.ToInt32(expirelen.Value);
                        }
                        emailrecord.ExpireOn = DateTime.Now.AddHours(nExpireLen);
                        emailrecord.IncomeOn = DateTime.Now;
                        emailrecord.AssignOn = DateTime.Now;
                        emailrecord.InitAgentId = AgentID;
                        emailrecord.CurrAgentId = AgentID;
                        emailrecord.AssignCount = 0;
                        emailrecord.ForwardCount = 0;
                        emailrecord.OrderNo = 0;
                        emailrecord.UnRead = 1;
                        emailrecord.Garbage = null;

                        var record = new ERecords();
                        record.TypeID = 3;
                        record.AgentID = AgentID;
                        record.StatusID = 1;
                        record.IncomingDT = DateTime.Now;

                        var customers = db.Customers;
                        var emailrawheader = db.EmailRawHeader.Find(emailrecord.RawHeaderId);
                        if (emailrawheader != null)
                        {
                            record.GroupID = CommonUtilities.GetMailGroupIDByMsgTo(db, emailrawheader.MsgTo);

                            var msgfrom = emailrawheader.MsgFrom;
                            foreach (var ct in customers)
                            {
                                bool bFound = false;
                                if (ct.Contact1_Email1 != null && ct.Contact1_Email1.Length > 0)
                                {
                                    if (msgfrom.Contains(ct.Contact1_Email1))
                                        bFound = true;
                                }

                                if (!bFound)
                                {
                                    if (ct.Contact1_Email2 != null && ct.Contact1_Email2.Length > 0)
                                    {
                                        if (msgfrom.Contains(ct.Contact1_Email2))
                                            bFound = true;
                                    }
                                }

                                if (!bFound)
                                {
                                    if (ct.Contact2_Email1 != null && ct.Contact2_Email1.Length > 0)
                                    {
                                        if (msgfrom.Contains(ct.Contact2_Email1))
                                            bFound = true;
                                    }
                                }

                                if (!bFound)
                                {
                                    if (ct.Contact2_Email2 != null && ct.Contact2_Email2.Length > 0)
                                    {
                                        if (msgfrom.Contains(ct.Contact2_Email2))
                                            bFound = true;
                                    }
                                }

                                if (!bFound)
                                {
                                    if (ct.Contact3_Email1 != null && ct.Contact3_Email1.Length > 0)
                                    {
                                        if (msgfrom.Contains(ct.Contact3_Email1))
                                            bFound = true;
                                    }
                                }

                                if (!bFound)
                                {
                                    if (ct.Contact3_Email2 != null && ct.Contact3_Email2.Length > 0)
                                    {
                                        if (msgfrom.Contains(ct.Contact3_Email2))
                                            bFound = true;
                                    }
                                }
                                if (bFound)
                                {
                                    record.CustomerID = ct.ID;
                                }
                            }
                        }

                        string RID;
                        string today;
                        string type;
                        today = DateTime.Now.ToString("yyyyMMdd");
                        type = today + "3";
                        var curr_RID = db.Records.Where(d => d.RID.StartsWith(type) && !d.RID.Contains("_B")).Max(d => d.RID);

                        if (curr_RID != null)
                        {
                            RID = (Convert.ToInt64(curr_RID) + 1).ToString();
                        }
                        else
                        {
                            RID = type + "0001";
                        }
                        emailrecord.RID = RID;
                        db.Entry(emailrecord).State = EntityState.Modified;

                        record.RID = RID;
                        db.Records.Add(record);

                        db.SaveChanges();

                        return RedirectToAction("IndexQueue");
                    }
                    else
                    {
                        ModelState.AddModelError("CurrAgentId", "此郵件已由系統指派(案編:" + emailrecord.RID + ", 無法再次指派");
                    }
                }
                else
                {
                    return RedirectToAction("IndexQueue");
                }
            }
            else if (nId < 0)
            {
                var emailrecord = db.EmailRecord.Find(0 - nId);
                if (emailrecord != null)
                {
                    if (emailrecord.RID != null)
                    {
                        ModelState.AddModelError("CurrAgentId", "此郵件已被指派, 無法刪除");
                    }
                    else
                    {
                        var rawheaderid = emailrecord.RawHeaderId;

                        db.EmailRecord.Remove(emailrecord);

                        if (rawheaderid != null)
                        {
                            var rawheader = db.EmailRawHeader.Find(rawheaderid);
                            if (rawheader != null)
                            {
                                rawheader.Completed = 0;
                                db.Entry(rawheader).State = EntityState.Modified;
                            }
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("IndexQueue");
                }
            }
            else if (AgentID == null || AgentID.Length < 1)
            {
                ModelState.AddModelError("CurrAgentId", "請選擇客服人員");
            }
            else if (db.Agent.Find(AgentID).EmailIsLogin != 1)
            {
                ModelState.AddModelError("CurrAgentId", "此客服人員(" + db.Agent.Find(AgentID).AgentName + ")並未郵件值機");
            }

            ViewBag.Disabled = 0;
            return GetIndexQueueDetailView(Id.ToString());
        }

        //
        // GET: /EmailAdmin/DetailQueue
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult DetailQueue(string id)
        {
            ViewBag.Disabled = 1;
            return GetIndexQueueDetailView(id);
        }

        private ActionResult GetIndexQueueDetailView(string id)
        {
            EEmailRecord emailrecord = db.EmailRecord.Find(Convert.ToInt32(id));
            if (emailrecord == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> attachments = new List<SelectListItem>();
            string strBody = "";

            BuildUpEmailBodyAndAttachmentsQueue(id, ref strBody, ref attachments);

            emailrecord.EmailRawHeader.MsgReplyBody = strBody;

            #region 網頁物件準備
            ViewBag.Attachments = attachments;

            ViewBag.AgentsSuper = CommonUtilities.GetAgentList(db, userGroupId);
            #endregion

            return View("~/Areas/EmailSrv/Views/EmailAdmin/DetailQueue.cshtml", emailrecord);
        }

        private ERecords GetDetailObject(int EID)
        {
            EEmailRecord erecord = db.EmailRecord.Find(EID);            
            
            ERecords records = db.Records.Find(erecord.RID);

            List<SelectListItem> attachments = new List<SelectListItem>();
            string strBody = "", strattchmentlist = "", strErr = "";

            if (erecord.RawHeaderId != null && erecord.RawHeaderId.Value > 0)
            {
                BuildUpEmailBodyAndAttachmentsFromRawHeader(erecord, ref strBody, ref attachments, ref strattchmentlist, ref strErr);
                erecord.EmailRawHeader.MsgReplyBody = strBody;
            }
            else
            {
                BuildUpEmailBodyAndAttachmentsFromDraft(erecord, ref strBody, ref attachments, ref strattchmentlist, ref strErr);
                if (erecord.EmailRawHeader == null)
                    erecord.EmailRawHeader = new EEmailRawHeader();

                erecord.EmailRawHeader.MsgReplyBody = strBody.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\\n", "<br />");
            }            

            #region 網頁物件準備
            ViewBag.Attachments = attachments;

            SelectList ServiceMap = new SelectList(from s in db.RecordServiceMap where s.RID == records.RID select s, "GroupID", "ItemID");
            string strItemNames = "";
            foreach (var item in ServiceMap)
            {
                int nGroupID = Convert.ToInt32(item.Value);
                int nItemID = Convert.ToInt32(item.Text);
                strItemNames += "," + db.ServiceItem.FirstOrDefault(si => si.GroupID == nGroupID && si.ItemID == nItemID).ItemDesc;
            }
            if (strItemNames.Length > 0)
                strItemNames = strItemNames.Substring(1);

            ViewBag.ServiceItemNames = strItemNames;

            ViewBag.AgentsSuper = CommonUtilities.GetAgentList(db, userGroupId);
            #endregion

            return records;
        }

        //
        // GET: /EmailAdmin/Detail
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Detail(string id, string Source)
        {
            ViewBag.Source = Source;
            // Telephone Record
            if (id.StartsWith("T"))
            {
                ERecords records = db.Records.Find(id.Substring(1));
                return View(records);
            }
            else
            {
                EEmailRecord erecord = db.EmailRecord.Find(Convert.ToInt32(id));
                ERecords records = db.Records.Find(erecord.RID);
                if (erecord == null || records == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Disabled = 1;
                return View(GetDetailObject(Convert.ToInt32(id)));
            }
        }

        public ActionResult Download(int id = 0)
        {
            EEmailRawBody emailrawbody = db.EmailRawBody.Find(id);
            if (emailrawbody == null)
            {
                return HttpNotFound();
            }

            return File(emailrawbody.BinaryCnt, emailrawbody.CntMedia, emailrawbody.FileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //-------------------------------------------------------------------------------------
        // 重組郵件內容及其附件(未指派)
        private bool BuildUpEmailBodyAndAttachmentsQueue(string id, ref string strBody, ref List<SelectListItem> attachments)
        {
            bool bRet = true;
            try
            {
                EEmailRecord emailrecord = db.EmailRecord.Find(Convert.ToInt32(id));

                string curpath = Server.MapPath("~");
                if (curpath[curpath.Length - 1] == '\\')
                    curpath = curpath.Substring(0, curpath.Length - 1);

                string mailbox = String.Format("{0}\\inbox\\temp", curpath);

                // If the folder is not existed, create it.
                if (!Directory.Exists(mailbox))
                {
                    Directory.CreateDirectory(mailbox);
                }

                List<SelectListItem> cids = new List<SelectListItem>();
                attachments.Clear();
                string strCnt = "", strCntTmp = "";
                foreach (var item in from bodys in emailrecord.EmailRawHeader.EmailRawBody orderby bodys.PlaceHolder select bodys)
                {
                    if (item.FileName.Length < 1)
                    {
                        if (item.CntMedia.IndexOf("plain") >= 0)
                        {
                            if (strCntTmp.Length > 0)
                                strCnt += strCntTmp;

                            strCntTmp = System.Text.Encoding.GetEncoding(item.CodePage).GetString(item.BinaryCnt);
                            strCntTmp = strCntTmp.Replace("\r\n", "<br />").Replace("\n", "<br />");
                            continue;
                        }

                        strCnt += System.Text.Encoding.GetEncoding(item.CodePage).GetString(item.BinaryCnt);
                        strCntTmp = "";
                    }
                    /*else
                    {
                        if (!item.FileName.Contains('|'))
                        {
                            attachments.Add(new SelectListItem() { Text = item.FileName, Value = item.Id.ToString() });
                        }
                        else
                        {
                            string[] cid_filename = item.FileName.Split('|');

                            // 內嵌資訊不是圖片
                            if (!item.CntMedia.Contains("image"))
                            {
                                attachments.Add(new SelectListItem() { Text = cid_filename[1], Value = item.Id.ToString() });
                                continue;
                            }                                                       

                            if (cid_filename[1].Contains("(no name)"))
                            {
                                Random RGen = new Random();
                                string fname = RGen.Next(100000, 9999999).ToString();
                                cid_filename[1] = fname + "." + item.CntMedia.Substring(item.CntMedia.IndexOf("/") + 1);
                            }
                            if (!cid_filename[1].Contains("."))
                            {
                                cid_filename[1] += "." + item.CntMedia.Substring(item.CntMedia.IndexOf("/") + 1);
                            }

                            string attname = String.Format("{0}_{1}_{2}", item.RawHeaderId, cid_filename[0].Replace(':','-'), cid_filename[1]);

                            if (!System.IO.File.Exists(String.Format("{0}\\{1}", mailbox, attname)))
                            {
                                BinaryWriter writer = new BinaryWriter(new FileStream(String.Format("{0}\\{1}", mailbox, attname), FileMode.CreateNew, FileAccess.Write, FileShare.None));
                                writer.Write(item.BinaryCnt);
                                writer.Close();
                            }
                            cids.Add(new SelectListItem() { Text = cid_filename[0], Value = Url.Content(String.Format("~/inbox/temp/{0}", attname.Replace("\\", "/"))) });
                        }
                    }*/
                }

                if (strCntTmp.Length > 0)
                    strCnt += strCntTmp;

                strBody = strCnt;

                foreach (SelectListItem cidatt in cids)
                {
                    strBody = strBody.Replace("cid:" + cidatt.Text, cidatt.Value);
                }
            }
            catch (Exception ex)
            {
                strBody = ex.Message;
                attachments.Clear();
                bRet = false;
            }
            return bRet;
        }

        // 重組郵件內容及其附件(原始郵件)
        private bool BuildUpEmailBodyAndAttachmentsFromRawHeader(EEmailRecord erecord, ref string strBody, ref List<SelectListItem> attachments, ref string attachlist, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                EEmailRawHeader erawheader = erecord.EmailRawHeader;
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
        private bool BuildUpEmailBodyAndAttachmentsFromDraft(EEmailRecord erecord, ref string strBody, ref List<SelectListItem> attachments, ref string attachlist, ref string errMsg)
        {
            bool bRet = true;
            try
            {
                EEmailSavedBody esavedbody = erecord.EmailSavedBody;
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
                errMsg = ex.Message;
                attachments.Clear();
                bRet = false;
            }
            return bRet;
        }

        // 逾期處理流程
        private bool DeadLineProcess(IQueryable<ERecords> records)
        {
            //.Where(p => p.EmailRecord.Any(q => q.ExpireOn != null))
            foreach (var record in records.Where(r => r.TypeID == 3 && 
                                                 !r.RID.Contains("_B") && 
                                                 r.EmailRecord.Any(q => q.ExpireOn != null)))
            {                
                #region 逾期處理
                // "已結案"
                if (record.StatusID == 3)
                {
                    // 清空逾期期限
                    if (record.EmailRecord != null)
                    {
                        foreach (var er in record.EmailRecord.Where(e => e.ExpireOn != null))
                        {
                            er.ExpireOn = null;
                            db.Entry(er).State = EntityState.Modified;
                        }
                    }
                }
                // "未處理"
                else if (record.StatusID == 1)
                {
                    var ec = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault();
                    if (ec == null || ec.ExpireOn == null || ec.ExpireOn.Value > DateTime.Now)
                        continue;

                    foreach (var re in records.Where(r => r.RID.Contains(record.RID) && r.AgentID != null))
                    {
                        re.AgentID = null;
                        db.Entry(re).State = EntityState.Modified;
                    }
                }
                // "處理中"
                else if (record.StatusID == 2)
                {
                    if (record.AgentID == null || record.EAgent == null || record.EAgent.EmailIsLogin == 1)
                        continue;

                    // "未值機"
                    var ec = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault();
                    if (ec == null || (ec.ProcessStatus != null && ec.ProcessStatus.Length > 0) || ec.ExpireOn == null || ec.ExpireOn.Value > DateTime.Now)
                        continue;
                                        
                    foreach (var re in records.Where(r => r.RID.Contains(record.RID) && r.AgentID != null))
                    {
                        re.AgentID = null;
                        db.Entry(re).State = EntityState.Modified;
                    }
                }
                #endregion
            }

            db.SaveChanges();
            return true;
        }
        
        #region 讀取郵件信箱資訊
        public static string MailSender_Enterprise = System.Configuration.ConfigurationManager.AppSettings["MailSender_Enterprise"];

        public static string MailSender_Personal = System.Configuration.ConfigurationManager.AppSettings["MailSender_Personal"];
        #endregion
        
        // 群組處理流程
        private bool MailMemberProcessNormal(string userGroup, ref IQueryable<ERecords> records)
        {
            if (userGroup.ToUpper().Contains("SUPER"))
                return true;

            string strAllGroupID = "";
            foreach (var mg in db.MailGroup)
                strAllGroupID += "," + mg.GroupID;

            if (userGroup.ToUpper().Contains(strAllGroupID.ToUpper()))
                return true;

            records = records.Where(r => userGroup.ToUpper().Contains(r.GroupID.ToUpper()) || r.GroupID.ToUpper().Contains(userGroup.ToUpper()));
            return true;
        }

        // 群組處理流程 (原始郵件)
        private bool MailMemberProcessQueue(string userGroup, ref IQueryable<EEmailRecord> erecords)
        {
            if (userGroup.ToUpper().Contains("SUPER"))
                return true;

            var erecordlist = erecords.ToList();
            string MailGroupID = "";
            string MsgTo = "";

            List<int> EIDList = new List<int>();

            userGroup = userGroup.ToUpper();
            foreach (var erecord in erecordlist)
            {
                if (erecord.EmailRawHeader == null)
                    continue;

                if (erecord.EmailRawHeader.MsgTo != MsgTo)
                {
                    MsgTo = erecord.EmailRawHeader.MsgTo;
                    MailGroupID = CommonUtilities.GetMailGroupIDByMsgTo(db, MsgTo).ToUpper();
                }
                if (userGroup.Contains(MailGroupID) || MailGroupID.Contains(userGroup))
                    EIDList.Add(erecord.Id);
            }
            
            erecords = erecords.Where(r => EIDList.Contains(r.Id));

            return true;
        }

        // 逾期及群組處理流程
        /*private bool DeadLineAndMemberGroupProcess(string group, ref IQueryable<ERecords> records)
        {
            string ignorelist = "";
            foreach (var record in records)
            {               
                if (!record.RID.Contains("_B"))
                {
                    // "已結案"
                    if (record.StatusID == 3)
                    {
                        // 清空逾期期限
                        foreach (var er in record.EmailRecord)
                        {
                            er.ExpireOn = null;
                            db.Entry(er).State = EntityState.Modified;
                        }
                    }
                    // "未處理"
                    else if (record.StatusID == 1)
                    {
                        var ec = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault();
                        if (ec != null)
                        {
                            var expireon = ec.ExpireOn;

                            if (expireon != null && expireon.Value < DateTime.Now)
                            {
                                record.AgentID = null;
                                db.Entry(record).State = EntityState.Modified;
                            }
                        }
                    }
                    // "處理中"
                    else if (record.StatusID == 2)
                    {
                        // "未值機"
                        if (record.AgentID != null && record.AgentID.Length > 0)
                        {
                            var a = db.Agent.Find(record.AgentID);
                            if (a != null && a.EmailIsLogin == 0)
                            {
                                var ec = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault();
                                if (ec != null)
                                {

                                    var expireon = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault().ExpireOn;

                                    if (expireon != null && expireon.Value < DateTime.Now)
                                    {
                                        record.AgentID = null;
                                        db.Entry(record).State = EntityState.Modified;
                                    }
                                }
                            }
                        }
                    }
                }

                var emailrecord = record.EmailRecord.OrderByDescending(e => e.OrderNo).FirstOrDefault();
                if (emailrecord == null)
                    emailrecord = new EEmailRecord();
                if (record.EmailRecord.Count > 1)
                    emailrecord.IsReply = 1;
                else
                    emailrecord.IsReply = 0;

                if (!group.ToUpper().Contains("ENTERPRISE,PERSONAL") && !group.ToUpper().Contains("SUPER"))
                {
                    var agent = db.Agent.FirstOrDefault(a => a.AgentID == record.AgentID);
                    if (agent == null)
                    {
                        agent = db.Agent.FirstOrDefault(a => a.AgentID == emailrecord.CurrAgentId);
                    }

                    if (agent != null)
                    {
                        var mm = db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID);
                        if (mm != null)
                        {
                            bool bFound = false;
                            foreach (var m in mm)
                            {
                                if (group.ToUpper().Contains(m.GroupID.ToUpper()))
                                {
                                    bFound = true;
                                    break;
                                }
                            }

                            if (!bFound)
                            {
                                ignorelist += "|" + record.RID;
                            }
                        }
                    }
                    else
                    {
                        var mm = db.EmailRawHeader.Find(emailrecord.RawHeaderId);
                        if (mm != null)
                        {
                            string GroupMailServer = "mailserver98@juntai.com.tw";
                            if (group.ToUpper().Contains("PERSONAL"))
                                GroupMailServer = "mailserver99@juntai.com.tw";

                            if (!mm.MsgTo.ToUpper().Contains(GroupMailServer.ToUpper()))
                                ignorelist += "|" + record.RID;
                            
                        }
                    }
                }
            }

            db.SaveChanges();

            if (ignorelist.Length > 0)
            {
                ignorelist = ignorelist.Substring(1);
                string[] arrIgnore = ignorelist.Split('|');
                records = records.Where(r => !arrIgnore.Contains(r.RID));
            }
            return true;
        }*/

        // 查詢及排序
        private bool SearchAndSort(EmailAdmin_SearchCondition searchcondition, string orderby, ref IQueryable<ERecords> records, int Desc = 0)
        {
            #region 查詢
            if (searchcondition != null)
            {
                if (!String.IsNullOrEmpty(searchcondition.type) && !String.IsNullOrEmpty(searchcondition.condition))
                {
                    string condition = searchcondition.condition;
                    switch (searchcondition.type)
                    {
                        // 案件編號
                        case "3":
                            records = records.Where(e => condition.ToUpper().Contains(e.RID.ToUpper()));
                            break;
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
                if (!String.IsNullOrEmpty(searchcondition.Filter_Record_StatusID))
                {
                    string[] arrStatusIDs = searchcondition.Filter_Record_StatusID.Split(',');
                    int[] intStatusIDs = arrStatusIDs.Select(x => int.Parse(x)).ToArray();
                    records = records.Where(r => intStatusIDs.Contains(r.StatusID.Value));
                }

                if (!String.IsNullOrEmpty(searchcondition.Filter_Record_TypeID))
                {
                    string[] arrTypeIDs = searchcondition.Filter_Record_TypeID.Split(',');
                    int[] intTypeIDs = arrTypeIDs.Select(x => int.Parse(x)).ToArray();
                    records = records.Where(r => intTypeIDs.Contains(r.TypeID.Value));
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

                    // 案件類型
                    case "TypeID":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.TypeID) : records.OrderBy(r => r.TypeID);
                        break;

                    // 處理方式
                    case "CloseApproachID":
                        records = (Desc == 1) ? records.OrderByDescending(r => r.CloseApproachID) : records.OrderBy(r => r.CloseApproachID);
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

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult _PartialServiceMap(string id)
        {
            var serviceitems = from si in db.ServiceItem select si;

            string UserID = CommonUtilities.GetUserID(User);
            var agent = db.Agent.Find(UserID);
            if(agent != null)
            {
                List<int> servicegrouplist = new List<int>();
                var servicegroup = db.ServiceGroup.Where(sg => sg.DeptID == agent.DeptID);
                foreach (var sg in servicegroup)
                    servicegrouplist.Add(sg.GroupID);

                serviceitems = serviceitems.Where(si => servicegrouplist.Contains(si.GroupID));
            }
            
            var servicemap = db.RecordServiceMap.Where(rsm => rsm.RID == id);
            foreach (var si in serviceitems)
            {
                if (servicemap.FirstOrDefault(sm => sm.GroupID == si.GroupID && sm.ItemID == si.ItemID) != null)
                    si.Checked = 1;
                else
                    si.Checked = 0;

                si.GroupName = db.ServiceGroup.Find(si.GroupID).GroupDesc;
            }
            return PartialView("~/Areas/EmailSrv/Views/EmailAdmin/_PartialServiceMap.cshtml", serviceitems);
        }
    }
}