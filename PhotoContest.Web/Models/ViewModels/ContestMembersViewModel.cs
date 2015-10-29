using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ContestMembersViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<UserBasicViewModel> Users { get; set; }
    }
}