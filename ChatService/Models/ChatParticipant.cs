namespace ChatService.Models
{
    public class ChatParticipant
    {
        public Guid ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsAdmin{ get; set; } = false;

    }
}
