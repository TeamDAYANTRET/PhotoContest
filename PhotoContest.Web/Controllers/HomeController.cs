using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PhotoContest.Data;
using PhotoContest.Data.Contracts;

namespace PhotoContest.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {
        }

        public HomeController(IPhotoContestData ctx) : base(ctx)
        {
        }
        public ActionResult Index()
        {
            var start = this.Data.Comments.All().Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}