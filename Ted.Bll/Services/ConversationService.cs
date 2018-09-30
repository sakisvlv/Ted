using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Bll.SignalR;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.Conversations;
using Ted.Model.DTO;

namespace Ted.Bll.Services
{
    public class ConversationService : IConversationService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<MessagesHub> _hubContext;
        private readonly IHubContext<BudgiesHub> _BudgiesHubContext;


        public ConversationService(Context context, UserManager<User> userManager, IHubContext<MessagesHub> hubContext, IHubContext<BudgiesHub> BudgiesHubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _BudgiesHubContext = BudgiesHubContext;
        }

        public async Task<Result<IEnumerable<ConversationDTO>>> GetConversations(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<ConversationDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var conversations = await _context.Conversations
                .Where(x => x.FromUser == user || x.ToUser == user)
                .Include(x => x.ToUser)
                .Include(x => x.FromUser)
                .OrderByDescending(x => x.LastMessageDate)
                .Distinct()
                .ToListAsync();

            if (conversations == null)
            {
                return Result<IEnumerable<ConversationDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Conversations not found");
            }
            return Result<IEnumerable<ConversationDTO>>.CreateSuccessful(ConversationDTO.ToConversationDTOList(conversations, user));
        }

        public async Task<Result<ConversationDTO>> StartConversation(string userId, string toUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ConversationDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var toUser = await _userManager.FindByIdAsync(toUserId);
            if (toUser == null)
            {
                return Result<ConversationDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var conversation = new Conversation();
            conversation.FromUser = user;
            conversation.ToUser = toUser;
            conversation.LastMessageDate = DateTime.Now;
            await _context.Conversations.AddAsync(conversation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<ConversationDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't sstart conversation");
            }


            return Result<ConversationDTO>.CreateSuccessful(new ConversationDTO(conversation, user));
        }

        public async Task<Result<IEnumerable<MessageDTO>>> GetMessages(string userId, string conversationId, int page)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<MessageDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var messages = await _context.Messages
                .Where(x => x.Conversation.Id == Guid.Parse(conversationId))
                .Include(x => x.Sender)
                .OrderByDescending(x => x.DateSended)
                .Skip(page * 10)
                .Take(10)
                .ToListAsync();

            if (messages == null)
            {
                return Result<IEnumerable<MessageDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Messages not found");
            }
            await _BudgiesHubContext.Clients.User(user.Id.ToString()).SendAsync("CheckBudgies", "FriendRequest");
            return Result<IEnumerable<MessageDTO>>.CreateSuccessful(MessageDTO.ToMessageDTOList(messages.OrderBy(x => x.DateSended)));
        }

        public async Task<Result<MessageDTO>> SendMessage(string userId, string text, string conversationId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<MessageDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var conversation = await _context.Conversations
                .Where(x => x.Id == Guid.Parse(conversationId))
                .Include(x => x.Messages)
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                return Result<MessageDTO>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            if (conversation.ToUser == user)
            {
                conversation.FromUserHasNewMessages = true;
            }
            else
            {
                conversation.ToUserHasNewMessages = true;
            }
            conversation.LastMessageDate = DateTime.Now;
            var message = new Message();
            message.Sender = user;
            message.Text = text;
            message.DateSended = DateTime.Now;
            conversation.Messages.Add(message);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<MessageDTO>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't send the message");
            }

            await _hubContext.Clients.User(conversation.ToUser == user ? conversation.FromUser.Id.ToString() : conversation.ToUser.Id.ToString()).SendAsync("ReceiveMessage", message.Text, conversation.Id.ToString());
            await _BudgiesHubContext.Clients.User(conversation.ToUser == user ? conversation.FromUser.Id.ToString() : conversation.ToUser.Id.ToString()).SendAsync("CheckBudgies", "FriendRequest");


            return Result<MessageDTO>.CreateSuccessful(new MessageDTO(message));
        }

        public async Task<Result<bool>> AckConversation(string userId, string conversationId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var conversation = await _context.Conversations
                .Where(x => x.Id == Guid.Parse(conversationId))
                .Include(x => x.FromUser)
                .Include(x => x.ToUser)
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                return Result<bool>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            if (conversation.ToUser == user)
            {
                conversation.ToUserHasNewMessages = false;
            }
            else
            {
                conversation.FromUserHasNewMessages = false;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't send the message");
            }

            await _BudgiesHubContext.Clients.User(user.Id.ToString()).SendAsync("CheckBudgies", "FriendRequest");

            return Result<bool>.CreateSuccessful(true);
        }

    }
}
