using PhotoContest.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Models
{
    public class Contest
    {
        private ICollection<Image> pictures;
        private ICollection<ApplicationUser> committeeMembers;
        private ICollection<ApplicationUser> participants;
        private ICollection<Prize> prizes;

        public Contest()
        {
            this.pictures = new HashSet<Image>();
            this.committeeMembers = new HashSet<ApplicationUser>();
            this.participants = new HashSet<ApplicationUser>();
            this.prizes = new HashSet<Prize>();
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

        public RewardStrategy RewardStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public TypeOfEnding Ended { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime? EndTime { get; set; }

        public int? MaxParticipationsCount { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Image> Pictures { get { return this.pictures; } set { this.pictures = value; } }

        public virtual ICollection<ApplicationUser> Participants { get { return this.participants; } set { this.participants = value; } }

        public virtual ICollection<ApplicationUser> CommitteeMembers { get { return this.committeeMembers; } set { this.committeeMembers = value; } }

        public virtual ICollection<Prize> Prizes { get { return this.prizes; } set { this.prizes = value; } }
    }
}
