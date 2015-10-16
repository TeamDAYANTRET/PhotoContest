namespace PhotoContest.Models
{
    using System;

    using System.ComponentModel.DataAnnotations;

    public class Message//Update 1
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool Read { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}
