using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PhotoContest.Models;

namespace PhotoContest.Web.Models.ViewModels
{
    public class VotedImagesViewModel
    {
        public string ImgPath { get; set; }
        public string ContestName { get; set; }
        public DateTime? EndTime { get; set; }
        public int Votes { get; set; }

        public static Expression<Func<Image, VotedImagesViewModel>> Create
        {
            get
            {
                return images => new VotedImagesViewModel()
                {
                    ImgPath = images.ImagePath,
                    ContestName = images.Contest.Title,
                    EndTime = images.Contest.ContestEndTime,
                    Votes = images.Votes.Count
                };
            }
        }
    }
}