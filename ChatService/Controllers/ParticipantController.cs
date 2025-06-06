using ChatService.DTOs;
using ChatService.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly ParticipantManager _participantManager;
        public ParticipantController(ParticipantManager participantManager)
        {
            _participantManager = participantManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddParticipantAsync([FromBody] AddParticipantDTO addParticipantDTO)
        {
            if (addParticipantDTO == null)
            {
                return BadRequest("Participant data is required.");
            }
            var result = await _participantManager.AddParticipantAsync(addParticipantDTO);
            if (result)
            {
                return Ok("Participant added successfully.");
            }
            return BadRequest("Failed to add participant.");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveParticipantAsync([FromBody] AddParticipantDTO addParticipantDTO)
        {
            if (addParticipantDTO == null)
            {
                return BadRequest("Participant data is required.");
            }
            var result = await _participantManager.RemoveParticipantAsync(addParticipantDTO);
            if (result)
            {
                return Ok("Participant removed successfully.");
            }
            return BadRequest("Failed to remove participant.");
        }

        [HttpGet("get/{chatRoomId}")]
        public async Task<IActionResult> GetParticipantsByChatRoomIdAsync(Guid chatRoomId)
        {
            if (chatRoomId == Guid.Empty)
            {
                return BadRequest("Chat room ID is required.");
            }
            var participants = await _participantManager.GetParticipantsByChatRoomIdAsync(chatRoomId);
            if (participants != null && participants.Count > 0)
            {
                return Ok(participants);
            }
            return NotFound("No participants found for the specified chat room.");
        }

        [HttpPost("/setAdmin")]
        public async Task<IActionResult> SetAdminAsync([FromBody]AddParticipantDTO addParticipantDTO, bool isAdmin = false)
        {
            if (addParticipantDTO == null)
            {
                return BadRequest("Paticipant data is required");
            }
            bool result = await _participantManager.SetAdminAsync(addParticipantDTO, isAdmin);
            if (result)
            {
                return Ok("Participant is set as Admin successfully");
            }
            return BadRequest("Error in setting the participant admin");
        }
    }
}
