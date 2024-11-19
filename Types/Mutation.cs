using GettingStarted.interfaces;

public record MessageInput(
        string? title,
        string? body,
        string? ctaLabel,
        string? ctaUrl
    );
public class Mutation
{
    public async Task<MessageResponse> AddMessage(string userId, MessageInput messageInput, [Service] IMessageService messageService)
    {
        var messageResponse = await messageService.AddMessage(userId, messageInput);
        return messageResponse!;
    }
    public  string AddName(string input){
        return input;
    }
}


