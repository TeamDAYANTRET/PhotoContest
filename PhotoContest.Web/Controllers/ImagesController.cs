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
    using Data.Contracts;
    using App_Start;
    using PhotoContest.Models.Enumerations;
    using PhotoContest.Models;
    using System.Text.RegularExpressions;
    using Models.ViewModels;
    using AutoMapper;

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
            var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == model.ContestId);

            if (!this.ModelState.IsValid || model == null)
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

                TempData["uploadFail"] = "Wrong input. " + string.Join("\n", errorList);
                return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
            }

            var userId = User.Identity.GetUserId();
            //var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == model.ContestId);

            if (!this.RightToParticipate(contest, userId))
            {
                TempData["uploadFail"] = "You have no right to participate in this contest.";
                return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
            }

            var fileExtension = Path.GetExtension(model.PhotoFile.FileName);
            var rawFileName = Path.GetFileNameWithoutExtension(model.PhotoFile.FileName);
            var uniquePhotoName = Guid.NewGuid() + "-" + rawFileName + fileExtension;

            var folderNameInDropbox = Regex.Replace(contest.Title, "\\s+", "");

            var sharedLink = await DropboxManager.Upload("/" + folderNameInDropbox, uniquePhotoName, model.PhotoFile.InputStream);

            if (sharedLink == null)
            {
                TempData["uploadFail"] = "No connection to the cloud environment. \nPlease try again later";
                return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
            }

            var rawSharedLink = sharedLink.Substring(0, sharedLink.IndexOf("?")) + "?raw=1";

            var newImage = new Image()
            {
                Title = model.Title,
                Description = model.Description,
                ContestId = model.ContestId,
                CreatedOn = DateTime.Now,
                ImagePath = rawSharedLink,
                UserId = userId,
                FileName = uniquePhotoName
            };

            this.Data.Images.Add(newImage);
            await this.Data.SaveChangesAsync();

            TempData["successUpload"] = "Image was successfully uploaded.";
            return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var imageTarget = this.Data.Images.GetById(id);
            if (imageTarget == null)
            {
                return new HttpStatusCodeResult(400, "No record for this image in the database");
            }

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || userId != imageTarget.User.Id)
            {
                return new HttpStatusCodeResult(400, "You don't have the right to delete this picture.");
            }

            var folderNameInDropbox = Regex.Replace(imageTarget.Contest.Title, "\\s+", "");
            bool deleted = await DropboxManager.Delete("/" + folderNameInDropbox + "/" + imageTarget.FileName);

            if (!deleted)
            {
                return new HttpStatusCodeResult(500, "Unable to delete image.");
            }

            int contestId = imageTarget.Contest.Id;

            this.Data.Images.Delete(imageTarget);
            await this.Data.SaveChangesAsync();

            var imgs = this.Data.Contests.All().First(c => c.Id == contestId).Pictures
                .Select(p => new PagedImageViewModel()
                {
                    Id = p.Id,
                    ImagePath = p.ImagePath,
                    VotesCount = p.Votes.Count(),
                    AuthorUsername = p.User.UserName,
                    HasVoted = p.Votes.Any(u => u.Id == userId)
                });

            TempData["succDelete"] = "Image was successfully deleted.";
            return PartialView("_ListPagedImages", imgs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditImage(EditImageBindingModel model)
        {
            var imageTarget = this.Data.Images.GetById(model.Id);
            if (imageTarget == null)
            {
                return new HttpStatusCodeResult(400, "No record for this image in the database");
            }

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin") || userId != imageTarget.User.Id)
            {
                return this.Content("You don't have the right to edit this picture.");
            }

            imageTarget.Title = model.Title;
            imageTarget.Description = model.Description;

            await this.Data.SaveChangesAsync();

            return this.Content("The image was successfully edited.");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewImage(int id)
        {
            var imageTarget = this.Data.Images.GetById(id);
            if (imageTarget == null)
            {
                return new HttpStatusCodeResult(400, "No record for this image in the database");
            }

            var imgPreview = new ImageViewModel(imageTarget);

            return this.View(imgPreview);
        }

        private bool RightToParticipate(Contest contest, string userId)
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