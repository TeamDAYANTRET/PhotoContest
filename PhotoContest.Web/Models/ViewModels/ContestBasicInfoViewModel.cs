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
        public int? MaxParticipationsCount { get; set; }
        public int ParticipationsCount { get; set; }
        public Strategy VotingStrategy { get; set; }
        public Strategy ParticipationStrategy { get; set; }
        public string OwnerId { get; set; }
    }
}