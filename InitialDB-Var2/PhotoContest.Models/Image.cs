using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class Image
    {
        private ICollection<ApplicationUser> votes { get; set; }
        private ICollection<Tag> tags { get; set; }//Update 1
        private ICollection<Comment> comments { get; set; }//Update 1
        private ICollection<ApplicationUser> favoredBy { get; set; }//Update 1

        public Image()
        {
            this.votes = new HashSet<ApplicationUser>();
            this.tags = new HashSet<Tag>();//Update 1
            this.comments = new HashSet<Comment>();//Update 1
            this.favoredBy = new HashSet<ApplicationUser>();//Update 1
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public string ImageURL { get; set; } //Update 1

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public virtual Prize Prize { get; set; }//Update 1

        public virtual ICollection<ApplicationUser> Votes { get { return this.votes; } set { this.votes = value; } }

        public virtual ICollection<Tag> Tags { get { return this.tags; } set { this.tags = value; } }//Update 1

        public virtual ICollection<Comment> Comments { get { return this.comments; } set { this.comments = value; } }//Update 1

        public virtual ICollection<ApplicationUser> FavoredBy { get { return this.favoredBy; } set { this.favoredBy = value; } }//Update 1

    }
}
