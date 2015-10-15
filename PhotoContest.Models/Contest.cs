using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class Contest
    {
        private ICollection<Image> pictures;
        private ICollection<ApplicationUser> committeeMembers;
        private ICollection<ApplicationUser> participants;

        public Contest()
        {
            this.pictures = new HashSet<Image>();
            this.committeeMembers = new HashSet<ApplicationUser>();
            this.participants = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int PossibleWinnersCount { get; set; }

        public Strategy VotingStrategy { get; set; }

        public Strategy ParticipationStrategy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EndTime { get; set; }

        public int? MaxParticipationsCount { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Image> Pictures { get { return this.pictures; } set { this.pictures = value; } }

        public virtual ICollection<ApplicationUser> Participants { get { return this.participants; } set { this.participants = value; } }

        public virtual ICollection<ApplicationUser> CommitteeMembers { get { return this.committeeMembers; } set { this.committeeMembers = value; } }
    }
}
