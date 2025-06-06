namespace ChatService.DTOs
{
    public class RemoveChatRoomDTO
    {
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public RemoveChatRoomDTO(Guid chatRoomId, Guid userId)
        {
            ChatRoomId = chatRoomId;
            UserId = userId;
        }
    }
}
