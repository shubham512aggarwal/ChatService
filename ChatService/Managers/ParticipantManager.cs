using ChatService.Database;
using ChatService.DTOs;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Managers
{
    public class ParticipantManager
    {
        private readonly ChatDbContext _context;
        private readonly ILogger _logger;

        public ParticipantManager(ChatDbContext context, ILogger<ParticipantManager> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> AddParticipantAsync(AddParticipantDTO addParticipantDTO)
        {
            try
            {
                if (addParticipantDTO != null)
                {
                    var chatRoom = await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .FirstOrDefaultAsync(cr => cr.Id == addParticipantDTO.ChatRoomId && !cr.IsDeleted);

                    if (chatRoom != null)
                    {
                        var existingParticipant = chatRoom.Participants
                            .FirstOrDefault(p => p.UserId == addParticipantDTO.UserId);

                        if (existingParticipant == null)
                        {
                            chatRoom.Participants.Add(new ChatParticipant
                            {
                                UserId = addParticipantDTO.UserId,
                                ChatRoomId = chatRoom.Id,
                                JoinedAt = DateTime.UtcNow,
                                IsAdmin = false
                            });
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"User {addParticipantDTO.UserId} added to chat room {chatRoom.Name}");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function AddParticipantAsync() ", ex);
                return false;
            }
        }

        public async Task<bool> RemoveParticipantAsync(AddParticipantDTO addParticipantDTO)
        {
            try
            {
                if (addParticipantDTO != null)
                {
                    var chatRoom = await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .FirstOrDefaultAsync(cr => cr.Id == addParticipantDTO.ChatRoomId && !cr.IsDeleted);
                    if (chatRoom != null)
                    {
                        var participantToRemove = chatRoom.Participants
                            .FirstOrDefault(p => p.UserId == addParticipantDTO.UserId);
                        if (participantToRemove != null)
                        {
                            chatRoom.Participants.Remove(participantToRemove);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"User {addParticipantDTO.UserId} removed from chat room {chatRoom.Name}");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function RemoveParticipant() ", ex);
                return false;
            }
        }

        public async Task<List<ChatParticipant>> GetParticipantsByChatRoomIdAsync(Guid chatRoomId)
        {
            try
            {
                if (chatRoomId != Guid.Empty)
                {
                    return await _context.ChatParticipants
                        .Where(cp => cp.ChatRoomId == chatRoomId)
                        .ToListAsync();
                }
                return new List<ChatParticipant>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function GetParticipantsByChatRoomIdAsync() ", ex);
                return new List<ChatParticipant>();
            }
        }

        public async Task<bool> SetAdminAsync(AddParticipantDTO addParticipantDTO, bool isAdmin)
        {
            try
            {
                if (addParticipantDTO != null)
                {
                    var chatRoom = await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .FirstOrDefaultAsync(cr => cr.Id == addParticipantDTO.ChatRoomId && !cr.IsDeleted);
                    if (chatRoom != null)
                    {
                        var participant = chatRoom.Participants
                            .FirstOrDefault(p => p.UserId == addParticipantDTO.UserId);
                        if (participant != null)
                        {
                            participant.IsAdmin = isAdmin;
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"User {addParticipantDTO.UserId} admin status set to {isAdmin} in chat room {chatRoom.Name}");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function SetAdminAsync() ", ex);
                return false;
            }
        }
    }
}