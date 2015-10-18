using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IPhotoContestData Data { get; private set; }

        public BaseController()
            : this(new PhotoContestData())
        {
        }

        public BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }
    }
}