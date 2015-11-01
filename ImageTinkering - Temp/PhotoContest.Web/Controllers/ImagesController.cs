using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Controllers
{
    using Microsoft.AspNet.Identity;

    using System.IO;
    using System.Web.Mvc;
    using System.Threading.Tasks;

    using Models.BindingModels;
    using PhotoContest.Data.Contracts;
    using App_Start;
    using PhotoContest.Models.Enumerations;
    using PhotoContest.Models;
    using System.Text.RegularExpressions;

    [Authorize]
    public class ImagesController : BaseController
    {
        public ImagesController(IPhotoContestData data) : base(data)
        {
        }

        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadImage(UploadImageBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                return this.Content("Wrong input.");//this.View();
            }

            var userId = User.Identity.GetUserId();
            var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == model.ContestId);

            if (!this.rightToParticipate(contest, userId))
            {
                TempData["contestClosed"] = "You have no right to participate in this contest.";
                return this.Redirect("/Home/Index");//Will be changed
            }

            var fileExtension = Path.GetExtension(model.PhotoFile.FileName);
            var rawFileName = Path.GetFileNameWithoutExtension(model.PhotoFile.FileName);            
            var uniquePhotoName = Guid.NewGuid() + "-" + rawFileName + fileExtension;

            var folderNameInDropbox = Regex.Replace(contest.Title, "\\s+", "");

            var sharedLink = await DropboxManager.Upload("/" + folderNameInDropbox, uniquePhotoName, model.PhotoFile.InputStream);
            var rawSharedLink = sharedLink.Substring(0, sharedLink.IndexOf("?")) + "?raw=1";

            var newImage = new Image()
            {
                Title = model.Title,
                Description = model.Description,
                Contest = contest,
                CreatedOn = DateTime.Now,
                ImagePath = rawSharedLink,
                UserId = userId,
                FileName = uniquePhotoName
            };

            this.Data.Images.Add(newImage);
            await this.Data.SaveChangesAsync();
            
            TempData["successUpload"] = "Image was successfully uploaded.";
            return this.Redirect("/");//Will be changed
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var imageTarget = this.Data.Images.GetById(id);
            if (imageTarget == null)
            {
                return this.Content("No record for this image in the database");
            }

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || userId != imageTarget.User.Id)
            {
                return this.Content("You don't have the right to delete this picture.");
            }
            
            var folderNameInDropbox = Regex.Replace(imageTarget.Contest.Title, "\\s+", "");
            bool deleted = await DropboxManager.Delete("/" + folderNameInDropbox + "/" + imageTarget.FileName);

            if (!deleted)
            {
                return this.Content("Unable to delete image.");
            }

            this.Data.Images.Delete(imageTarget);
            await this.Data.SaveChangesAsync();

            return this.Content("Image was successfully deleted.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ViewPicture(int id)
        {
            var imageTarget = this.Data.Images.GetById(id);
            if (imageTarget == null)
            {
                return this.Content("No record for this image in the database");
            }

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || userId != imageTarget.User.Id)
            {
                return this.Content("You don't have the right to delete this picture.");
            }

            var folderNameInDropbox = Regex.Replace(imageTarget.Contest.Title, "\\s+", "");
            bool deleted = await DropboxManager.Delete("/" + folderNameInDropbox + "/" + imageTarget.FileName);

            if (!deleted)
            {
                return this.Content("Unable to delete image.");
            }

            this.Data.Images.Delete(imageTarget);
            await this.Data.SaveChangesAsync();

            return this.Content("Image was successfully deleted.");
        }

        private bool rightToParticipate(Contest contest, string userId)
        {
            bool validContest = contest != null;

            bool isOpen = contest.State.Equals(TypeOfEnding.Ongoing);

            bool openAccTime = contest.ParticipationEndTime == null ? true : contest.ParticipationEndTime.Value > DateTime.Now;
            bool deadlineByTime = contest.DeadlineStrategy.Equals(DeadlineStrategy.ByTime) && openAccTime;
            bool deadlineByNumParticipants = contest.DeadlineStrategy.Equals(DeadlineStrategy.ByNumberOfParticipants) &&
                contest.Participants.Count() < contest.MaxParticipationsCount.Value;
            
            bool rightToParticipateOpen = contest.VotingStrategy.Equals(Strategy.Open);
            bool rightToParticipateClose = contest.VotingStrategy.Equals(Strategy.Closed) && contest.Participants.Any(p => p.Id == userId);     

            return validContest && isOpen && (rightToParticipateClose || rightToParticipateOpen) && (deadlineByTime || deadlineByNumParticipants);
        }
    }
}