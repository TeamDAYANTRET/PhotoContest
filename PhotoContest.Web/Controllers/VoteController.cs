using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoContest.Data;
using PhotoContest.Data.Contracts;

namespace PhotoContest.Web.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {

        public VoteController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult AddVote(int id)
        {
            var picture = this.Data.Images.GetById(id);
            if (picture == null)
            {
                return this.HttpNotFound("Image is not found.");
            }
            var loginUser = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(loginUser);
            var votedImage = this.Data.Contests.GetById(picture.ContestId)
                .Pictures.FirstOrDefault(c => c.Votes.FirstOrDefault(v => v.UserName == currentUser.UserName) != null);

            if (votedImage == null)
            {
                currentUser.VotedPictures.Add(picture);
                this.Data.SaveChanges();
            }
            var count = picture.Votes.Count;
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveVote(int id)
        {
            var picture = this.Data.Images.GetById(id);
            if (picture == null)
            {
                return this.HttpNotFound("Image is not found.");
            }
            var loginUser = User.Identity.GetUserId();
            var currentUser = this.Data.Users.GetById(loginUser);
            var votedImage = this.Data.Images.GetById(picture.Id)
                .Votes.FirstOrDefault(v => v.UserName == currentUser.UserName);
            if (votedImage != null)
            {
                currentUser.VotedPictures.Remove(picture);
                this.Data.SaveChanges();
            }
            var count = picture.Votes.Count;
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}