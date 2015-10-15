namespace PhotoContest.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using PhotoContest.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using PhotoContest.Data.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class PhotoContestDbContext : IdentityDbContext<ApplicationUser>
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

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Contest> Contests { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Prize> Prizes { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}