namespace ChatService.DTOs
{
    public class GetChatRoomDTO
    {
        public Guid ChatRoomId { get; set; }
        public string ChatRoomName { get; set; } = string.Empty;
        public List<Guid> ParticipantIds { get; set; } = new List<Guid>();
        public DateTime CreatedAt { get; set; }
    }
}
