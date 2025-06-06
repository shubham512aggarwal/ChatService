using ChatService.Database;
using ChatService.DTOs;
using ChatService.Managers;
using ChatService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly ChatRoomManager _chatRoomManager;
        public ChatRoomController(ChatRoomManager chatRoomManager)
        {
            _chatRoomManager = chatRoomManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChatRoomAsync([FromBody] CreateChatRoomDTO chatRoom)
        {
            if (chatRoom == null)
            {
                return BadRequest("Chat room data is required.");
            }

            if(chatRoom.Participants == null || chatRoom.Participants.Count == 0)
            {
                return BadRequest("At least one participant is required to create a chat room.");
            }

            var result = await _chatRoomManager.CreateChatRoomAsync(chatRoom);
            if (result)
            {
                return Ok("Chat room created successfully.");
            }
            return BadRequest("Failed to create chat room.");
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetChatRoomByUserIdAsync(Guid userId)
        {
            var chatRooms = await _chatRoomManager.GetChatRoomByUserIdAsync(userId);
            if (chatRooms != null && chatRooms.Count > 0)
            {
                return Ok(chatRooms);
            }
            return NotFound("No chat rooms found for the specified user.");
        }

        [HttpDelete("delete/{chatRoomId}")]
        public async Task<IActionResult> DeleteChatRoomAsync(Guid chatRoomId)
        {
            var result = await _chatRoomManager.DeleteChatRoomAsync(chatRoomId);
            if (result)
            {
                return Ok("Chat room deleted successfully.");
            }
            return NotFound("Chat room not found or already deleted.");
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllChatRoomsAsync()
        {
            var chatRooms = await _chatRoomManager.GetAllChatRoomsAsync();
            if (chatRooms != null && chatRooms?.Count > 0)
            {
                return Ok(chatRooms);
            }
            return NotFound("No chat rooms found.");
        }
}
