using System;
using System.Linq;
using System.Web.Mvc;

using PhotoContest.Data.Contracts;
using PhotoContest.Models.Enumerations;
using Microsoft.AspNet.SignalR;
using PhotoContest.Web.Hubs;
using PhotoContest.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;
using PhotoContest.Models;
using System.Collections.Generic;
using PhotoContest.Web.Models.ViewModels;
using AutoMapper;
using PhotoContest.Web.Models.BindingModels;
using System.Threading.Tasks;
using System.IO;
using RazorEngine;
using PhotoContest.Data;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    public class NotificationsController : BaseAdminController
    {
        public NotificationsController() : this(new PhotoContestData())
        {
        }

        public NotificationsController(IPhotoContestData data)
            : base(data)
        {
        }

        // GET: Admin/Notifications
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> SendNotification(NotificationBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList(); 

                return new HttpStatusCodeResult(400, "Invalid input. " + string.Join("\n", errorList));
            }

            NotificationType type = (NotificationType)model.NotificationType;

            var notification = new Notification()
            {
                Content = model.Content,
                Type = type,
                SendOn = DateTime.Now
            };

            var template = System.IO.File.ReadAllText(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"Views\RazorEngine\_PagedNotificationViewModel.cshtml"));
            var renderedPartialView = Razor.Parse(template, 
                Mapper.Map<Notification, PagedNotificationViewModel>(notification));

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            hubContext.Clients.All.receiveNotification(renderedPartialView);

            hubContext.Clients.All.receiveNotificationCount();

            var allUsers = this.Data.Users.All();

            foreach (var user in allUsers)
            {
                var n = new Notification()
                {
                    Content = model.Content,
                    Type = type,
                    SendOn = DateTime.Now,
                    User = user
                };

                user.Notifications.Add(n);
            }

            this.Data.SaveChanges();  

            return new HttpStatusCodeResult(200, "Notification sent succssesfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnAllNotifications()
        {
            var userID = User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userID);

            var pagedNotifications = user.Notifications.AsQueryable()
                .Select(PagedNotificationViewModel.Create).OrderByDescending(n => n.SendOn).ToList();    

            return PartialView("_PagedNotifications", pagedNotifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Read(int id)
        {
            var notification = this.Data.Notifications.GetById(id);
            notification.IsRead = true;
            this.Data.SaveChangesAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int CountNotifications()
        {
            var user = this.Data.Users.GetById(User.Identity.GetUserId());
            int num = user.Notifications.Count();

            return num;
        }
    }
}