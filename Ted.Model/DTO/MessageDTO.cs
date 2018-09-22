using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Conversations;

namespace Ted.Model.DTO
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public UserInfoSmallDTO Sender { get; set; }
        public DateTime DateSended { get; set; }

        public MessageDTO()
        {
        }

        public MessageDTO(Message message)
        {
            Id = message.Id;
            Text = message.Text;
            Sender = new UserInfoSmallDTO(message.Sender);
            DateSended = message.DateSended;
        }

        public static IEnumerable<MessageDTO> ToMessageDTOList(IEnumerable<Message> messages)
        {
            return messages?.Select(x => new MessageDTO(x));
        }
    }
}
