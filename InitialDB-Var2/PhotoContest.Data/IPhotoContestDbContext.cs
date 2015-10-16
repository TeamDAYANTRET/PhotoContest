namespace PhotoContest.Data
{
    using PhotoContest.Models;

    using System.Data.Entity;

    public interface IPhotoContestDbContext
    {
        IDbSet<Image> Images { get; set; }

        IDbSet<Contest> Contests { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        IDbSet<Prize> Prizes { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<Message> Messages { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<Category> Categories { get; set; }
    }
}
