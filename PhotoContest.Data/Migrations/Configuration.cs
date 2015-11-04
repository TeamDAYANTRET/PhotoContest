namespace PhotoContest.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using System.IO;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;

    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Text.RegularExpressions;
    using Models.Enumerations;

    using Models;
    using Helper;
    using System.Threading.Tasks;


    public sealed class Configuration : DbMigrationsConfiguration<PhotoContest.Data.PhotoContestDbContext>
    {
        private readonly List<string> solPathParts = new List<string>();
        private List<ApplicationUser> allUsers;
        private readonly string[] contestNames = { "Animal Memes", "Bridges", "Right Place at the Right Time", "Snap Shot of Life", "Space", "The Art of Nature", "Wired Ideas" };


        public Configuration()
        {
            solPathParts = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\\", @"\").Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PhotoContest.Data.PhotoContestDbContext context)
        {
            if (!context.Users.Any())
                this.SeedUsers(context);

            if (!context.Categories.Any())
                this.SeedCategories(context);

            if (!context.Contests.Any())
                this.SeedContests(context);

            if (!context.Prizes.Any())
                this.SeedPrizes(context);

            if (!context.Tags.Any())
                this.SeedTags(context);

            if (!context.Images.Any())
                this.SeedImages(context);

            if (!context.Notifications.Any())
			this.SeedNotifications(context);
        }

        private void SeedUsers(PhotoContestDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                roleManager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com", FirstName = "Admin", LastName = "Adminov" };

                manager.Create(user, "Temp_123");
                manager.AddToRole(user.Id, "Admin");
            }

            using (var reader = new StreamReader(this.PathBuilder("PhotoContest.Data", @"/Resources/Users.txt")))
            {
                string line = reader.ReadLine();
                int i = 0;

                while (!string.IsNullOrEmpty(line.Trim()))
                {
                    var userData = Regex.Split(line, @"\s+\|\s+");

                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = userData[0] + userData[1],
                        Email = userData[0] + userData[1] + "@somemail.com",
                        FirstName = userData[0],
                        LastName = userData[1],
                        JoinedAt = DateTime.Now,
                        AboutMe = userData[2],
                        ProfilePic = userData[3] == "https://www.dropbox.com/s/225jip7nvs2gwt6/images%20%283%29.jpg?raw=1" ? null : userData[3]
                    };

                    manager.Create(user, "Temp_123" + i++);

                    line = reader.ReadLine();
                }

                context.SaveChanges();
            }

            if (!context.Messages.Any())
                this.SeedMessages(context);

            this.allUsers = context.Users.OrderByDescending(u => u.JoinedAt).ToList();
        }

        private void SeedMessages(PhotoContestDbContext context)
        {
            var userHolder = new Queue<ApplicationUser>(context.Users.ToArray());

            for (int i = 0; i < userHolder.Count; i++)
            {
                var user = userHolder.Dequeue();
                userHolder.Enqueue(user);

                for (int j = 0; j < 4; j++)
                {
                    var recipient = userHolder.Dequeue();
                    userHolder.Enqueue(recipient);

                    var message = new Message()
                    {
                        Title = "Msg Title " + j,
                        Content = "Message to " + recipient.FirstName + " from " + user.FirstName,
                        CreatedAt = DateTime.Now,
                        Sender = user,
                        Recipient = recipient
                    };
                    context.Messages.Add(message);
                }

                context.SaveChanges();
            }
        }

        private void SeedCategories(PhotoContestDbContext context)
        {
            using (var reader = new StreamReader(this.PathBuilder("PhotoContest.Data", @"/Resources/Categories.txt")))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    var categoryNames = Regex.Split(line, @",\s+");
                    for (int i = 0; i < categoryNames.Length; i++)
                    {
                        var category = new Category()
                        {
                            CategoryName = categoryNames[i]
                        };

                        context.Categories.Add(category);
                    }

                    line = reader.ReadLine();
                }

                context.SaveChanges();
            }
        }

        private void SeedContests(PhotoContestDbContext context)
        {
            for (int i = 0; i < contestNames.Length; i++)
            {
                var u = allUsers.Skip(0).Take(1);
                var newContest = new Contest()
                {
                    Title = contestNames[i],
                    Description = "A very intriguing description #" + i,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Owner = allUsers.Skip(i).First()
                };

                switch (i)
                {
                    case 0:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Funny");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.TopNPrizes;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByTime;
                            newContest.ParticipationEndTime = DateTime.Now.AddDays(31);
                            newContest.Participants = allUsers.GetRange(3, 14);
                            newContest.MaxParticipationsCount = 125;
                            newContest.PossibleWinnersCount = 3;
                        }
                        break;
                    case 1:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Architecture");
                            newContest.VotingStrategy = Strategy.Closed;
                            newContest.ParticipationStrategy = Strategy.Closed;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByTime;
                            newContest.ParticipationEndTime = DateTime.Now.AddDays(10);
                            newContest.Participants = allUsers.GetRange(3, 10);
                            newContest.CommitteeMembers = allUsers.GetRange(16, 3);
                        }
                        break;
                    case 2:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Conceptual");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByNumberOfParticipants;
                            newContest.MaxParticipationsCount = 15;
                            newContest.Participants = allUsers.GetRange(4, 15);
                        }
                        break;
                    case 3:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Conceptual");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByNumberOfParticipants;
                            newContest.MaxParticipationsCount = 25;
                        }
                        break;
                    case 4:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Abstract");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByNumberOfParticipants;
                            newContest.MaxParticipationsCount = 25;
                            newContest.Participants = allUsers.GetRange(0, 4);
                        }
                        break;
                    case 5:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "Nature");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByNumberOfParticipants;
                            newContest.MaxParticipationsCount = 25;
                            newContest.Participants = allUsers.GetRange(8, 5);
                        }
                        break;
                    default:
                        {
                            newContest.Category = context.Categories.FirstOrDefault(c => c.CategoryName == "People");
                            newContest.VotingStrategy = Strategy.Open;
                            newContest.ParticipationStrategy = Strategy.Open;
                            newContest.RewardStrategy = RewardStrategy.SingleWinner;
                            newContest.DeadlineStrategy = DeadlineStrategy.ByTime;
                            newContest.ParticipationEndTime = DateTime.Now.AddDays(20);
                            newContest.Participants = allUsers.GetRange(allUsers.Count() - 3, 2);
                        }
                        break;
                };

                context.Contests.Add(newContest);
            };

            context.SaveChanges();
        }

        private void SeedTags(PhotoContestDbContext context)
        {
            for (int i = 0; i < 9; i++)
            {
                var newTag = new Tag()
                {
                    Name = "Tag " + (i + 1)
                };

                context.Tags.Add(newTag);
            }

            context.SaveChanges();
        }

        private void SeedPrizes(PhotoContestDbContext context)
        {
            for (int i = 0; i < 9; i++)
            {
                var prize = new Prize();
                string cName = null;

                if (i < 3)
                {
                    cName = contestNames[0];
                    prize.Name = cName + " Winner " + (i + 1);
                    prize.ForPlace = i + 1;
                    prize.Contest = context.Contests.FirstOrDefault(c => c.Title == cName);
                }
                else
                {
                    cName = contestNames[i - 2];
                    prize.ForPlace = 1;
                    prize.Name = cName + "'s Winner";
                    prize.Contest = context.Contests.FirstOrDefault(c => c.Title == cName);
                }

                prize.Description = "Lorem ipsum dolor sit amet. " + i;

                context.Prizes.Add(prize);
            }

            context.SaveChanges();
        }

        private void SeedImages(PhotoContestDbContext context)
        {
            string[] foldersInDropBox = { "AnimalMemes", "Bridges", "RightPlaceRightTime", "SnapShotOfLife", "Space", "TheArtOfNature", "WiredIdeas" };

            var task = Task.Run(() => DropboxManager.GetAllSharedLinks());
            task.Wait();

            Dictionary<string, List<Tuple<string, string>>> photoSeedData = task.Result;

            for (var i = 0; i < foldersInDropBox.Length; i++)
            {
                if (!photoSeedData.ContainsKey("/" + foldersInDropBox[i]))
                    continue;

                List<Tuple<string, string>> imgInContest = photoSeedData["/" + foldersInDropBox[i]];
                int counter = 1;

                Queue<ApplicationUser> userHolder = new Queue<ApplicationUser>(allUsers);
                Queue<Tag> tagHolder = new Queue<Tag>(context.Tags.ToArray());

                Random rnd = new Random();

                foreach (var img in imgInContest)
                {
                    var user = userHolder.Dequeue();
                    userHolder.Enqueue(user);

                    string contestName = contestNames[i];

                    var newImg = new Image()
                    {
                        Title = "Img " + (counter++),
                        ImagePath = img.Item1,
                        Description = img.Item2 + " - Some inspiring description",
                        CreatedOn = DateTime.Now,
                        User = user,
                        Contest = context.Contests.FirstOrDefault(c => c.Title == contestName),
                        Prize = null
                    };

                    context.Images.Add(newImg);
                    context.SaveChanges();

                    for (int j = 0; j < rnd.Next(1, 15); j++)
                    {
                        user = userHolder.Dequeue();
                        userHolder.Enqueue(user);

                        if (j % 3 == 0)
                            user.FavoritePictures.Add(newImg);

                        if (j % 2 == 0)
                            newImg.Votes.Add(user);

                        var comment = new Comment()
                        {
                            Picture = newImg,
                            Author = user,
                            Content = "Comment " + j,
                            CreatedAt = DateTime.Now
                        };

                        context.Comments.Add(comment);

                        for (int k = 0; k < rnd.Next(1, 4); k++)
                        {
                            var tag = tagHolder.Dequeue();
                            tagHolder.Enqueue(tag);

                            newImg.Tags.Add(tag);
                        }
                    }

                    context.SaveChanges();
                }
            }

        }

        private void SeedNotifications(PhotoContestDbContext context)
        {
            var rnd = new Random();
            var notifTypes = (Enum.GetValues(typeof(NotificationType))).Cast<NotificationType>().ToArray();

            foreach (var user in allUsers)
            {
                var notif = new Notification()
                {
                    Content = "Notification for " + user.FirstName,
                    SendOn = DateTime.Now,
                    User = user,
                    Type = notifTypes[rnd.Next(0, notifTypes.Length)]
                };

                context.Notifications.Add(notif);
            }

            context.SaveChanges();
        }

        private string PathBuilder(string project, string appending)
        {
            var pathParts = this.solPathParts;
            pathParts[pathParts.Count() - 1] = project;
            string path = string.Join("\\", pathParts) + appending;

            return path;
        }
    }
}
