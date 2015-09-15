using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCS.Areas.EmailSrv.Models;
using System.Data.Entity;
using System.Data;

using PagedList;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Web.Security;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class SystemSettingController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();

        private int pageSize = 5;

        private SystemSetting GetIndexModel(string type = "", string condition = "", int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            SystemSetting systemsetting = new SystemSetting();
            #region 排程設定
            systemsetting.EmailScheduleSetting = db.EmailScheduleSetting.Where(s =>s.Name=="Base").FirstOrDefault() ?? new EEmailScheduleSetting();
            
            int value = 0, fre = 0;
            bool bIsEnable = true;
            GetSQLJobMinute(ref value, ref fre, ref bIsEnable);

            systemsetting.EmailScheduleSetting.Name= "Base";
            systemsetting.EmailScheduleSetting.Period = value;
            systemsetting.EmailScheduleSetting.Frequency = fre;
            systemsetting.EmailScheduleSetting.Active = bIsEnable;

            if (value == 0)
                systemsetting.EmailScheduleSetting.Value = "0";

            List<SelectListItem> frequency = new List<SelectListItem>();
            frequency.Add(new SelectListItem() { Text = "小時", Value = "0", Selected=fre.Equals(0)});
            frequency.Add(new SelectListItem() { Text = "分鐘", Value = "1", Selected=fre.Equals(1)});

            ViewBag.Frequency = frequency;

            #endregion

            #region 退回原因
            systemsetting.EmailRejectReason = db.EmailRejectReason;
            systemsetting.EmailRejectReason = systemsetting.EmailRejectReason.OrderBy(c => c.Id);
            
            #endregion

            #region 處理方式
            systemsetting.RecordCloseApproach = db.RecordCloseApproach;
            systemsetting.RecordCloseApproach = systemsetting.RecordCloseApproach.OrderBy(c => c.ApproachID);

            #endregion

            // 退回原因
            if (type.Equals("0"))
            {
                if (!String.IsNullOrEmpty(condition) && !String.IsNullOrEmpty(condition))
                {
                    systemsetting.EmailRejectReason = systemsetting.EmailRejectReason.Where(c => (c.Reason.ToUpper().Contains(condition.ToUpper())));
                }
                                
                systemsetting.EmailRejectReason = systemsetting.EmailRejectReason.ToPagedList(currentPage, pageSize);
                ViewBag.ConditionRejectReason = condition;
                ViewBag.NumberBeginRejectReason = pageSize * (page - 1);

                systemsetting.RecordCloseApproach = systemsetting.RecordCloseApproach.ToPagedList(1, pageSize);
                ViewBag.ConditionCloseApproach = "";
                ViewBag.NumberBeginCloseApproach = 0;

            }
            else
            {
                systemsetting.EmailRejectReason = systemsetting.EmailRejectReason.ToPagedList(1, pageSize);
                ViewBag.ConditionRejectReason = "";
                ViewBag.NumberBeginRejectReason = 0;

                systemsetting.RecordCloseApproach = systemsetting.RecordCloseApproach.ToPagedList(currentPage, pageSize);
                ViewBag.ConditionCloseApproach = condition;
                ViewBag.NumberBeginCloseApproach = pageSize * (page - 1);
            }

            var expire = db.EmailScheduleSetting.Where(s => s.Name == "ExpireLen").FirstOrDefault();
            if (expire == null)
                ViewBag.ExpireLen = "";
            else
                ViewBag.ExpireLen = expire.Value;

            return systemsetting;
        }

        //
        // GET: /EmailSrv/SystemSetting/Index
        public ActionResult Index(string type="", string condition="", int page = 1)
        {
            return View(GetIndexModel(type,condition,page));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleSetting(EEmailScheduleSetting emailschedulesetting)
        {
            int OK = 1;

            if (ModelState.IsValid)
            {
                string strErr = "";

                if (OK > 0)
                {
                    OK = 0;

                    if (emailschedulesetting.Id < 1)
                    {

                        if (UpdateSQLJob(emailschedulesetting.Period, emailschedulesetting.Active, emailschedulesetting.Frequency, ref strErr))
                        {
                            db.EmailScheduleSetting.Add(emailschedulesetting);
                            OK = 1;
                        }
                    }
                    else
                    {
                        if (UpdateSQLJob(emailschedulesetting.Period, emailschedulesetting.Active, emailschedulesetting.Frequency, ref strErr))
                        {
                            db.Entry(emailschedulesetting).State = EntityState.Modified;
                            OK = 1;
                        }
                    }
                }

                if (OK == 1)
                {
                    db.SaveChanges();
                    ViewBag.ScheduleTaskResult = "（儲存成功）";
                }
                else if(OK == 0)
                {
                    ViewBag.ScheduleTaskResult = String.Format("（儲存失敗：{0}）", strErr);
                }

                return View("Index",GetIndexModel());
            }
            else
            {
                ModelState.AddModelError("", "請輸入任一條件值!!");
            }
            return RedirectToAction("Index");
        }

        private bool GetSQLJobMinute(ref int nValue, ref int nFrequency, ref bool bEnabled)
        {
            ServerConnection conn = null;
            try
            {
                string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["SQLJob"].ConnectionString;

                System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection(connstring);
                conn = new ServerConnection(sqlconn);

                Server server = new Server(conn);

                // Get instance of SQL Agent SMO object 
                JobServer jobServer = server.JobServer;
                Job job = jobServer.Jobs["EmailDispatch"];

                if (job == null)
                {
                    nValue = 0; nFrequency = 0;
                    return true;
                }

                JobStep step = job.JobSteps["EmailDispatchProcess"];
                if (step == null)
                {
                    nValue = 0; nFrequency = 0;
                    return true;
                }

                JobSchedule schedule = job.JobSchedules["MinuteDispatch"];
                if (schedule == null)
                {
                    nValue = 0; nFrequency = 0;
                    return true;
                };

                nValue = schedule.FrequencySubDayInterval;

                if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Hour)
                    nFrequency = 0;
                else if (schedule.FrequencySubDayTypes == FrequencySubDayTypes.Minute)
                    nFrequency = 1;

                bEnabled = job.IsEnabled;
                conn.Disconnect();

                return true;
            }
            catch 
            {
                if(conn != null)
                    conn.Disconnect();
                nValue = -1; nFrequency = 0; bEnabled = false;
                return false;
            }
        }

        private bool UpdateSQLJob(int nPeriod, bool bEnable, int nFreq, ref string err)
        {
            ServerConnection conn = null;
            try
            {
                string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["SQLJob"].ConnectionString;

                System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection(connstring);
                conn = new ServerConnection(sqlconn);

                Server server = new Server(conn);

                // Get instance of SQL Agent SMO object 
                JobServer jobServer = server.JobServer;
                Job job = jobServer.Jobs["EmailDispatch"];

                if (job == null)
                {
                    // Create Job 
                    job = new Job(jobServer, "EmailDispatch");
                    job.Create();
                    //job.AddSharedSchedule(schedule.ID);
                    //job.ApplyToTargetServer(server.Name);
                }

                JobStep step = job.JobSteps["EmailDispatchProcess"];
                if (step == null)
                {
                    // Create JobStep 
                    step = new JobStep(job, "EmailDispatchProcess");
                    step.Command = "EXEC P_EmailDispatchForSchedule";
                    step.SubSystem = AgentSubSystem.TransactSql;
                    step.DatabaseName = conn.DatabaseName;

                    step.Create();
                }

                JobSchedule schedule = job.JobSchedules["MinuteDispatch"];
                if (schedule == null)
                {
                    // Create a schedule 
                    schedule = new JobSchedule(job, "MinuteDispatch");
                    schedule.ActiveStartDate = DateTime.Today;
                    schedule.FrequencyTypes = FrequencyTypes.Daily;
                    schedule.FrequencyInterval = 1;
                    if (nFreq == 0)
                        schedule.FrequencySubDayTypes = FrequencySubDayTypes.Hour;
                    else if(nFreq == 1)
                        schedule.FrequencySubDayTypes = FrequencySubDayTypes.Minute;

                    schedule.FrequencySubDayInterval = nPeriod;
                    //schedule.ActiveStartTimeOfDay = new TimeSpan(DateTime.Now.Hour, (DateTime.Now.Minute + 2), 0);
                    schedule.Create();

                    job.AddSharedSchedule(schedule.ID);
                    job.ApplyToTargetServer("(local)");

                }
                else
                {
                    if (nFreq == 0)
                        schedule.FrequencySubDayTypes = FrequencySubDayTypes.Hour;
                    else if (nFreq == 1)
                        schedule.FrequencySubDayTypes = FrequencySubDayTypes.Minute;

                    schedule.FrequencySubDayInterval = nPeriod;

                    schedule.Alter();
                }

                job.IsEnabled = bEnable;
                job.Alter();

                conn.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if(conn != null)
                    conn.Disconnect();
                return false;
            }
        }
        
        public ActionResult _PartialOpenPopupReject(int id = 0)
        {
            var emailrejectreason = db.EmailRejectReason.Find(id);

            if (emailrejectreason != null)
            {
                return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupReject.cshtml", emailrejectreason);
            }
            else
            {
                emailrejectreason = new EEmailRejectReason();
                emailrejectreason.Id = id;
                emailrejectreason.Reason = "";

                return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupReject.cshtml", emailrejectreason);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialOpenPopupReject(EEmailRejectReason emailrejectreason, int? deleteId)
        {
            if (ModelState.IsValid)
            {
                if (deleteId != null && deleteId > 0)
                {
                    emailrejectreason = db.EmailRejectReason.Find(emailrejectreason.Id);
                    db.EmailRejectReason.Remove(emailrejectreason);
                }
                else
                {
                    if (emailrejectreason.Id == 0)
                    {
                        db.EmailRejectReason.Add(emailrejectreason);
                    }
                    else
                    {
                        db.Entry(emailrejectreason).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "SystemSetting", new {type="0", Area = "EmailSrv" });
        }

        public ActionResult _PartialOpenPopupApproach(int id = 0)
        {
            var recordcloseapproach = db.RecordCloseApproach.Find(id);

            if (recordcloseapproach != null)
            {
                return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupApproach.cshtml", recordcloseapproach);
            }
            else
            {
                recordcloseapproach = new ERecordCloseApproach();
                recordcloseapproach.ApproachID = id;
                recordcloseapproach.ApproachName = "";

                return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupApproach.cshtml", recordcloseapproach);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _PartialOpenPopupApproach(ERecordCloseApproach recordcloseapproach, int? deleteApproachId)
        {
            if (ModelState.IsValid)
            {
                if (deleteApproachId != null && deleteApproachId > 0)
                {
                    recordcloseapproach = db.RecordCloseApproach.Find(recordcloseapproach.ApproachID);
                    db.RecordCloseApproach.Remove(recordcloseapproach);
                }
                else
                {
                    if (recordcloseapproach.ApproachID == 0)
                    {
                        db.RecordCloseApproach.Add(recordcloseapproach);
                    }
                    else
                    {
                        db.Entry(recordcloseapproach).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "SystemSetting", new { type="1", Area = "EmailSrv" });
        }
        
        public ActionResult _PartialOpenPopupAgentWeight()
        {
            List<string> AgentList = new List<string>();
            foreach (var a in db.Agent)
                AgentList.Add(a.AgentID);

            bool bModifed = false;
            foreach (var aw in db.EmailAgentWeight)
            {
                if (!AgentList.Contains(aw.AgentId))
                {
                    bModifed = true;
                    db.EmailAgentWeight.Remove(aw);
                }
            }

            foreach (var aid in AgentList)
            {
                if (db.EmailAgentWeight.Where(aw => aw.AgentId == aid).FirstOrDefault() == null)
                {
                    bModifed = true;
                    var eaw = new EEmailAgentWeight();
                    eaw.AgentId = aid;
                    eaw.Weight = 1;
                    db.EmailAgentWeight.Add(eaw);
                }
            }

            if (bModifed)
                db.SaveChanges();
                        
            string groupID = CommonUtilities.GetUserGrupID(User).ToUpper();
            List<string> memberList = new List<string>();
            IEnumerable<TTCS.Areas.EmailSrv.Models.EEmailAgentWeight> emailagentweight = db.EmailAgentWeight.ToList();
            foreach (var emailaw in emailagentweight)
            {
                var agent = db.Agent.Find(emailaw.AgentId);
                if(agent == null)
                    continue;
                
                emailaw.AgentName = db.Agent.Find(emailaw.AgentId).AgentName;
                if(groupID.Contains("SUPER"))
                {
                    memberList.Add(emailaw.AgentId);
                    continue;
                }

                foreach(var mm in db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID))
                {
                    if(groupID.Contains(mm.GroupID.ToUpper()) || mm.GroupID.ToUpper().Contains(groupID))
                    {
                        memberList.Add(emailaw.AgentId);
                        break;
                    }
                }
            }
            emailagentweight = emailagentweight.Where(aw => memberList.Contains(aw.AgentId));
            return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupAgentWeight.cshtml", emailagentweight);

        }

        public ActionResult _PartialOpenPopupAgentWeightDetail(string id)
        {
            var emailagentweight = db.EmailAgentWeight.Find(id);
            emailagentweight.AgentName = db.Agent.Find(emailagentweight.AgentId).AgentName;

            return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupAgentWeightDetail.cshtml", emailagentweight);

        }

        [HttpPost]
        public ActionResult _PartialOpenPopupAgentWeightDetail(string id, string weight)
        {
            var emailagentweight = db.EmailAgentWeight.Find(id);
            if(emailagentweight != null)
            {
                emailagentweight.Weight = Convert.ToDouble(weight);
                db.Entry(emailagentweight).State = EntityState.Modified;
                db.SaveChanges();
            }

            var emailagentweights = db.EmailAgentWeight;
            foreach (var emailaw in emailagentweights)
            {
                emailaw.AgentName = db.Agent.Find(emailaw.AgentId).AgentName;
            }
            return PartialView("~/Areas/EmailSrv/Views/SystemSetting/_PartialOpenPopupAgentWeight.cshtml", emailagentweights.ToList());
        }

        public ActionResult _AjaxUpdateExpireLen(string expirelen)
        {
            try
            {
                var emailschedulesetting = db.EmailScheduleSetting.Where(s => s.Name == "ExpireLen").FirstOrDefault();
                if (emailschedulesetting == null)
                {
                    emailschedulesetting = new EEmailScheduleSetting();
                    emailschedulesetting.Name = "ExpireLen";
                    emailschedulesetting.Value = expirelen;
                    emailschedulesetting.Period = 1;
                    db.EmailScheduleSetting.Add(emailschedulesetting);
                }
                else
                {
                    emailschedulesetting.Value = expirelen;
                    emailschedulesetting.Period = 1;
                    db.Entry(emailschedulesetting).State = EntityState.Modified;

                }
                db.SaveChanges();

                var ret = new
                {
                    result = 1,
                    err = ""
                };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ret = new
                {
                    result = 0,
                    err = ex.Message
                };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
