using DLARS.Constants;
using Microsoft.AspNetCore.SignalR;

namespace DLARS.Hubs
{
    public class RequestHub : Hub
    {
        public async Task JoinStatusGroup(string statusId)
        {
            string groupName = $"status-{statusId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveStatusGroup(string statusId)
        {
            string groupName = $"status-{statusId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task BroadcastHangfireTriggered()
        {
            await Clients.All.SendAsync(SignalREvents.HangfireTriggered);
        }

    }
}
