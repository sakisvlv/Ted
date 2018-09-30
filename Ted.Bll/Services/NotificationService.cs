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
using Ted.Model.DTO;

namespace Ted.Bll.Services
{
    public class NotificationService : INotificationService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<BudgiesHub> _hubContext;

        public NotificationService(Context context, UserManager<User> userManager, IHubContext<BudgiesHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<Result<IEnumerable<NotificationDTO>>> GetNotifications(string userId, int page)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<IEnumerable<NotificationDTO>>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            var notifications = await _context.Notifications
                .Where(x => x.ToUser.Id == Guid.Parse(userId))
                .Skip(page * 20)
                .Take(20)
                .ToListAsync();

            if (notifications == null)
            {
                return Result<IEnumerable<NotificationDTO>>.CreateFailed(
                    HttpStatusCode.NotFound, "Notifications Not Found");
            }

            return Result<IEnumerable<NotificationDTO>>.CreateSuccessful(NotificationDTO.ToNotificationDTOList(notifications));
        }

        public async Task<Result<bool>> AcknowledgeNotification(string userId, string notificationId)
        {
            var user = await _context.Users.Include(t => t.Photo).SingleOrDefaultAsync(x => x.Id == Guid.Parse(userId));
            if (user == null)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.NotFound, "User Does Not Exit");
            }

            var notification = await _context.Notifications.Where(x => x.Id == Guid.Parse(notificationId)).FirstOrDefaultAsync();

            if (notification == null)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.NotFound, "Notifications Not Found");
            }

            notification.IsAcknowledged = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<bool>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Cound't set acknoledged");
            }
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("CheckBudgies", "FriendRequest");
            return Result<bool>.CreateSuccessful(true);
        }
    }
}
