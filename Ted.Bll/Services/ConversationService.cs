using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Bll.Services
{
    public class ConversationService : IConversationService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public ConversationService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<ConversationDTO>>> GetConversations(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<IEnumerable<ConversationDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "User not found");
            }

            var conversations = await _context.Conversations.Where(x => x.FromUser == user || x.ToUser == user).ToListAsync();

            if (conversations == null)
            {
                return Result<IEnumerable<ConversationDTO>>.CreateFailed(
                   HttpStatusCode.NotFound, "Conversations not found");
            }
            return Result<IEnumerable<ConversationDTO>>.CreateSuccessful(ConversationDTO.ToConversationDTOList(conversations, user));
        }
    }
}
