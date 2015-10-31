namespace PhotoContest.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoContest.Models.Enumerations;

    public class Tag
    {
        private ICollection<Image> images;

        public Tag()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Image> Images { get { return this.images; } set { this.images = value; } }
    }
}
