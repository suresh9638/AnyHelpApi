using anyhelp.Service.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NET5SignalR.Models
{
    public class NotificationHub : Hub
    {

        private readonly IHubContext<NotificationHub> _hub;

        public NotificationHub(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendNotification(NotificationSignalR notificationSignalR)
        {

            await _hub.Clients.All.SendAsync("OnNotification", notificationSignalR);
        }
    }
}