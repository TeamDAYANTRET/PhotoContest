using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class Prize
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int ForPlace { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? PictureId { get; set; }

        public virtual Image Picture { get; set; }
    }
}
