using System.Threading.Tasks.Dataflow;
using static ChatService.Helpers.EnumHelper;

namespace ChatService.DTOs
{
    public class RecieveMessageDTO
    {
        public Guid MessageId { get; set; }
        public Guid ChatRoomId { get; set; }
        public string SenderName{ get; set; }
        public string MessageContent { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageStatus Status { get; set; }
    }
}
