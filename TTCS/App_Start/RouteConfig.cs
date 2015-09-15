using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TTCS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("Content/");

            routes.MapRoute(
                name: "Record-AjaxJsonList",
                url: "Record/AjaxJsonList/{agent_id}/",
                defaults: new { controller = "Record", action = "AjaxJsonList"}
            );

            //routes.MapRoute(
            //    name: "Customer-AjaxIncomingSearch",
            //    url: "Customer/AjaxIncomingSearch/{number}/{isEnterprise}",
            //    defaults: new { controller = "Customer", action = "AjaxIncomingSearch"}
            //);

            routes.MapRoute(
                name: "TwoPara",
                url: "Record/AjaxInfoList/{customer_id}/{limited}",
                defaults: new { controller = "Record", action = "AjaxInfoList", customer_id = UrlParameter.Optional, limited = 10 }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Record", action = "Show", id = UrlParameter.Optional },
                namespaces: new[] { "TTCS.Controllers" }
            );
        }
    }
}