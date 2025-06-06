namespace ChatService.DTOs
{
    public class CreateGroupChatDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> Participants { get; set; } = new List<Guid>();
        public Guid CreatorUserId { get; set; }
        public bool isGroup { get; set; } = false;
        public Guid AdminGuid { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
