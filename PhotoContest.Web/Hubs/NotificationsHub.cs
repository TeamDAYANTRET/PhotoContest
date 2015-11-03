namespace PhotoContest.Web.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("notificatoins")]
    public class NotificationsHub : Hub
    {
        public void SendNotification(string template)
        {
            this.Clients.Others.receiveNotification(template);
        }

        public void SendNotification()
        {
            this.Clients.Others.receiveNotificationCount();
        }
    }
}