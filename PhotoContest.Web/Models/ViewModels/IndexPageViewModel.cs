using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class IndexPageViewModel
    {
        public IEnumerable<OngoingContestBasicInfoViewModel> OngoingContests { get; set; }
        public IEnumerable<EndedContestBasicInfoViewModel> EndedContests { get; set; }
    }
}