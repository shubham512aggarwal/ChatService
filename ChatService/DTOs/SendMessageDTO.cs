namespace ChatService.DTOs
{
    public class SendMessageDTO
    {
        public Guid ChatRoomId { get; set; }
        public Guid ParticipantId { get; set; }
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
