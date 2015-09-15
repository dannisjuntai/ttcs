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
    public class LogController : Controller
    {
        private kskyEntities db = new kskyEntities();
        //
        // GET: /Log/

        public ActionResult Add(int id)
        {
            int flag_success = 0;
            string ex_msg = "";
            string tmp_level = Request.Form["level"];
            string tmp_msg = Request.Form["message"];
            string tmp_logger = Request.Form["logger"];

            CTIAgentLog log = new CTIAgentLog();
            log.AgentId = id.ToString();
            log.Level = (string.IsNullOrWhiteSpace(tmp_level) ? "Error": tmp_level);
            log.Message = string.Format("[{0}]: {1} ",
                (string.IsNullOrWhiteSpace(tmp_logger) ? "None" : tmp_logger), (string.IsNullOrWhiteSpace(tmp_msg) ? "Error" : tmp_msg));
            log.LogDT = DateTime.Now;
            try{
                if (ModelState.IsValid)
                {
                    db.CTIAgentLog.Add(log);
                    db.SaveChanges();
                    flag_success = 0;
                }
                else
                {
                    ex_msg = Helpers.Error.GetModelCheckString(ModelState.Values);
                    flag_success = -2;
                }
            }
            catch (Exception ex)
            {
                ex_msg = Helpers.Error.FetchExceptionMessage(ex);
                flag_success = -1;
            }

            var ret = new {
                ret = flag_success,
                message = ex_msg
            };
            return Content(JsonConvert.SerializeObject(ret), "application/json");
        }

    }
}
