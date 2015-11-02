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
                new { action = "Index", id = UrlParameter.Optional },
                new { controller = "Notifications|AdminHome" },
                new[] { "PhotoContest.Web.Areas.Admin.Controllers" }
            );
        }
    }
}