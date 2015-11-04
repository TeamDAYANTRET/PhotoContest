﻿using System.Web.Mvc;

namespace PhotoContest.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Notifications", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "PhotoContest.Web.Areas.Admin.Controllers" }
            );
        }
    }
}