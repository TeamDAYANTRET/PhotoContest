using System;
using System.Linq;
using System.Web.Mvc;

using PhotoContest.Data.Contracts;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    public class NotificationsAdminController : BaseAdminController
    {
        // GET: Admin/NotificationsAdmin
        public NotificationsAdminController(IPhotoContestData ctx) : base(ctx)
        {
        }
        // GET: Admin/Notifications
        public ActionResult Index()
        {
            return View();
        }

        //[ValidateAntiForgeryToken]
        //[HttpPost, ValidateInput(false)]
        //public ActionResult SendNotification(string type, string notification)
        //{
        //    NotificationKind notificationKind = NotificationKind.CustomAdminNotification;
        //    Enum.TryParse(type, out notificationKind);

        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
        //    hubContext.Clients.All.receiveNotification(notificationKind.ToString(), notification);

        //    var adminId = User.Identity.GetUserId();
        //    User admin = this.Data.Users.Find(adminId);

        //    var allUsers = this.Data.Users.All();

        //    foreach (var user in allUsers)
        //    {
        //        var newNotification = new Notification()
        //        {
        //            Content = notification,
        //            NotificationKind = notificationKind,
        //            Date = DateTime.Now,
        //            SendTo = user,
        //            TriggeredBy = admin
        //        };

        //        this.Data.Notifications.Add(newNotification);
        //    }

        //    this.Data.SaveChanges();

        //    return this.Content("Notification sent.<br />");
        //}
    }
}