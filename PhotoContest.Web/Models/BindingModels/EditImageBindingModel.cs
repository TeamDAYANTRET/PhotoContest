using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    using System.Web;
    using Attributes;

    using System.ComponentModel.DataAnnotations;

    public class EditImageBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Image Title")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}