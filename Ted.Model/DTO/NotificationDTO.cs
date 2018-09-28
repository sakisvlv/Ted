using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Notifications;

namespace Ted.Model.DTO
{
    public class NotificationDTO
    {
        public Guid Id { set; get; }
        public string Sender { set; get; }
        public Guid SenderId { set; get; }
        public NotificationType Type { get; set; }
        public Guid? PostId { set; get; }
        public bool IsAcknowledged { set; get; }
        public DateTime DateAdded { set; get; }

        public NotificationDTO(Notification notification)
        {
            Sender = notification.Sender;
            SenderId = notification.SenderId;
            Type = notification.Type;
            PostId = notification.PostId;
            IsAcknowledged = notification.IsAcknowledged;
            DateAdded = notification.DateAdded;
            Id = notification.Id;
        }

        public NotificationDTO()
        {
        }

        public static IEnumerable<NotificationDTO> ToNotificationDTOList(List<Notification> notifications)
        {
            return notifications.Select(x => new NotificationDTO(x));
        }
        
    }
}
