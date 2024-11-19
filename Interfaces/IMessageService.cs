using NCGraphQL.Types;

namespace GettingStarted.interfaces
{

    public interface IMessageService
    {
        List<Message> GetMessagesByUserId(string userId);
        Task<List<Message>> GetMessagesByUserIdAsync(string userId);

        Task<MessageResponse> AddMessage(string userId, MessageInput messageInput);
    }
}