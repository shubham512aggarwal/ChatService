using ChatService.Database;
using ChatService.DTOs;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Managers
{
    public class MessageManager
    {
        private readonly ChatDbContext _context;
        private readonly ILogger _logger;

        public MessageManager(ChatDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(SendMessageDTO sendMessageDTO)
        {
            try
            {
                if (sendMessageDTO != null)
                {
                    var chatRoom = await _context.ChatRooms
                        .Include(cr => cr.Participants)
                        .FirstOrDefaultAsync(cr => cr.Id == sendMessageDTO.ChatRoomId && !cr.IsDeleted);
                    if (chatRoom != null)
                    {
                        var participant = chatRoom.Participants
                            .FirstOrDefault(p => p.UserId == sendMessageDTO.ParticipantId);
                        if (participant != null)
                        {
                            var message = new Message
                            {
                                ChatRoomId = chatRoom.Id,
                                SenderId = participant.UserId,
                                Content = sendMessageDTO.MessageContent,
                                SentAt = sendMessageDTO.Timestamp
                            };
                            chatRoom.Messages.Add(message);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Message sent by {sendMessageDTO.ParticipantId} in chat room {chatRoom.Name}");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function SendMessage() ", ex);
                return false;
            }
        }

        public async Task<List<Message>> GetMessagesByChatRoomIdAsync(Guid chatRoomId)
        {
            try
            {
                if (chatRoomId != Guid.Empty)
                {
                    return await _context.Messages
                        .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
                        .OrderByDescending(m => m.SentAt)
                        .ToListAsync();
                }
                return new List<Message>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function GetMessagesByChatRoomIdAsync() ", ex);
                return new List<Message>();
            }
        }

        public async Task<bool> DeleteMessageAsync(Guid messageId, Guid chatRoomId)
        {
            try
            {
                if (messageId != Guid.Empty && chatRoomId != Guid.Empty)
                {
                    var message = await _context.Messages
                        .FirstOrDefaultAsync(m => m.Id == messageId && m.ChatRoomId == chatRoomId && !m.IsDeleted);
                    if (message != null)
                    {
                        message.IsDeleted = true;
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Message {messageId} in chat room {chatRoomId} deleted successfully");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in function DeleteMessageAsync() ", ex);
                return false;
            }
        }

    }
}
