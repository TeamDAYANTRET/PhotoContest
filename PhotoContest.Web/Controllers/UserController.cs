using PhotoContest.Data;
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
                .OrderByDescending(c => c.ParticipationEndTime)
                .Select(OngoingContestBasicInfoViewModel.Create).ToListAsync();
            var pastContests = await this.Data.Contests.All()
                .Where(c => c.OwnerId == userId && c.State != TypeOfEnding.Ongoing)
                .OrderByDescending(c => c.ContestEndTime)
                .Select(EndedContestBasicInfoViewModel.Create).ToListAsync();

            var contests = new IndexPageViewModel()
            {
                EndedContests = pastContests,
                OngoingContests = currentContests
            };

            ViewBag.msg = TempData["msg"];
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

        public JsonResult getUsers(string username)
        {
            var users = this.Data.Users.All().Where(t => t.UserName.Contains(username)).Select(t => t.UserName).ToList();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetAllVotesImagesByUser()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(userId);
            var votedImages = currentUser.VotedPictures.AsQueryable();
            var viewVotedImages= votedImages.Select(VotedImagesViewModel.Create);
            return View(viewVotedImages);
        }
    }
}