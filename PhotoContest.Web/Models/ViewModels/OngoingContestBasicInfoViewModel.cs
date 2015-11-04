using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class OngoingContestBasicInfoViewModel : ContestBasicInfoViewModel
    {
        public DateTime ParticipationEndTime { get; set; }

        public static Expression<Func<Contest, OngoingContestBasicInfoViewModel>> Create
        {
            get
            {
                return contest => new OngoingContestBasicInfoViewModel()
                {
                    Id = contest.Id,
                    MaxParticipationsCount = contest.MaxParticipationsCount,
                    ParticipationStrategy = contest.ParticipationStrategy,
                    Title = contest.Title,
                    VotingStrategy = contest.VotingStrategy,
                    ParticipationEndTime = contest.ParticipationEndTime ?? default(DateTime),
                    ParticipationsCount = contest.Pictures.Select(p => p.User.Id).Distinct().Count(),
                    OwnerId = contest.OwnerId,
                    Prizes = contest.Prizes.Count
                };
            }
        }
    }
}