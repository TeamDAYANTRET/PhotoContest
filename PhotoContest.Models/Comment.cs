namespace PhotoContest.Models
{
    using System;

    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int PictureId { get; set; }

        public virtual Image Picture { get; set; }
    }
}
