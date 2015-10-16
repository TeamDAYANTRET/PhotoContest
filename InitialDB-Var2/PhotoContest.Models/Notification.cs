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

        public DateTime SendOn { get; set; }

        public NotificationType Type { get; set; }

        public bool Read { get; set; } //Update 1

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
