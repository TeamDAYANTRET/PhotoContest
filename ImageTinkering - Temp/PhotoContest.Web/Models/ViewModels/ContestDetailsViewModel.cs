using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNet.Identity;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ContestDetailsViewModel : ContestBasicInfoViewModel
    {
        public IEnumerable<ImageViewModel> Picuters { get; set; }
        public bool CanParticipate { get; set; }
        public bool CanVote { get; set; }

        public static Expression<Func<Contest, ContestDetailsViewModel>> Create
        {
            get
            {
                return contest => new ContestDetailsViewModel()
                {
                    Id = contest.Id,
                    EndTime = contest.ContestEndTime,
                    MaxParticipationsCount = contest.MaxParticipationsCount,
                    ParticipationStrategy = contest.ParticipationStrategy,
                    Title = contest.Title,
                    VotingStrategy = contest.VotingStrategy,
                    State = contest.State,
                    ParticipationEndTime = contest.ParticipationEndTime ?? default(DateTime),
                    ParticipationsCount = contest.Pictures.Select(p => p.User.Id).Distinct().Count(),
                    OwnerId = contest.OwnerId,
                    Picuters = contest.Pictures.Select(p => new ImageViewModel() 
                    {
                        Id = p.Id,
                        Path = p.ImagePath,
                        VotesCount = p.Votes.Count()
                    })
                };
            }
        }
    }
}