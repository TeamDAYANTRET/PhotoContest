using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class IndexPageViewModel
    {
        public IEnumerable<ContestBasicInfoViewModel> OngoingContests { get; set; }
        public IEnumerable<ContestBasicInfoViewModel> EndedContests { get; set; }
    }
}