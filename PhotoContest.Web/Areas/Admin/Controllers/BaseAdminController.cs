using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BaseAdminController : Controller
    {
        protected BaseAdminController(IPhotoContestData data)
        {
            this.Data = data;
        }

        protected IPhotoContestData Data { get ; private set; }
    }
}