namespace ChatService.Models
{
    public class ChatUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();
    }
}
