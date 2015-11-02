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
        public async Task<ActionResult> Index()
        {
            var ongoingContests = await this.getContestsBasicInfo(c => c.State == TypeOfEnding.Ongoing).ToListAsync();
            var endedContests = await this.getContestsBasicInfo(c => c.State != TypeOfEnding.Ongoing).ToListAsync();

            var allContests = new IndexPageViewModel()
            {
                OngoingContests = ongoingContests,
                EndedContests = endedContests
            };

            return View(allContests);
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var isInRole = User.IsInRole("Admin");
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

            return View("commitee", contestComittee);
        }

        [Authorize]
        public ActionResult RemoveParticipant(int id, string userId)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id).FirstOrDefault();
            if (contest == null)
            {
                return RedirectToRoute("Index");
            }

            var ownerId = User.Identity.GetUserId();
            if (ownerId != contest.OwnerId)
            {
                return RedirectToRoute("Index");
            }

            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
            contest.Participants.Remove(user);
            this.Data.SaveChanges();

            return RedirectToAction("GetParticipants", "Contest", new { id = id });
        }

        [Authorize]
        public ActionResult RemoveCommiteeMember(int id, string userId)
        {
            var contest = this.Data.Contests.All().Where(c => c.Id == id).FirstOrDefault();
            if (contest == null)
            {
                return RedirectToRoute("Index");
            }

            var ownerId = User.Identity.GetUserId();
            if (ownerId != contest.OwnerId)
            {
                return RedirectToRoute("Index");
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

            return View("participants", contestComittee);
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Post(ContestModel contest)
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
            await this.Data.SaveChangesAsync();

            return RedirectToAction("GetUserContests", "User", null);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId).Select(ContestModel.Create).FirstOrDefaultAsync();
            return View(contest);
        }

        [Authorize]
        public async Task<ActionResult> Put(int id, ContestModel contest)
        {
            var userId = User.Identity.GetUserId();
            var contestDb = await this.Data.Contests.All().Where(c => c.Id == id && c.OwnerId == userId).FirstOrDefaultAsync();
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
            return RedirectToAction("index", "Contests");
        }

        [Authorize]
        public async Task<ActionResult> Dismiss(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);
            if (contest == null)
            {
                // error message n redirect
            }

            contest.State = TypeOfEnding.Dissmissed;
            await this.Data.SaveChangesAsync();
            return RedirectToAction("index", "Contest");
        }

        [Authorize]
        public async Task<ActionResult> Finalize(int id)
        {
            var userId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);
            if (contest == null)
            {
                // error message n redirect
            }

            contest.State = TypeOfEnding.Finalized;
            // get rewards
            await this.Data.SaveChangesAsync();
            return RedirectToAction("index", "Contest");
        }

        [Authorize]
        public async Task<ActionResult> AddParticipant(int contestId, string userId)
        {
            var ownerId = User.Identity.GetUserId();
            var contest = await this.Data.Contests.All().FirstOrDefaultAsync(c => c.Id == contestId && c.OwnerId == ownerId);
            if (contest == null)
            {
                // error message n redirect
            }

            var participant = await this.Data.Users.All().FirstOrDefaultAsync(u=>u.Id==userId);
            if (participant == null)
            {

            }
            contest.Participants.Add(participant);
            await this.Data.SaveChangesAsync();
            return RedirectToAction("index", "Contest");
        }

        private IQueryable<ContestBasicInfoViewModel> getContestsBasicInfo(Expression<Func<Contest, bool>> wherePredicate)
        {
            return this.Data.Contests.All().Where(wherePredicate).OrderByDescending(c => c.ContestEndTime).ThenByDescending(c => c.ParticipationEndTime).Select(ContestBasicInfoViewModel.Create);
        }
    }
}