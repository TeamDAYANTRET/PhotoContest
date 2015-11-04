using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PhotoContest.Models;

namespace PhotoContest.Web.Models.ViewModels
{
    public class PrizesForEndedContestViewModel
    {
        public int? Place { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContestId { get; set; }
        public string ContestName { get; set; }
        public string WinnerName { get; set; }
        public string WinnerImage { get; set; }
        public string Votes { get; set; }

        public static Expression<Func<Prize, PrizesForEndedContestViewModel>> Create
        {
            get
            {
                return pz => new PrizesForEndedContestViewModel()
                {
                    Place = pz.ForPlace,
                    Name = pz.Name,
                    Description = pz.Description,
                    ContestId = pz.ContestId,
                    ContestName = pz.Contest.Title,
                    WinnerName = pz.User.UserName,
                    WinnerImage = pz.Picture.ImagePath,
                    Votes=pz.Picture.Votes.Count.ToString()
                };
            }
        }
    }
}