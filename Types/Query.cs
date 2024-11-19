using GettingStarted.interfaces;
using NCGraphQL.Types;

[QueryType]
public class Query
{
    public async Task<List<Message>> GetMessages(string userId, [Service] IMessageService messageService) {
        return await messageService.GetMessagesByUserIdAsync(userId);
    }   
}
