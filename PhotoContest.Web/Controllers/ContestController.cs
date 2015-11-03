using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using PhotoContest.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PhotoContest.Web.Models.ViewModels;
using System.Linq.Expressions;
using PhotoContest.Models;
using Microsoft.AspNet.Identity;

namespace PhotoContest.Web.Controllers
{
    public class ContestController : BaseController
    {
        public ContestController() : this(new PhotoContestData())
        {
        }

        public ContestController(IPhotoContestData data)
            : base(data)
        {
        }

        // GET: Contest
        public ActionResult Index()
        {
            var ongoingContests = this.Data.Contests.All().Where(c => c.State == TypeOfEnding.Ongoing)
                .OrderByDescending(c => c.ParticipationEndTime)
                .Select(OngoingContestBasicInfoViewModel.Create).ToList();
            var endedContests = this.Data.Contests.All().Where(c => c.State != TypeOfEnding.Ongoing)
                .OrderByDescending(c => c.ContestEndTime)
                .Select(EndedContestBasicInfoViewModel.Create).ToList();

            var allContests = new IndexPageViewModel()
            {
                OngoingContests = ongoingContests,
                EndedContests = endedContests
            };

            return View("Index", allContests);
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().Where(c => c.Id == id).Select(c => new ContestDetailsViewModel()
            {
                Id = c.Id,
                EndTime = c.ContestEndTime,
                MaxParticipationsCount = c.MaxParticipationsCount,
                ParticipationStrategy = c.ParticipationStrategy,
                Title = c.Title,
                VotingStrategy = c.VotingStrategy,
                State = c.State,
                ParticipationEndTime = c.ParticipationEndTime ?? default(DateTime),
                ParticipationsCount = c.Pictures.Select(p => p.User.Id).Distinct().Count(),
                OwnerId = c.OwnerId,
                CanParticipate = c.Participants.Any(u => u.Id == userId) || c.ParticipationStrategy == Strategy.Open,
                CanVote = c.CommitteeMembers.Any(u => u.Id == userId) || c.VotingStrategy == Strategy.Open,
                Picuters = c.Pictures.Select(p => new PagedImageViewModel()
                {
                    Id = p.Id,
                    ImagePath = p.ImagePath,
                    VotesCount = p.Votes.Count(),
                    AuthorUsername = p.User.UserName
                })
            }).FirstOrDefaultAsync();

            return View(contest);
        }

        [Authorize]
        public async Task<ActionResult> GetComiteeMembers(int id)
        {
            var userId = User.Identity.GetUserId();
            var contestComittee = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId).Select(c => new ContestMembersViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Users = c.CommitteeMembers.Select(u => new UserBasicViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName
                })
            }).FirstOrDefaultAsync();

            if (contestComittee == null)
            {
                return RedirectToAction("contests", "User");
            }

            ViewBag.msg = TempData["msg"];
            return View("commitee", contestComittee);
        }

        [Authorize]
        public ActionResult RemoveParticipant(int id, string userId)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id && c.State == TypeOfEnding.Ongoing).FirstOrDefault();
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var ownerId = User.Identity.GetUserId();
            if (ownerId != contest.OwnerId)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
            contest.Participants.Remove(user);
            this.Data.SaveChanges();

            return RedirectToAction("GetParticipants", "Contest", new { id = id });
        }

        [Authorize]
        public ActionResult RemoveCommiteeMember(int id, string userId)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id && c.State == TypeOfEnding.Ongoing).FirstOrDefault();
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var ownerId = User.Identity.GetUserId();
            if (ownerId != contest.OwnerId)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
            contest.CommitteeMembers.Remove(user);
            this.Data.SaveChanges();

            return RedirectToAction("GetComiteeMembers", "Contest", new { id = id });
        }

        [Authorize]
        public async Task<ActionResult> GetParticipants(int id)
        {
            var userId = User.Identity.GetUserId();
            var contestComittee = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId).Select(c => new ContestMembersViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Users = c.Participants.Select(u => new UserBasicViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName
                })
            }).FirstOrDefaultAsync();

            if (contestComittee == null)
            {
                return RedirectToAction("contests", "User");
            }

            ViewBag.msg = TempData["msg"];
            return View("participants", contestComittee);
        }

        [Authorize]
        public ActionResult Add()
        {
            return View("Add");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Post(ContestModel contest)
        {
            if (!ModelState.IsValid)
            {
                return View("add", contest);
            }

            if (contest.ParticipationEndTime == null && contest.MaxParticipationsCount == null)
            {
                ModelState.AddModelError("", "You have to choose end date or max pariticpations count");
                return View("add", contest);
            }

            var userId = User.Identity.GetUserId();
            var newContest = new Contest()
            {
                Title = contest.Title,
                Description = contest.Description,
                CreatedOn = DateTime.Now,
                DeadlineStrategy = contest.DeadlineStrategy,
                MaxParticipationsCount = contest.MaxParticipationsCount,
                OwnerId = userId,
                ParticipationEndTime = contest.ParticipationEndTime,
                ParticipationStrategy = contest.ParticipationStrategy,
                PossibleWinnersCount = contest.PossibleWinnersCount,
                State = TypeOfEnding.Ongoing,
                VotingStrategy = contest.VotingStrategy,
                LastUpdated = DateTime.Now
            };

            this.Data.Contests.Add(newContest);
            this.Data.SaveChanges();

            return RedirectToAction("GetUserContests", "User", null);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId && c.State == TypeOfEnding.Ongoing).Select(ContestModel.Create).FirstOrDefaultAsync();
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            return View(contest);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Put(int id, ContestModel contest)
        {
            var userId = User.Identity.GetUserId();
            var contestDb = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId && c.State == TypeOfEnding.Ongoing).FirstOrDefaultAsync();
            if (contestDb == null)
            {
                ModelState.AddModelError("", "This contest does not exist");
                return View("add", contest);
            }

            contestDb.Title = contest.Title;
            contestDb.Description = contest.Description;
            contestDb.DeadlineStrategy = contest.DeadlineStrategy;
            contestDb.LastUpdated = DateTime.Now;
            contestDb.MaxParticipationsCount = contest.MaxParticipationsCount;
            contestDb.ParticipationEndTime = contest.ParticipationEndTime;
            contestDb.ParticipationStrategy = contest.ParticipationStrategy;
            contestDb.PossibleWinnersCount = contest.PossibleWinnersCount;
            contestDb.VotingStrategy = contest.VotingStrategy;

            await this.Data.SaveChangesAsync();
            TempData["msg"] = "The contest was edited";
            return RedirectToAction("GetUserContests", "User");
        }

        [Authorize]
        public ActionResult Dismiss(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == id && c.OwnerId == userId && c.State == TypeOfEnding.Ongoing);
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            contest.State = TypeOfEnding.Dissmissed;
            contest.ContestEndTime = DateTime.Now;
            this.Data.SaveChanges();
            return RedirectToAction("Index", "Contest");
        }

        [Authorize]
        public async Task<ActionResult> Finalize(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId && c.State == TypeOfEnding.Ongoing);
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            contest.State = TypeOfEnding.Finalized;
            contest.ContestEndTime = DateTime.Now;
            // get rewards
            await this.Data.SaveChangesAsync();
            return RedirectToAction("index", "Contest");
        }

        [Authorize]
        public ActionResult AddParticipant(int id, string username)
        {
            var ownerId = User.Identity.GetUserId();
            var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == id && c.OwnerId == ownerId && c.State == TypeOfEnding.Ongoing);
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var participant = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            if (participant == null)
            {
                TempData["error"] = "This user is no longer exist";
                return RedirectToAction("GetParticipants", "Contest", new { id = id });
            }

            contest.Participants.Add(participant);
            this.Data.SaveChanges();
            TempData["msg"] = "User added to participants";
            return RedirectToAction("GetParticipants", "Contest", new { id = id });
        }

        [Authorize]
        public async Task<ActionResult> AddCommitee(int id, string username)
        {
            var ownerId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == ownerId && c.State == TypeOfEnding.Ongoing);
            if (contest == null)
            {
                return RedirectToAction("GetUserContests", "User");
            }

            var participant = await this.Data.Users.All().FirstOrDefaultAsync(u => u.UserName == username);
            if (participant == null)
            {
                TempData["error"] = "This user is no longer exist";
                return RedirectToAction("GetComiteeMembers", "Contest", new { id = id });
            }

            contest.CommitteeMembers.Add(participant);
            await this.Data.SaveChangesAsync();
            TempData["msg"] = "User added to commitee";
            return RedirectToAction("GetComiteeMembers", "Contest", new { id = id });
        }
    }
}