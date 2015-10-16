namespace PhotoContest.Models
{
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;

    public class Comment //Upgrade 1
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
                
        public virtual ApplicationUser Author { get; set; }
        public virtual Image Picture { get; set; }
    }
}
