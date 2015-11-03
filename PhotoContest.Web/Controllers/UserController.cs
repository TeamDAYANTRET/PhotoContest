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
using PhotoContest.Web.Models.BindingModels;

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
            List<VotedImagesViewModel> viewVoted=new List<VotedImagesViewModel>();
            var userId = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(userId);
            var votedImages = currentUser.VotedPictures.OrderBy(x=> x.Contest.State).ToList();
            foreach (var image in votedImages)
            {
                viewVoted.Add(new VotedImagesViewModel
                {
                    ContestName = image.Contest.Title,
                    ImgPath = image.ImagePath,
                    EndTime = image.Contest.ParticipationEndTime,
                    Votes = image.Votes.Count,
                    State=image.Contest.State.ToString()

                });
            }
            return this.View(viewVoted);
        }
        [Authorize]
        public ActionResult AllOwnImages()
        {
            var userId = User.Identity.GetUserId();
            var ownImages =
                this.Data.Images.All()
                    .Where(x => x.UserId == userId)
                    .OrderBy(x => x.Contest.State)
                    .Select(OwnImageViewModel.Create)
                    .ToList();
            return this.View(ownImages);
        }
        [Authorize]
        public ActionResult Profile()
        {
            var userId = User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            var viewUser = new ProfileUserViewModel()
            {
                Id=user.Id,
                UserName = user.UserName,
                FullName = user.FirstName + " " + user.LastName,
                AboutMe = user.AboutMe,
                Joined = user.JoinedAt,
                ProfilePath = user.ProfilePic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OwnedImagesCount = user.Pictures.Count,
                ReceivedPrizesCount = user.Prizes.Count,
            };
            return this.View(viewUser);
        }

        public PartialViewResult EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            var userBinding = new EditUserProfileBindingModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                PhoneNumber = user.PhoneNumber,
                AvatarPath = user.ProfilePic
            };
            return PartialView("_EditProfile", userBinding);
        }

        public ActionResult SaveEditingUserProfile(EditUserProfileBindingModel userProfile)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Profile");
            }
            var loginUserId = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(loginUserId);
            if (userProfile.FirstName != null)
            {
                currentUser.FirstName = userProfile.FirstName;
            }
            if (userProfile.LastName != null)
            {
                currentUser.LastName = userProfile.LastName;
            }
            if (userProfile.AvatarPath != null)
            {
                currentUser.AboutMe = userProfile.AboutMe;
            }
            if (userProfile.PhoneNumber != null)
            {
                currentUser.PhoneNumber = userProfile.PhoneNumber;
            }
            if (userProfile.AvatarPath != null)
            {
                currentUser.ProfilePic = userProfile.AvatarPath;
            }
            this.Data.SaveChanges();
            return this.RedirectToAction("Profile");
        }
    }
}