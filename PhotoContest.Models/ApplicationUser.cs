using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        public ApplicationUser()
        {
            this.pictures = new HashSet<Image>();
            this.votedPictures = new HashSet<Image>();
            this.committeeParticipation = new HashSet<Contest>();
            this.contestParticipation = new HashSet<Contest>();
            this.notifications = new HashSet<Notification>();
            this.prizes = new HashSet<Prize>();
        }

        public virtual ICollection<Image> Pictures { get { return this.pictures; } set { this.pictures = value; } }

        public virtual ICollection<Image> VotedPictures { get { return this.votedPictures; } set { this.votedPictures = value; } }

        public virtual ICollection<Contest> CommitteeParticipation { get { return this.committeeParticipation; } set { this.committeeParticipation = value; } }

        public virtual ICollection<Contest> ContestParticipation { get { return this.contestParticipation; } set { this.contestParticipation = value; } }

        public virtual ICollection<Notification> Notifications { get { return this.notifications; } set { this.notifications = value; } }

        public virtual ICollection<Prize> Prizes { get { return this.prizes; } set { this.prizes = value; } }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
