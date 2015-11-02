using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoContest.Models;

namespace PhotoContest.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IPhotoContestData Data { get; private set; }
        protected ApplicationUser UserProfile { get; private set; }

        public BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }
        protected BaseController(IPhotoContestData data, ApplicationUser userProfile)
            : this(data)
        {
            this.UserProfile = userProfile;
        }
    }
}