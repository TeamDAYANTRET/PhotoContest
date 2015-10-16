namespace PhotoContest.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoContest.Models.Enumerations;

    public class Tag//Update 1
    {
        private ICollection<Image> images;

        public Tag()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }

        public virtual ICollection<Image> Images { get { return this.images; } set { this.images = value; } }
    }
}
