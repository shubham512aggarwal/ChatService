using ChatService.DTOs;
using ChatService.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageManager _messageManager;

        public MessageController(MessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        [HttpPost("sendMessage")]
        public async Task<bool> SendMessageAsync([FromBody] SendMessageDTO messageDTO)
        {
            if (messageDTO == null)
            {
                return false;
            }
            bool res = await _messageManager.SendMessageAsync(messageDTO);
            if (!res)
            {
                return false;
            }
            return true;
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