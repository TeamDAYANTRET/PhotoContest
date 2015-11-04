using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class EndedContestBasicInfoViewModel : ContestBasicInfoViewModel
    {
        public DateTime? EndTime { get; set; }


        public static Expression<Func<Contest, EndedContestBasicInfoViewModel>> Create
        {
            get
            {
                return contest => new EndedContestBasicInfoViewModel()
                {
                    Id = contest.Id,
                    EndTime = contest.ContestEndTime,
                    MaxParticipationsCount = contest.MaxParticipationsCount,
                    ParticipationStrategy = contest.ParticipationStrategy,
                    Title = contest.Title,
                    VotingStrategy = contest.VotingStrategy,
                    ParticipationsCount = contest.Pictures.Select(p => p.User.Id).Distinct().Count(),
                    OwnerId = contest.OwnerId,
                    Prizes = contest.Prizes.Count
                };
            }
        }
    }
}