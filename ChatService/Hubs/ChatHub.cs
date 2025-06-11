using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToGroup(string chatRoomId, string userId, string message)
        {
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task JoinChatRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
            await Clients.GroupExcept(chatRoomId, Context.ConnectionId).SendAsync("UserJoined", Context.UserIdentifier ?? Context.ConnectionId);
        }

        public async Task LeaveGroup(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            await Clients.GroupExcept(chatRoomId, Context.ConnectionId).SendAsync("UserLeft", Context.UserIdentifier ?? Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
