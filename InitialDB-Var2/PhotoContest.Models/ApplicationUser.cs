using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<Image> pictures;
        private ICollection<Image> votedPictures;
        private ICollection<Contest> committeeParticipation;
        private ICollection<Contest> contestParticipation;
        private ICollection<Notification> notifications;
        private ICollection<Prize> prizes;
        private ICollection<Message> recievedMessages;//Update 1
        private ICollection<Message> sendMessages;//Update 1
        private ICollection<Comment> comments;//Update 1
        private ICollection<Contest> contestsWon;//Update 1
        private ICollection<Image> favoritePictures;//Updated 1

        public ApplicationUser()
        {
            this.pictures = new HashSet<Image>();
            this.votedPictures = new HashSet<Image>();
            this.committeeParticipation = new HashSet<Contest>();
            this.contestParticipation = new HashSet<Contest>();
            this.notifications = new HashSet<Notification>();
            this.prizes = new HashSet<Prize>();
            this.recievedMessages = new HashSet<Message>();//Update 1
            this.sendMessages = new HashSet<Message>();//Update 1
            this.comments = new HashSet<Comment>();//Update 1
            this.contestsWon = new HashSet<Contest>();//Update 1
            this.favoritePictures = new HashSet<Image>();//Update 1
        }

        public string FirstName { get; set; }//Update 1

        public string LastName { get; set; }//Update 1

        public string AboutMe { get; set; }//Update 1

        public DateTime? JoinedAt { get; set; }//Update 1

        public string ProfilePic { get; set; }//Update 1

        public virtual ICollection<Image> Pictures { get { return this.pictures; } set { this.pictures = value; } }

        public virtual ICollection<Image> VotedPictures { get { return this.votedPictures; } set { this.votedPictures = value; } }

        public virtual ICollection<Contest> CommitteeParticipation { get { return this.committeeParticipation; } set { this.committeeParticipation = value; } }

        public virtual ICollection<Contest> ContestParticipation { get { return this.contestParticipation; } set { this.contestParticipation = value; } }

        public virtual ICollection<Notification> Notifications { get { return this.notifications; } set { this.notifications = value; } }

        public virtual ICollection<Prize> Prizes { get { return this.prizes; } set { this.prizes = value; } }

        public virtual ICollection<Message> MessagesSend { get { return this.sendMessages; } set { this.sendMessages = value; } }//Update 1        

        public virtual ICollection<Message> RecievedMessages { get { return this.recievedMessages; } set { this.recievedMessages = value; } }//Update 1 

        public virtual ICollection<Comment> Comments { get { return this.comments; } set { this.comments = value; } }//Update 1 

        public virtual ICollection<Contest> ContestsWon { get { return this.contestsWon; } set { this.contestsWon = value; } }//Update 1 

        public virtual ICollection<Image> FavoritePictures { get { return this.favoritePictures; } set { this.favoritePictures = value; } }//Update 1 





        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
