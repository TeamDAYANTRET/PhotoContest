using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using PhotoContest.Models.Enumerations;
using PhotoContest.Models;

namespace PhotoContest.Web.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {
        private const string voteSuccessPreffix = "<span class='fa fa-thumbs-o-down'></span>";
        private const string voteSuccessSuff = "<span class='fa fa-heart'></span>";
        private const string voteEnablePreffix = "<span class='fa fa-thumbs-o-up'></span>";
        private const string voteEnableSuffix = "";

        public VoteController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult ManageVote(int id)
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
            var votedImageInContest= this.Data.Contests.GetById(picture.ContestId)
                 .Pictures.FirstOrDefault(c => c.Votes.FirstOrDefault(v => v.UserName == currentUser.UserName) != null);


            var contest = picture.Contest;

            if (votedImage != null)
            {
                currentUser.VotedPictures.Remove(picture);
                this.Data.SaveChanges();

                return this.Content(voteEnablePreffix + picture.Votes.Count() + voteEnableSuffix);
            }
            else
            {
                if (contest.VotingStrategy == Strategy.Open && contest.State == TypeOfEnding.Ongoing && votedImageInContest==null)
                {
                    currentUser.VotedPictures.Add(picture);
                    this.Data.SaveChanges();

                    return this.Content(voteSuccessPreffix + picture.Votes.Count + voteSuccessSuff);
                }
                else if (contest.VotingStrategy == Strategy.Closed && contest.State == TypeOfEnding.Ongoing)
                { 
                    var comitteeMembers = contest.CommitteeMembers.FirstOrDefault(x => x.Id == loginUser);

                    if (comitteeMembers != null && votedImageInContest == null)
                    {
                        currentUser.VotedPictures.Add(picture);
                        this.Data.SaveChanges();

                        return this.Content(voteEnablePreffix + picture.Votes.Count + voteEnableSuffix);
                    }
                    else
                        return this.Content("X " + picture.Votes.Count);
                }
            }

            return this.Content("X " + picture.Votes.Count);
        } 
    }
}