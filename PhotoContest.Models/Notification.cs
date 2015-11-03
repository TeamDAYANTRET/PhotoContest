using PhotoContest.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SendOn { get; set; }

        [Required(ErrorMessage = "Notification type is requred.")]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } 

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
