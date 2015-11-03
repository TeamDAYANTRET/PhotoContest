namespace PhotoContest.Web.Models.BindingModels
{
    using Attributes;
    using System.ComponentModel.DataAnnotations;

    public class NotificationBindingModel
    {
        [Required(ErrorMessage = "Notification type is requred.")]
        [ValidInteger(0, 10)]
        public int NotificationType { get; set; }


        [Required(ErrorMessage = "Notification message is requred.")]
        [StringLength(555, ErrorMessage = "The {0} must be min {2} and max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }
    }
}