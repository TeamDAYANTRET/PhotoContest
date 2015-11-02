using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        //[Authorize(Roles = "Admin")]
        private readonly IPhotoContestData data;

        protected BaseAdminController(IPhotoContestData data)
        {
            this.data = data;
        }

        protected IPhotoContestData Data { get; private set; }
    }
}