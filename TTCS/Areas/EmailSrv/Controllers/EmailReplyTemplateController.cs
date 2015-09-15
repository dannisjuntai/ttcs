using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;

using TTCS.Areas.EmailSrv.Models;
using PagedList;
using System.Web.Security;
using System.IO;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class EmailReplyTemplateController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();

        private int pageSize = 10;

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        private EmailReplyTemplate GetIndexModel(string type = "", string condition = "", int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;

            EmailReplyTemplate emailreplytemp = new EmailReplyTemplate();

            #region 檢查簽名檔, 若沒有, 則新增
            bool bAddNew = false;
            if (db.EmailReplyCan.Find(1) == null)
            {
                EEmailReplyCan emailreplycan = new EEmailReplyCan();
                emailreplycan.Id = 1;
                emailreplycan.Name = "個人版簽名檔";
                emailreplycan.Active = true;
                emailreplycan.TempCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes("");
                db.EmailReplyCan.Add(emailreplycan);

                bAddNew = true;
            }

            if (db.EmailReplyCan.Find(2) == null)
            {
                EEmailReplyCan emailreplycan = new EEmailReplyCan();
                emailreplycan.Id = 1;
                emailreplycan.Name = "企業版簽名檔";
                emailreplycan.Active = true;
                emailreplycan.TempCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes("");
                db.EmailReplyCan.Add(emailreplycan);

                bAddNew = true;
            }

            if (bAddNew)
                db.SaveChanges();

            #endregion

            emailreplytemp.EmailReplyCan = db.EmailReplyCan.OrderByDescending(c => c.Id);

            #region 查詢
            if (!String.IsNullOrEmpty(condition) && !String.IsNullOrEmpty(condition))
            {
                emailreplytemp.EmailReplyCan = emailreplytemp.EmailReplyCan.Where(c => (c.Name.ToUpper().Contains(condition.ToUpper())));
            }
            #endregion

            // 抓取需求頁碼的資料
            emailreplytemp.EmailReplyCan = emailreplytemp.EmailReplyCan.ToPagedList(currentPage, pageSize);

            #region 回覆範本內容解碼
            foreach (var rc in emailreplytemp.EmailReplyCan)
            {               
                rc.TmpContent = System.Text.Encoding.GetEncoding("utf-8").GetString(rc.TempCnt);
                rc.TempCnt = null;
            }

            #endregion

            ViewBag.ConditionReplyCan = condition;
            ViewBag.NumberBeginReplyCan = pageSize * (page - 1);

            return emailreplytemp;
        }

        //
        // GET: /EmailSrv/EmailReplyTemplate/
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(string type = "", string condition = "", int page = 1)
        {
            return View(GetIndexModel(type, condition, page));
        }

        public ActionResult _PartialOpenPopupReply(int id = 0)
        {
            var emailreplycan = db.EmailReplyCan.Find(id);

            if (emailreplycan != null)
            {
                emailreplycan.TmpContent = System.Text.Encoding.GetEncoding("utf-8").GetString(emailreplycan.TempCnt);
                emailreplycan.TempCnt = null;
                return PartialView("~/Areas/EmailSrv/Views/EmailReplyTemplate/_PartialOpenPopupReply.cshtml", emailreplycan);
            }
            else
            {
                emailreplycan = new EEmailReplyCan();
                emailreplycan.Id = id;
                emailreplycan.Name = "";
                emailreplycan.TmpContent = "";
                emailreplycan.Active = true;

                return PartialView("~/Areas/EmailSrv/Views/EmailReplyTemplate/_PartialOpenPopupReply.cshtml", emailreplycan);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _PartialOpenPopupReply(EEmailReplyCan emailreplycan, int? deleteReply)
        {
            if (ModelState.IsValid)
            {
                if (deleteReply != null && deleteReply > 0)
                {
                    emailreplycan = db.EmailReplyCan.Find(emailreplycan.Id);
                    db.EmailReplyCan.Remove(emailreplycan);
                }
                else
                {
                    emailreplycan.TempCnt = System.Text.Encoding.GetEncoding("utf-8").GetBytes(emailreplycan.TmpContent);

                    if (emailreplycan.Id == 0)
                    {
                        db.EmailReplyCan.Add(emailreplycan);
                    }
                    else
                    {
                        if (emailreplycan.Id == 1 || emailreplycan.Id == 2)
                            emailreplycan.Active = true;

                        db.Entry(emailreplycan).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "EmailReplyTemplate", new { Area = "EmailSrv" });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public JsonResult _VerifyReplyTemplateName(string name, int Id)
        {            
            var result = (db.EmailReplyCan.Where(rc => rc.Name == name && rc.Id != Id).FirstOrDefault() == null);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _PartialTemplateVariable()
        {
            #region 範本變數
            var variableAgent = db.EmailScheduleSetting.Where(s => s.Name == "AgentID").FirstOrDefault();
            TemplateVariables templatevariables = new TemplateVariables();
            templatevariables.AgentID = (variableAgent == null) ? "" : variableAgent.Value;
            #endregion

            return PartialView("~/Areas/EmailSrv/Views/EmailReplyTemplate/_PartialTemplateVariable.cshtml", templatevariables);
        }

        [HttpPost]
        public ActionResult _PartialTemplateVariable(TemplateVariables templatevariables)
        {
            var agentVar = db.EmailScheduleSetting.Where(s => s.Name == "AgentID").FirstOrDefault();
            if (agentVar == null)
            {
                var emailsetting = new EEmailScheduleSetting();
                emailsetting.Name = "AgentID";
                emailsetting.Value = templatevariables.AgentID;
                emailsetting.Period = 1;
                db.EmailScheduleSetting.Add(emailsetting);
            }
            else
            {
                agentVar.Value = templatevariables.AgentID;
                agentVar.Period = 1;

                db.Entry(agentVar).State = EntityState.Modified;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
