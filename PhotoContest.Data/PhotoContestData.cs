using PhotoContest.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoContest.Models;

namespace PhotoContest.Data
{
    public class PhotoContestData : IPhotoContestData
    {
        private readonly DbContext context;

        private readonly IDictionary<Type, object> repositories;

        public PhotoContestData()
            : this(new PhotoContestDbContext())
        {
        }

        public PhotoContestData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Category> Categories
        {
            get { return this.GetRepository<Category>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<Contest> Contests
        {
            get { return this.GetRepository<Contest>(); }
        }

        public IRepository<Image> Images
        {
            get { return this.GetRepository<Image>(); }
        }

        public IRepository<Notification> Notifications
        {
            get { return this.GetRepository<Notification>(); }
        }

        public IRepository<Prize> Prizes
        {
            get { return this.GetRepository<Prize>(); }
        }

        public IRepository<Tag> Tags
        {
            get { return this.GetRepository<Tag>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
