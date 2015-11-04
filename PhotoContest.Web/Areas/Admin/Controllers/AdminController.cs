using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PhotoContest.Web.Areas.Admin.Models;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : BaseAdminController
    {
        public AdminController() : this(new PhotoContestData())
        {
        }

        public AdminController(IPhotoContestData data)
            : base(data)
        {
        }
        // GET: Admin/Admin
        public ActionResult Users()
        {
            int id =  1;
            var users = this.Data.Users.All().OrderBy(u => u.JoinedAt).Select(u => new UserViewModel()
            {
                Id = u.Id,
                Username = u.UserName
            }).ToPagedList(id, 20);
            return View(users);
        }
    }
}