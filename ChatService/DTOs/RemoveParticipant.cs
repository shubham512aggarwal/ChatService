namespace ChatService.DTOs
{
    public class RemoveParticipant
    {
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
    }
}
