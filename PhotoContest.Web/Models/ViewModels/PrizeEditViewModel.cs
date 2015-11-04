using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class PrizeEditViewModel
    {
        public int Id { get; set; }

        public int ContestId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Place { get; set; }
    }
}