namespace PhotoContest.Web.Models.BindingModels
{
    using System.Web;
    using Attributes;

    using System.ComponentModel.DataAnnotations;

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