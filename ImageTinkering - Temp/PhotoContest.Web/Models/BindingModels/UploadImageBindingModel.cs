using PhotoContest.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Models.BindingModels
{
    public class UploadImageBindingModel
    {
        [Required]
        [Display(Name = "Image Title")]
        [StringLength(20, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [ValidImage(2097152)] //Max size 2MB
        public HttpPostedFileBase PhotoFile { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Contest Name")]
        public int ContestId { get; set; }
    }
}