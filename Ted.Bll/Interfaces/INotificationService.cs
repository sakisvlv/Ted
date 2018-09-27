using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface INotificationService
    {
        Task<Result<IEnumerable<NotificationDTO>>> GetNotifications(string userId, int page);
        Task<Result<bool>> AcknowledgeNotification(string userId, string notificationId);
    }
}
