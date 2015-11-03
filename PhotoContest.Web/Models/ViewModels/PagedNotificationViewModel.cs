using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using PhotoContest.Models;
using PhotoContest.Models.Enumerations;

namespace PhotoContest.Web.Models.ViewModels
{
    public class PagedNotificationViewModel
    {
        public static Expression<Func<Notification, PagedNotificationViewModel>> Create
        {
            get
            {
                return n => new PagedNotificationViewModel
                {
                    Id = n.Id,
                    Content = n.Content,
                    SendOn = n.SendOn,
                    UserId = n.UserId,
                    Type = n.Type,
                    IsRead = n.IsRead
                };
            }
        }

        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Send date is required")]
        public DateTime SendOn { get; set; }

        [Required(ErrorMessage = "Notification Recipient is required.")]
        public string UserId { get; set; }

        [EnumDataType(typeof(NotificationType))]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }
    }
}