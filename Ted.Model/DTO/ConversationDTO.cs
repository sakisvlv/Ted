using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ted.Model.Auth;
using Ted.Model.Conversations;

namespace Ted.Model.DTO
{
    public class ConversationDTO
    {
        public Guid Id { get; set; }
        public UserInfoSmallDTO ToUser { get; set; }
        public bool HasNewMessagees { get; set; }

        public ConversationDTO()
        {
        }

        public ConversationDTO(Conversation conversation, User user)
        {
            Id = conversation.Id;
            ToUser = new UserInfoSmallDTO(conversation.ToUser == user ? conversation.FromUser : conversation.ToUser);
            HasNewMessagees = conversation.ToUser == user ? conversation.FromUserHasNewMessages : conversation.ToUserHasNewMessages;
        }

        public static IEnumerable<ConversationDTO> ToConversationDTOList(IEnumerable<Conversation> conversation, User user)
        {
            return conversation?.Select(x => new ConversationDTO(x, user));
        }
    }
}
