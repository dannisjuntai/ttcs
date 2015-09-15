using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using TTCS.Areas.EmailSrv.Models;
namespace TTCS.Areas.EmailSrv.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private EmailSrvEntities db = new EmailSrvEntities();

        //
        // GET: /EmailSrv/Account/
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var agent = db.Agent.FirstOrDefault(a => a.CTILoginID == loginModel.UserName);
            if (agent == null)
            {
                ModelState.AddModelError("UserName", "請輸入正確的帳號!");
                return View(loginModel);
            }

            var Password = loginModel.Password;
            var isRemeber = false;
            ViewBag.pwd1 = agent.AgentPWD;
            ViewBag.pwd2 = loginModel.Password;
            if (agent.CTILoginPWD.Equals(Password))
            {
                if (agent.Authority == 0)
                {
                    ModelState.AddModelError("UserName", "此帳號權限不足!!");
                    return View(loginModel);
                }

                var mailmember = db.MailMember.Where(m => m.CTILoginID == agent.CTILoginID);
                string group = "";
                if (agent.Authority == 2)
                {
                    group = "super";
                }
                else
                {
                    if (mailmember != null)
                    {
                        foreach (var mm in mailmember)
                            group += "," + mm.GroupID;
                    }
                }

                var ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: agent.AgentName + "|" + agent.AgentID,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddMinutes(30),
                    isPersistent: isRemeber,
                    userData: group,
                    cookiePath: FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "EmailAdmin", new { area = "EmailSrv" });
            }
            else
            {
                ModelState.AddModelError("Password", "請輸入正確的密碼!");
                return View(loginModel);
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            //清除所有的 session            
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie            
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋            
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login", "Account");
        }
    }
}
