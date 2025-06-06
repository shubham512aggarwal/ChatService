namespace ChatService.DTOs
{
    public class CreateChatRoomDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsGroup { get; set; } = false;
        public List<Guid> Participants { get; set; } = new List<Guid>();
        public Guid AdminUserId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
