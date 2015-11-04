using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PhotoContest.Models;

namespace PhotoContest.Web.Models.ViewModels
{
    public class PrizesForGoingContestViewModel
    {
        public int? Place { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContestId { get; set; }
        public string ContestName { get; set; }

        public static Expression<Func<Prize, PrizesForGoingContestViewModel>> Create
        {
            get
            {
                return pz => new PrizesForGoingContestViewModel()
                {
                    Place = pz.ForPlace,
                    Name = pz.Name,
                    Description = pz.Description,
                    ContestId = pz.ContestId,
                    ContestName = pz.Contest.Title
                };
            }
        }
    }
}