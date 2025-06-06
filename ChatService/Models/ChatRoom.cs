namespace ChatService.Models
{
    public class ChatRoom
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsGroup { get; set; } = false;
        public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
