using ChatService.DTOs;
using ChatService.Hubs;
using ChatService.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageManager _messageManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(MessageManager messageManager, IHubContext<ChatHub> hubContext)
        {
            _messageManager = messageManager;
            _hubContext = hubContext;
        }

        [HttpPost("sendMessage")]
        public async Task<bool> SendMessageAsync([FromBody] SendMessageDTO messageDTO)
        {
            if (messageDTO == null)
            {
                return false;
            }
            bool res = await _messageManager.SendMessageAsync(messageDTO);
            if (res)
            {
                // Notify connected clients about the new message
                await _hubContext.Clients.Group(messageDTO.ChatRoomId.ToString())
                    .SendAsync("ReceiveMessage", messageDTO.ParticipantId, messageDTO.MessageContent);
                return true;
            }
            return false;
        }

        [HttpGet("getMessages/{chatRoomId}")]
        public async Task<IActionResult> GetMessagesByChatRoomIdAsync(Guid chatRoomId)
        {
            if (chatRoomId == Guid.Empty)
            {
                return BadRequest("Chat room ID is required.");
            }
            var messages = await _messageManager.GetMessagesByChatRoomIdAsync(chatRoomId);
            if (messages != null && messages.Count > 0)
            {
                return Ok(messages);
            }
            return NotFound("No messages found for the specified chat room.");
        }

        [HttpDelete("deleteMessage")]
        public async Task<IActionResult> DeleteMessageAsync(Guid messageId, Guid chatRoomId)
        {
            if (messageId == Guid.Empty || chatRoomId == Guid.Empty)
            {
                return BadRequest("Message ID is required.");
            }
            var result = await _messageManager.DeleteMessageAsync(messageId, chatRoomId);
            if (result)
            {
                return Ok("Message deleted successfully.");
            }
            return NotFound("Message not found or could not be deleted.");
        }
    }
}