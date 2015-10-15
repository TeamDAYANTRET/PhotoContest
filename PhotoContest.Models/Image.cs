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

        public Image()
        {
            this.votes = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public virtual ICollection<ApplicationUser> Votes { get { return this.votes; } set { this.votes = value; } }
    }
}
