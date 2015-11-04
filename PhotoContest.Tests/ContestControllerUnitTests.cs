using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoContest.Web.Controllers;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Web;
using PhotoContest.Models;
using PhotoContest.Models.Enumerations;
using PhotoContest.Web.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using PhotoContest.Tests.Mocks;
using PhotoContest.Tests.Mocks.Identity;

namespace PhotoContest.Tests
{
    [TestClass]
    public class ContestControllerUnitTests
    {
        [TestMethod]
        public void TestIndexAction_ReturnIndexView()
        {
            var identity = new GenericIdentity("as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(new PhotoContestDataMock());
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestIndexAction_ReturnIndexPageViewModel()
        {
            var data = new PhotoContestDataMock();
            var contest = data.Contests;
            contest.Add(new Contest()
            {
                Id = 0,
                State = TypeOfEnding.Ongoing,
                Title = "test1",
                VotingStrategy = Strategy.Open,
                ParticipationEndTime = DateTime.Now.AddDays(10)
            });
            contest.Add(new Contest()
            {
                Id = 1,
                State = TypeOfEnding.Dissmissed,
                Title = "test2",
                VotingStrategy = Strategy.Open,
                ContestEndTime = new DateTime(2010, 10, 10)
            }); contest.Add(new Contest()
            {
                Id = 2,
                State = TypeOfEnding.Finalized,
                Title = "test3",
                VotingStrategy = Strategy.Open,
                ContestEndTime = new DateTime(2011, 10, 10)
            }); contest.Add(new Contest()
            {
                Id = 3,
                State = TypeOfEnding.Ongoing,
                Title = "test4",
                VotingStrategy = Strategy.Open,
                ParticipationEndTime = DateTime.Now.AddDays(12)
            }); contest.Add(new Contest()
            {
                Id = 4,
                State = TypeOfEnding.Ongoing,
                Title = "test5",
                VotingStrategy = Strategy.Open,
                ParticipationEndTime = DateTime.Now.AddDays(13)
            });
            int index = 0;

            var identity = new GenericIdentity("as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Index() as ViewResult;
            var contestResult = (IndexPageViewModel)result.ViewData.Model;

            foreach (var c in contest.All().Where(c => c.State == TypeOfEnding.Ongoing).OrderByDescending(c => c.ParticipationEndTime))
            {
                Assert.AreEqual(c.Title, contestResult.OngoingContests.Skip(index).Take(1).First().Title);
                index++;
            }

            index = 0;
            foreach (var c in contest.All().Where(c => c.State != TypeOfEnding.Ongoing).OrderByDescending(c => c.ContestEndTime))
            {
                Assert.AreEqual(c.Title, contestResult.EndedContests.Skip(index).Take(1).First().Title);
                index++;
            }
        }

        [TestMethod]
        public void TestAddAction_ReturnAddView()
        {
            var controller = new ContestController(new PhotoContestDataMock());
            var result = controller.Add() as ViewResult;
            Assert.AreEqual("Add", result.ViewName);
        }

        [TestMethod]
        public void TestPostAction_ShouldRedirectToGetUserContests()
        {
            var contest = new ContestModel()
            {
                Description = "test",
                PossibleWinnersCount = 2,
                Title = "testov",
                MaxParticipationsCount = 10
            };

            var identity = new GenericIdentity("as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(new PhotoContestDataMock());
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.Post(contest);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Prize", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestPostAction_InvalidModelState_ShouldReturnAddView()
        {
            var contest = new ContestModel()
            {
                PossibleWinnersCount = 2,
                MaxParticipationsCount = 10
            };

            var identity = new GenericIdentity("as");
            var controller = new ContestController(new PhotoContestDataMock());
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controller.ModelState.AddModelError("test", "test error");
            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Post(contest) as ViewResult;
            Assert.AreEqual("add", result.ViewName);
        }

        [TestMethod]
        public void TestPostAction_InvalidEndCondition_ShouldReturnAddView()
        {
            var contest = new ContestModel()
            {
                PossibleWinnersCount = 2
            };

            var identity = new GenericIdentity("as");
            var controller = new ContestController(new PhotoContestDataMock());
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Post(contest) as ViewResult;
            Assert.AreEqual("add", result.ViewName);
        }

        [TestMethod]
        public void TestPostAction_ValidModel_ShouldAddTheContest()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contest = new ContestModel()
            {
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open
            };

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Post(contest) as ViewResult;
            Assert.IsTrue(data.IsSaveCalled);
            Assert.AreEqual(contest.Title, data.Contests.All().First().Title);
        }

        [TestMethod]
        public void TestDismissAction_ShouldRedirectToIndex()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.Dismiss(0);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Contest", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestDismissAction_ShouldDismissTheContest()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.Dismiss(0) as ViewResult;
            Assert.AreEqual(TypeOfEnding.Dissmissed, data.Contests.All().First().State);
            Assert.IsTrue(data.IsSaveCalled);
        }

        [TestMethod]
        public void TestDismissAction_ContestNotExist_ShouldRedirectToGetUserContests()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.Dismiss(0);
            Assert.AreEqual("GetUserContests", result.RouteValues["action"]);
            Assert.AreEqual("User", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAddParticipantAction_ValidData_ShouldRedirectToGetParticipants()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            users.Add(new ApplicationUser()
            {
                Id = "1",
                Email = "a111s@as.as",
                UserName = "a111s@as.as",
                FirstName = "a111s@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.AddParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetParticipants", result.RouteValues["action"]);
            Assert.AreEqual("Contest", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAddParticipantAction_ValidData_ShouldAddTheParticipant()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            users.Add(new ApplicationUser()
            {
                Id = "1",
                Email = "a111s@as.as",
                UserName = "a111s@as.as",
                FirstName = "a111s@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = controller.AddParticipant(0, "a111s@as.as") as ViewResult;
            Assert.IsTrue(data.IsSaveCalled);
            Assert.AreEqual(1, data.Contests.All().First().Participants.Count());
        }

        [TestMethod]
        public void TestAddParticipantAction_ParticipantNotExist_ShouldRedirectToGetParticipants()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.AddParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetParticipants", result.RouteValues["action"]);
            Assert.AreEqual("Contest", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestAddParticipantAction_ContestNotExist_ShouldRedirectToGetUserContests()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.AddParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetUserContests", result.RouteValues["action"]);
            Assert.AreEqual("User", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRemoveParticipantAction_ContestNotExist_ShouldRedirectToGetUserContests()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.RemoveParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetUserContests", result.RouteValues["action"]);
            Assert.AreEqual("User", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRemoveParticipantAction_ContestExist_ShouldRedirectToGetParticipants()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.RemoveParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetParticipants", result.RouteValues["action"]);
            Assert.AreEqual("Contest", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRemoveParticipantAction_ContestWrongOwner_ShouldRedirectToGetUserContests()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = "1"
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.RemoveParticipant(0, "a111s@as.as");
            Assert.AreEqual("GetUserContests", result.RouteValues["action"]);
            Assert.AreEqual("User", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestRemoveParticipantAction_ContestExist_ShouldRemoveTheParticipantFromContest()
        {
            var data = new PhotoContestDataMock();
            var users = data.Users;
            users.Add(new ApplicationUser()
            {
                Id = "0",
                Email = "as@as.as",
                UserName = "as@as.as",
                FirstName = "as@as.as"
            });
            users.Add(new ApplicationUser()
            {
                Id = "1",
                Email = "a1s@as.as",
                UserName = "a1s@as.as",
                FirstName = "a1s@as.as"
            });
            var contests = data.Contests;
            contests.Add(new Contest()
            {
                Id = 0,
                Title = "test",
                Description = "test",
                DeadlineStrategy = DeadlineStrategy.ByTime,
                MaxParticipationsCount = 5,
                ParticipationEndTime = DateTime.Now.AddDays(10),
                ParticipationStrategy = Strategy.Open,
                VotingStrategy = Strategy.Open,
                State = TypeOfEnding.Ongoing,
                OwnerId = users.All().First().Id,
                Participants =  data.Users.All().Where(u => u.Id == "1").ToList()
            });

            var identity = new GenericIdentity("as@as.as");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "0"));
            var controller = new ContestController(data);
            var controllerContext = new TestableControllerContext();
            var principal = new GenericPrincipal(identity, null);
            var testableHttpContext = new TestableHttpContext
            {
                User = principal
            };

            controllerContext.HttpContext = testableHttpContext;
            controller.ControllerContext = controllerContext;
            var result = (RedirectToRouteResult)controller.RemoveParticipant(0, "1");
            Assert.AreEqual(0, data.Contests.All().First().Participants.Count());
        }
    }
}
