using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model.Auth;

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
    }
}
