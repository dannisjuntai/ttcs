using System.Web.Mvc;

namespace TTCS.Areas.EmailSrv
{
    public class EmailSrvAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EmailSrv";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EmailSrv_default",
                "EmailSrv/{controller}/{action}/{id}",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "TTCS.Areas.EmailSrv.Controllers" }
            );
        }
    }
}
