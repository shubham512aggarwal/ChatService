using ChatService.Database;
using ChatService.DTOs;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Managers
{
    public class ChatRoomManager
    {

        private readonly ChatDbContext _context;
        private readonly ILogger<ChatRoomManager> _logger;

        public ChatRoomManager(ChatDbContext context, ILogger<ChatRoomManager> logger) { 
            _context = context;
            _logger = logger;
        }

        public async Task<ChatRoom?> GetChatRoomByIdAsync(Guid chatRoomId)
        {
            try
            {
                if (chatRoomId != Guid.Empty)
                {
                    return await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .Include(cr => cr.Messages)
                        .FirstOrDefaultAsync(cr => cr.Id == chatRoomId && !cr.IsDeleted);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function GetChatRoomByIdAsync() ", ex);
                return null;
            }
        }

        public async Task<List<ChatRoom>> GetChatRoomByUserIdAsync(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .Include(cr => cr.Messages)
                        .Where(cr => cr.Participants.Any(p => p.UserId == userId) && !cr.IsDeleted)
                        .ToListAsync();
                }
                return new List<ChatRoom>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function GetChatRoomByUserId() ", ex);
                return new List<ChatRoom>();
            }
        }

        public async Task<bool> CreateChatRoomAsync(CreateChatRoomDTO chatRoom)
        {
            try
            {
                if (chatRoom != null)
                {
                    chatRoom.CreatedAt = DateTime.UtcNow;
                    ChatRoom newChatRoom = new ChatRoom
                    {
                        Id = Guid.NewGuid(),
                        Name = chatRoom.Name,
                        Description = chatRoom.Description,
                        IsGroup = false,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false,
                        Participants = new List<ChatParticipant>()
                    };
                    if (chatRoom.Participants != null && chatRoom.Participants.Count > 0)
                    {
                        foreach (var participantId in chatRoom.Participants)
                        {
                            newChatRoom.Participants.Add(new ChatParticipant
                            {
                                UserId = participantId,
                                ChatRoomId = newChatRoom.Id,
                                IsAdmin = participantId == chatRoom.AdminUserId
                            });
                        }
                    }
                    _context.ChatRooms.Add(newChatRoom);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"New Chat room {newChatRoom.Name} created successfully");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function CreateChatRoomAsync() ", ex);
                return false;
            }
        }

        public async Task<bool> DeleteChatRoomAsync(Guid chatRoomId)
        {
            try
            {
                if (chatRoomId != Guid.Empty)
                {
                    var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
                    if (chatRoom != null)
                    {
                        chatRoom.IsDeleted = true;
                        _context.ChatRooms.Update(chatRoom);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Chat room {chatRoom.Name} deleted successfully");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function DeleteChatRoomAsync() ", ex);
                return false;
            }
        }

        public async Task<bool> UpdateChatRoomAsync(ChatRoom chatRoom)
        {
            try
            {
                if (chatRoom != null && chatRoom.Id != Guid.Empty && chatRoom.IsGroup)
                {
                    var existingChatRoom = await _context.ChatRooms.FindAsync(chatRoom.Id);
                    if (existingChatRoom != null)
                    {
                        existingChatRoom.Name = chatRoom.Name;
                        existingChatRoom.Description = chatRoom.Description;
                        _context.ChatRooms.Update(existingChatRoom);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Chat room {existingChatRoom.Name} updated successfully");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function UpdateChatRoomAsync() ", ex);
                return false;
            }
        }

        public async Task<List<ChatRoom>> GetAllChatRoomsAsync()
        {
            try
            {
                return await _context.ChatRooms
                    .Include(cr => cr.Participants)
                    .Include(cr => cr.Messages)
                    .Where(cr => !cr.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function GetAllChatRoomsAsync() ", ex);
                return new List<ChatRoom>();
            }
        }
    }
}
