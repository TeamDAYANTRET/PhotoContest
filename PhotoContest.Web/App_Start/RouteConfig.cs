using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoContest.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Paging",
                url: "{controller}/{action}/{id}/page/{page}",
                defaults: new { controller = "Contest", action = "Index", id = UrlParameter.Optional, page = UrlParameter.Optional },
                namespaces: new string[] { "PhotoContest.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RemoveMember",
                url: "{controller}/{id}/{action}/{userId}",
                defaults: new { controller = "Contest", action = "Index" },
                namespaces: new string[] { "PhotoContest.Web.Controllers" }
            );

            routes.MapRoute(
             name: "NotificationsRoute",
             url: "I/Notifications",
             defaults: new { controller = "Notifications", action = "Index" },
             namespaces: new string[] { "PhotoContest.Web.Controllers" }
          ); 

             routes.MapRoute(
             name: "AdminNotifications",
             url: "Admin/Notifications",
             defaults: new { controller = "Notifications", action = "Index" },
             namespaces: new string[] { "PhotoContest.Web.Areas.Admin.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Contest", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "PhotoContest.Web.Controllers" }
            );
        }
    }
}
