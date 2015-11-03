using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNet.Identity;
using PhotoContest.Models.Enumerations;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ContestDetailsViewModel : ContestBasicInfoViewModel
    {
        public IEnumerable<PagedImageViewModel> Picuters { get; set; }
        public bool CanParticipate { get; set; }
        public bool CanVote { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime ParticipationEndTime { get; set; }
        public TypeOfEnding State { get; set; }
    }
}