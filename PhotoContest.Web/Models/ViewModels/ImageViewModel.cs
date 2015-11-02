using AutoMapper;
using PhotoContest.Models;
using PhotoContest.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace PhotoContest.Web.Models.ViewModels
{
    public class ImageViewModel
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
        [Display(Name = "Link")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Title { get; set; }

        [Display(Name = "Won Place")]
        public int? WinnerPlace { get; set; }
        
        [Required]
        [Display(Name = "Uploaded by:")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Posted on")]
        public DateTime PostedOn { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        [UIHint("Tags")]
        public IEnumerable<string> Tags { get; set; }

        public int ContestId { get; set; }

        public bool IsAuthorOrAdmin { get; set; }

        public static Expression<Func<Image, ImageViewModel>> Create
        {
            get
            {
                return img => new ImageViewModel
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath,
                    VotesCount = img.Votes.Count,
                    Title = img.Title,
                    WinnerPlace = img.Prize.ForPlace == null ? null : (int?)img.Prize.ForPlace.Value,
                    Author = img.User.UserName,
                    PostedOn = img.CreatedOn,
                    Comments = img.Comments.Select(c => new CommentViewModel()
                    {
                        Id = c.Id,
                        Author = c.Author.UserName,
                        CreatedAt = c.CreatedAt,
                        Content = c.Content
                    }),
                    Tags = img.Tags.Select(t => t.Name)
                    };
            }
        }

        public ImageViewModel() { }

        public ImageViewModel(Image img)
        {
            Id = img.Id;
            ImagePath = img.ImagePath;
            VotesCount = img.Votes.Count;
            Title = img.Title;
            WinnerPlace = img.Prize == null ? null : img.Prize.ForPlace == null ? null : (int?)img.Prize.ForPlace.Value;
            Author = img.User.UserName;
            PostedOn = img.CreatedOn;
            Comments = img.Comments.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                Author = c.Author.UserName,
                CreatedAt = c.CreatedAt,
                Content = c.Content
            });
            ContestId = img.Contest.Id;
            Tags = img.Tags.Select(t => t.Name);
        }
    }
}