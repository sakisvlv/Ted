using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IConversationService
    {
        Task<Result<IEnumerable<ConversationDTO>>> GetConversations(string userId);
        Task<Result<IEnumerable<MessageDTO>>> GetMessages(string userId, string conversationId, int page);
        Task<Result<MessageDTO>> SendMessage(string userId, string text, string conversationId);
        Task<Result<ConversationDTO>> StartConversation(string userId, string toUserId);
        Task<Result<bool>> AckConversation(string userId, string conversationId);
    }
}
