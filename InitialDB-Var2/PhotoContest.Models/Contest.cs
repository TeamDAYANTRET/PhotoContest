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
        private ICollection<ApplicationUser> winners;//Update 1
        private ICollection<Prize> prizes;//Update 1
        private ICollection<Category> categories;//Update 1

        public Contest()
        {
            this.pictures = new HashSet<Image>();
            this.committeeMembers = new HashSet<ApplicationUser>();
            this.participants = new HashSet<ApplicationUser>();
            this.winners = new HashSet<ApplicationUser>();//Update 1
            this.prizes = new HashSet<Prize>();//Update 1
            this.categories = new HashSet<Category>();//Update 1
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

        public RewardStrategy RewardStrategy { get; set; } //Update 1

        public DeadlineStrategy DeadlineStrategy { get; set; } //Update 1

        public TypeOfEnding Ended { get; set; } //Update 1

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }//Update 1

        public DateTime? EndTime { get; set; }

        public int? MaxParticipationsCount { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Image> Pictures { get { return this.pictures; } set { this.pictures = value; } }

        public virtual ICollection<ApplicationUser> Participants { get { return this.participants; } set { this.participants = value; } }

        public virtual ICollection<ApplicationUser> CommitteeMembers { get { return this.committeeMembers; } set { this.committeeMembers = value; } }

        public virtual ICollection<ApplicationUser> Winners { get { return this.winners; } set { this.winners = value; } } //Update 1

        public virtual ICollection<Prize> Prizes { get { return this.prizes; } set { this.prizes = value; } } //Update 1

        public virtual ICollection<Category> Categories { get { return this.categories; } set { this.categories = value; } } //Update 1
    }
}
