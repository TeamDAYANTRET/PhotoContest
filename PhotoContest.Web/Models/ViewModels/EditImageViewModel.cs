using PhotoContest.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class EditImageViewModel
    {
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

        [Display(Name = "Description")]
        public int Description { get; set; }

        [Required]
        [Display(Name = "Author")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Posted on")]
        public DateTime PostedOn { get; set; }
    }
}