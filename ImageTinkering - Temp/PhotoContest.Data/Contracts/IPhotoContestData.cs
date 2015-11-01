using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Data.Contracts
{
    public interface IPhotoContestData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<Category> Categories { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Image> Images { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<Prize> Prizes { get; }

        IRepository<Tag> Tags { get; }

        IRepository<Message> Messages { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
