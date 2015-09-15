using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TTCS.Areas.EmailSrv.Models;

namespace TTCS.Areas.EmailSrv.Controllers
{
    public class CommonUtilities
    {
        public static int GetPageSize(EmailSrvEntities db)
        {
            int nPageSize = 10;
            var pagesize = db.EmailScheduleSetting.Where(ess => ess.Name == "PageSize");
            if (pagesize == null || pagesize.Count() < 1)
            {
                EEmailScheduleSetting ess = new EEmailScheduleSetting();
                ess.Name = "PageSize";
                ess.Value = "10";
                ess.Period = 1;
                db.EmailScheduleSetting.Add(ess);
                db.SaveChanges();
            }
            else
            {
                bool bOK = false;
                foreach(var ess in pagesize)
                {
                    if (bOK)
                    {
                        db.EmailScheduleSetting.Remove(ess);
                    }
                    else
                    {   
                        try
                        {
                            nPageSize = Convert.ToInt32(ess.Value);
                            bOK = true;
                        }
                        catch
                        {
                            db.EmailScheduleSetting.Remove(ess);
                        }
                    }
                }
                if (!bOK)
                {
                    EEmailScheduleSetting ess = new EEmailScheduleSetting();
                    ess.Name = "PageSize";
                    ess.Value = "10";
                    ess.Period = 1;
                    db.EmailScheduleSetting.Add(ess);
                }
                db.SaveChanges();
            }
            return nPageSize;
        }

        public static string GetUserGrupID(System.Security.Principal.IPrincipal User)
        {
            try
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                return ticket.UserData;
            }
            catch
            {
                return "";
            }
        }

        public static string GetUserID(System.Security.Principal.IPrincipal User)
        {
            try
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                return ticket.Name.Split('|')[1];
            }
            catch
            {
                return "";
            }
        }

        public static string GetMailGroupIDByMsgTo(EmailSrvEntities db, string MsgTo)
        {
            string strRet = "";

            var Group = db.MailGroup.FirstOrDefault(mg => mg.Mailbox == MsgTo);
            if (Group != null && Group.GroupID != null && Group.GroupID.Length > 0)
            {
                strRet = Group.GroupID;
            }

            return strRet;
        }

        public static List<SelectListItem> GetAgentList(EmailSrvEntities db, string userGroupID)
        {
            List<SelectListItem> agents = new List<SelectListItem>();
            Dictionary<string, string> MailMember = new Dictionary<string, string>();
            foreach (var group in db.MailGroup)
            {
                if (!MailMember.Keys.Contains(group.GroupID))
                    MailMember.Add(group.GroupID, "");
            }

            foreach (var member in db.MailMember)
            {
                MailMember[member.GroupID] += "|" + member.CTILoginID;
            }

            if (userGroupID.ToUpper().Contains("SUPER"))
            {
                userGroupID = "";
                foreach (var group in db.MailGroup)
                    userGroupID += "," + group.GroupID;
            }

            string[] arrUserGroup = userGroupID.StartsWith(",") ? userGroupID.Substring(1).Split(',') : userGroupID.Split(',');

            if (userGroupID.ToUpper().Contains("SUPER"))
            {
                
            }

            var agent = from a in db.Agent select a;
            for (int n = 0; n < arrUserGroup.Length; n++)
            {
                string[] arrAgent = MailMember[arrUserGroup[n]].Split('|');
                var agent1 = agent.Where(a => arrAgent.Contains(a.CTILoginID));

                SelectList agentMember = new SelectList(agent1, "AgentID", "AgentName");

                string text = "", value = arrUserGroup[n];
                foreach (var a in agentMember)
                {
                    text += "|" + a.Value + ":" + a.Text;
                }
                if (text.Length > 0)
                    text = text.Substring(1);

                agents.Add(new SelectListItem() { Text = text, Value = value });
            }
            return agents;
        }
    }
}
