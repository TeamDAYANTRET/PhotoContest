using PhotoContest.Data.Contracts;
using PhotoContest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Tests.Mocks
{
    class PhotoContestDataMock : IPhotoContestData
    {
        private RepositoryMock<ApplicationUser> users = new RepositoryMock<ApplicationUser>();
        private RepositoryMock<Category> category = new RepositoryMock<Category>();
        private RepositoryMock<Comment> comment = new RepositoryMock<Comment>();
        private RepositoryMock<Contest> contest = new RepositoryMock<Contest>();
        private RepositoryMock<Image> image = new RepositoryMock<Image>();
        private RepositoryMock<Notification> notification = new RepositoryMock<Notification>();
        private RepositoryMock<Prize> prize = new RepositoryMock<Prize>();
        private RepositoryMock<Tag> tag = new RepositoryMock<Tag>();
        private RepositoryMock<Message> message = new RepositoryMock<Message>();
        public bool IsSaveCalled { get; set; }

        public IRepository<ApplicationUser> Users
        {
            get { return this.users; }
        }

        public IRepository<Category> Categories
        {
            get { return this.category; }
        }

        public IRepository<Comment> Comments
        {
            get { return this.comment; }
        }

        public IRepository<Contest> Contests
        {
            get { return this.contest; }
        }

        public IRepository<Image> Images
        {
            get { return this.image; }
        }

        public IRepository<Notification> Notifications
        {
            get { return this.notification; }
        }

        public IRepository<Prize> Prizes
        {
            get { return this.prize; }
        }

        public IRepository<Tag> Tags
        {
            get { return this.tag; }
        }

        public IRepository<Message> Messages
        {
            get { return this.message; }
        }

        public int SaveChanges()
        {
            return Convert.ToInt32(this.IsSaveCalled = true);
        }

        public Task<int> SaveChangesAsync()
        {
            return Task<int>.Run(() => Convert.ToInt32(this.IsSaveCalled = true));
        }
    }
}
