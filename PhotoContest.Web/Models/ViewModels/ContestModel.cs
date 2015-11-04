using PhotoContest.Models;
using PhotoContest.Models.Enumerations;
using PhotoContest.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ContestModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1, Int32.MaxValue)]
        [Display(Name = "Possible Winners Count")]
        public int PossibleWinnersCount { get; set; }

        public Strategy VotingStrategy { get; set; }

        public Strategy ParticipationStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public TypeOfEnding State { get; set; }

        [DatetimeGreaterThanNow]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ParticipationEndTime { get; set; }

        public int? MaxParticipationsCount { get; set; }
        public int Prizes { get; set; }

        public static Expression<Func<Contest, ContestModel>> Create
        {
            get
            {
                return contest => new ContestModel()
                {
                    Id = contest.Id,
                    Title = contest.Title,
                    Description = contest.Description,
                    MaxParticipationsCount = contest.MaxParticipationsCount,
                    ParticipationEndTime = contest.ParticipationEndTime,
                    ParticipationStrategy = contest.ParticipationStrategy,
                    PossibleWinnersCount = contest.PossibleWinnersCount,
                    VotingStrategy = contest.VotingStrategy,
                    State = contest.State,
                    DeadlineStrategy = contest.DeadlineStrategy,
                    Prizes = contest.Prizes.Count
                };
            }
        }
    }
}