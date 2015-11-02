namespace PhotoContest.Web.Models.ViewModels
{
    using Antlr.Runtime.Misc;
    using Attributes;
    using PhotoContest.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    public class PagedImageViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Link")]
        [StringLength(255, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 25)]
        public string ImagePath { get; set; }

        [ValidInteger(0, 255)]
        public int VotesCount { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(25, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 5)]
        public string AuthorUsername { get; set; }


        public static Expression<Func<Image, PagedImageViewModel>> Create
        {
            get
            {
                return img => new PagedImageViewModel
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath,
                    VotesCount = img.Votes.Count,
                    AuthorUsername = img.User.UserName
                };
            }
        }
    }
}