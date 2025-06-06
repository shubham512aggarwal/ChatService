namespace ChatService.DTOs
{
    public class AddParticipantDTO
    {
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
    }
}
