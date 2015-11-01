namespace PhotoContest.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using PhotoContest.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using PhotoContest.Data.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using PhotoContest.Data.Contracts;

    public class PhotoContestDbContext : IdentityDbContext<ApplicationUser>, IPhotoContestDbContext
    {
        public PhotoContestDbContext()
            : base("name=PhotoContestDbContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoContestDbContext, Configuration>());
        }

        public static PhotoContestDbContext Create()
        {
            return new PhotoContestDbContext();
        }

        public virtual IDbSet<Image> Images { get; set; }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public virtual IDbSet<Prize> Prizes { get; set; }

        public virtual IDbSet<Tag> Tags { get; set; }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(x => x.VotedPictures)
              .WithMany(v => v.Votes)
              .Map(m =>
              {
                  m.MapLeftKey("UserId");
                  m.MapRightKey("PictureId");
                  m.ToTable("Votes");
              });

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(x => x.CommitteeParticipation)
              .WithMany(v => v.CommitteeMembers)
              .Map(m =>
              {
                  m.MapLeftKey("UserId");
                  m.MapRightKey("ContestId");
                  m.ToTable("Committees");
              });

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(x => x.ContestParticipation)
              .WithMany(v => v.Participants)
              .Map(m =>
              {
                  m.MapLeftKey("UserId");
                  m.MapRightKey("ContestId");
                  m.ToTable("ContestParticipations");
              });

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(u => u.FavoritePictures)
              .WithMany(i => i.FavoredBy)
              .Map(m =>
              {
                  m.MapLeftKey("UserId");
                  m.MapRightKey("ImageId");
                  m.ToTable("FavoriteImages");
              });

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(u => u.MessagesSend)
               .WithRequired(t => t.Sender)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.RecievedMessages)
                .WithRequired(t => t.Recipient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(u => u.Prizes)
               .WithOptional(p => p.User)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithRequired(c => c.Author);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Notifications)
                .WithRequired(c => c.User);

            modelBuilder.Entity<Image>()
                .HasMany(i => i.Comments)
                .WithRequired(c => c.Picture);

            modelBuilder.Entity<Image>()
                .HasOptional(i => i.Prize)
                .WithOptionalPrincipal(p => p.Picture);

            modelBuilder.Entity<Contest>()
                .HasMany(c => c.Prizes)
                .WithRequired(p => p.Contest);

            modelBuilder.Entity<Tag>()
              .HasMany(t => t.Images)
              .WithMany(i => i.Tags)
              .Map(m =>
              {
                  m.MapLeftKey("ImageId");
                  m.MapRightKey("TagId");
                  m.ToTable("ImageTags");
              });

            base.OnModelCreating(modelBuilder);
        }
    }
}