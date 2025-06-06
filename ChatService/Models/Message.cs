using static ChatService.Helpers.EnumHelper;

namespace ChatService.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public  MessageStatus Status { get; set; }
    }
}
