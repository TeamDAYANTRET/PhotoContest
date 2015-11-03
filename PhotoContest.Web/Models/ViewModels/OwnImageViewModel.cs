using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using PhotoContest.Models;
using PhotoContest.Web.Attributes;


namespace PhotoContest.Web.Models.ViewModels
{
    public class OwnImageViewModel
    {
        public int Id { get; set; }
        public string ImgPath { get; set; }
        public string ContestName { get; set; }
        [DatetimeGreaterThanNow]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }
        public int Votes { get; set; }
        public string State { get; set; }

        public static Expression<Func<Image, OwnImageViewModel>> Create
        {
            get
            {
                return images => new OwnImageViewModel()
                {
                    Id = images.Id,
                    ImgPath = images.ImagePath,
                    ContestName = images.Contest.Title,
                    EndTime = images.Contest.ContestEndTime,
                    Votes = images.Votes.Count,
                    State=images.Contest.State.ToString()
                };
            }
        }
    }
}