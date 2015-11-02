using PhotoContest.Models;
using PhotoContest.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ContestBasicInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime ParticipationEndTime { get; set; }
        public int? MaxParticipationsCount { get; set; }
        public int ParticipationsCount { get; set; }
        public Strategy VotingStrategy { get; set; }
        public Strategy ParticipationStrategy { get; set; }
        public TypeOfEnding State { get; set; }
        public string OwnerId { get; set; }

        public static Expression<Func<Contest, ContestBasicInfoViewModel>> Create
        {
            get 
            {
                return contest => new ContestBasicInfoViewModel()
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
                    OwnerId = contest.OwnerId
                };
            } 
        }
    }
}