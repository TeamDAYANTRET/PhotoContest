﻿using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoContest.Web.Models.ViewModels;
using System.Data.Entity;
using PhotoContest.Models.Enumerations;

namespace PhotoContest.Web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IPhotoContestData data)
            : base(data)
        {

        }

        public UserController()
            : this(new PhotoContestData())
        {

        }

        // GET: User/Contests
        [Authorize]
        public async Task<ActionResult> GetUserContests()
        {
            var userId = User.Identity.GetUserId();
            var currentContests = await this.Data.Contests.All()
                .Where(c => c.OwnerId == userId && c.State == TypeOfEnding.Ongoing)
                .OrderByDescending(c => c.ContestEndTime)
                .ThenByDescending(c => c.ParticipationEndTime)
                .Select(ContestBasicInfoViewModel.Create).ToListAsync();
            var pastContests = await this.Data.Contests.All()
                .Where(c => c.OwnerId == userId && c.State != TypeOfEnding.Ongoing)
                .OrderByDescending(c => c.ContestEndTime)
                .ThenByDescending(c => c.ParticipationEndTime)
                .Select(ContestBasicInfoViewModel.Create).ToListAsync();

            var contests = new IndexPageViewModel()
            {
                EndedContests = pastContests,
                OngoingContests = currentContests
            };

            return View("Contests", contests);
        }

        [AllowAnonymous]
        public JsonResult isEmailExist(string email)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.Email == email);
            return Json(user == null);
        }

        [AllowAnonymous]
        public JsonResult isUsernameExist(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            return Json(user == null);
        }
    }
}