using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PhotoContest.Web.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Autor Username")]
        [StringLength(25, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 5)]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Comment")]
        [StringLength(255, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 5)]
        public string Content { get; set; }
    }
}