using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PhotoContest.Models;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ProfileUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; } 
        public string FullName { get; set; }
        public string AboutMe { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Joined { get; set; }
        public string ProfilePath { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public int OwnedImagesCount { get; set; }
        public int ReceivedPrizesCount { get; set; }

        public static Expression<Func<ApplicationUser, ProfileUserViewModel>> Create
        {
            get
            {
                return profile => new ProfileUserViewModel()
                {
                    Id = profile.Id,
                    UserName = profile.UserName,
                    FullName = profile.FirstName + " " + profile.LastName,
                    AboutMe = profile.AboutMe,
                    Joined = profile.JoinedAt.Value,
                    ProfilePath = profile.ProfilePic,
                    Email = profile.Email,
                    PhoneNumber = profile.PhoneNumber,
                    OwnedImagesCount = profile.Pictures.Count,
                    ReceivedPrizesCount = profile.Prizes.Count,
                };
            }
        }
    }
}