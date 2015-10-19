namespace PhotoContest.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using System.IO;
    using AppLimit.CloudComputing.SharpBox;
    using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
    using System.Diagnostics;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Text.RegularExpressions;
    using Models.Enumerations;
    using Dropbox.Api;

    public sealed class Configuration : DbMigrationsConfiguration<PhotoContest.Data.PhotoContestDbContext>
    {
        private readonly List<string> solPathParts = new List<string>();
        private const string App_key = "ss5yuuo0kiu7hro";
        private const string App_secret = "5kj2etxgkhid4oz";

        public Configuration()
        {
            solPathParts = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\\", @"\").Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PhotoContest.Data.PhotoContestDbContext context)
        {
            if (!context.Users.Any())
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var userHolder = new Queue<ApplicationUser>();

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
                            ProfilePic = userData[3] == "ProfilePicture" ? null : userData[3]
                        };

                        userHolder.Enqueue(user);
                        manager.Create(user, "Temp_123" + i++);

                        line = reader.ReadLine();
                    }

                    context.SaveChanges();
                }

                if (!context.Messages.Any())
                {
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
                                Title = "Msg Title " + i,
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
            }


            if (!context.Categories.Any())
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

            var allUsers = context.Users.OrderByDescending(u => u.JoinedAt).ToList();
            string[] contestNames = { "Animal Memes", "Bridges", "Right Place at the Right Time", "Snap Shot of Life", "Space", "The Art of Nature", "Wired Ideas" };

            if (!context.Contests.Any())
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
                                newContest.EndTime = DateTime.Now.AddDays(31);
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
                                newContest.EndTime = DateTime.Now.AddDays(10);
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
                                newContest.EndTime = DateTime.Now.AddDays(20);
                                newContest.Participants = allUsers.GetRange(allUsers.Count() - 3, 2);
                            }
                            break;
                    };

                    context.Contests.Add(newContest);
                };

                context.SaveChanges();
            }

            if (context.Prizes.Any())
            {
                for (int i = 0; i < 9; i++)
                {
                    var prize = new Prize();
                    string cName = null;

                    if (i < 3)
                    {
                        cName = contestNames[0];
                        prize.Name = cName + " Winner " + (i + 1);
                        prize.ForPlace = i+1;
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

                    context.Prizes.AddOrUpdate(prize);
                }

                context.SaveChanges();
            }

            if (!context.Images.Any())
            {
                string[] foldersInDropBox = { "AnimalMemes", "Bridges", "RightPlaceRightTime", "SnapShotOfLife", "Space", "TheArtOfNature", "WiredIdeas" };
                // embedded for App 2 Dropbox - attempt IV
                //using (var dbx = new DropboxClient("8ptNAGIQs1AAAAAAAAAAB4BjIvMC1ziLT5PuwUO7YslQTtn5c4lY-_SodeHv54_q"))
                //{
                //    var full = dbx.Users.GetCurrentAccountAsync().Result;
                //    Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
                //}

                //---------------------- DropNet - attempt III

                //var _client = new DropNetClient("ss5yuuo0kiu7hro", "5kj2etxgkhid4oz");

                //_client.GetToken();

                ////var url = _client.BuildAuthorizeUrl();
                ////var accessToken = _client.GetAccessToken();
                //var _client = new DropNetClient("ss5yuuo0kiu7hro", "5kj2etxgkhid4oz", "cxh29uj95pkk31b9", "qcar568i6kme37m");
                //var metaData = _client.GetMetaData("/AnimalMemes", true, false);

                //------------------ HigLabo - attepmt II
                //OAuthClient ocl = null;
                //HigLabo.Net.AuthorizeInfo ai = null;

                //ocl = DropboxClient.CreateOAuthClient(App_key, App_secret);
                //ai = ocl.GetAuthorizeInfo();

                //string RequestToken = ai.RequestToken;
                //string RequestTokenSecret = ai.RequestTokenSecret;
                //string redirect_url = ai.AuthorizeUrl;

                //AccessTokenInfo t = ocl.GetAccessToken(RequestToken, RequestTokenSecret);

                //string Token = "OAURNfHymnAAAAAAAAABV7BQxyt9KsGpMzmqXaBNLzE7igfmGfN74GB6_Uc9Bmr0";
                //string TokenSecret = ai.RequestTokenSecret;

                //DropboxClient cl = new DropboxClient(App_key, App_secret, Token, TokenSecret);

                //GetMetadataCommand gmc = new GetMetadataCommand();
                //gmc.Root = RootFolder.Dropbox; //Our Dropbox app has access to the root of the user's Dropbox 
                //gmc.Path = "AnimalMemes";

                //Metadata md = cl.GetMetadata(gmc);
                //var s = md.Contents;

                //------------------ Sharp Box Attempt I

                //    CloudStorage dropBoxStorage = new CloudStorage();

                //    var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                //    ICloudStorageAccessToken accessToken = null;

                //    string tokenPath = this.PathBuilder("ExternalLibraries", @"\token.txt");

                //    using (FileStream fs = File.Open(tokenPath, FileMode.Open, FileAccess.Read, FileShare.None))
                //    {
                //        accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                //    }

                //    var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                //   // var publicFolder = dropBoxStorage.GetFolder("/AnimalMemes");

                //    try
                //    {
                //        var publicFolder = dropBoxStorage.GetFolder("/AnimalMemes");
                //        foreach (var fof in publicFolder)
                //        {
                //            ICloudFileSystemEntry fs = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                //            //fsf.GetDataTransferAccessor.
                //            //Uri uri = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fs);
                //            var k = DropBoxStorageProviderTools.
                //            //var a = dropBoxStorage.GetFile(fof.Name, publicFolder);
                //        };

                //        // m_RessourceURI = m_DropBoxStorage.GetFileSystemObjectUrl("/" + RessourceFileName, m_DropBoxStorage.GetFolder("/"));
                //    }
                //    catch (SharpBoxException AnyException)
                //    {
                //        throw AnyException;
                //    }

                //    //foreach (var fof in publicFolder)
                //    //{
                //    //    var fsf = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                //    //    //fsf.GetDataTransferAccessor.
                //    //    Uri uri = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fsf);
                //    //};

                //    dropBoxStorage.Close();
                //}

            }

            if (!context.Notifications.Any())
            {
                foreach (var user in allUsers)
                {
                    var notif = new Notification()
                    {
                        Content = "Notification for " + user.FirstName,
                        SendOn = DateTime.Now,
                        User = user
                    };

                    context.Notifications.Add(notif);
                }

                context.SaveChanges();
            }
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
