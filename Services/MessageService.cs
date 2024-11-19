
using System.Net.Http.Headers;
using System.Text.Json;
using GettingStarted.interfaces;
using NCGraphQL.Types;
using static System.Net.Mime.MediaTypeNames;

namespace NCGraphQL.Services
{
    public class MessageService : IMessageService
    {
        private readonly string PATH_ENDPOINT = "Message";
        private readonly ILogger<MessageService> logger;
        private readonly HttpClient httpClient;

        public MessageService(ILogger<MessageService> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            httpClient = httpClientFactory.CreateClient("nc");
        }

        public List<Message> transform(List<MessagePayload> msgPayloadList)
        {
            List<Message> messages = new List<Message>();
            msgPayloadList.ForEach(nc =>
            {
                Cta nCCta = new Cta(nc.CtaLabel!, nc.CtaUrl!);
                messages.Add(new Message(nc.Id!, nc.CampaignCode, nc.CampaignVariant, nc.Title, nc.Body, nCCta, nc.Ttl, nc.CreatedOn));
            });
            return messages;
        }
        public List<Message> GetMessagesByUserId(string userId)
        {
            List<MessagePayload>? ncPayloadList = httpClient.GetFromJsonAsync<List<MessagePayload>>($"{PATH_ENDPOINT}/{userId}").Result;
            return transform(ncPayloadList!);
        }

        public async Task<List<Message>> GetMessagesByUserIdAsync(string userId)
        {
            try
            {
                List<MessagePayload>? messages = await httpClient.GetFromJsonAsync<List<MessagePayload>>($"{PATH_ENDPOINT}/{userId}");
                return transform(messages!);
            }
            catch (Exception ex)
            {
                logger.LogWarning($"GetMessagesByUserIdAsync: failed reason: {ex.Message}");
                throw new Exception($"GetMessagesByUserIdAsync: failed reason: {ex.Message}");
            }

        }

        public MessagePayload toMessagePayload(string userId, MessageInput messageInput)
        {
            return new MessagePayload(
                Id: Guid.NewGuid().ToString(),
                UserId: userId,
                Body: messageInput.body,
                Title: messageInput.title,
                CampaignCode: "MOB-6342",
                CampaignVariant: "WOW-OrderEvent-OrderPlacedMessage",
                CreatedOn: DateTime.UtcNow.ToString(),
                CtaUrl: messageInput.ctaUrl,
                CtaLabel: messageInput.ctaLabel,
                Ttl: DateTime.UtcNow
            );

        }

        public async Task<MessageResponse> AddMessage(string userId, MessageInput messageInput)
        {
            try
            {
                var messagePayload = JsonSerializer.Serialize(toMessagePayload(userId, messageInput));
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, PATH_ENDPOINT);
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Application.Json));
                httpRequestMessage.Content = new StringContent(messagePayload, System.Text.Encoding.UTF8);
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(Application.Json);
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var messageResponse = JsonSerializer.Deserialize<MessageResponse>(responseContent);
                httpResponseMessage.EnsureSuccessStatusCode();
                return messageResponse!;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"AddMessage: failed reason: {ex.Message}");
                throw new Exception($"failed to add message via rest api, reason: {ex.Message}");
            }
        }
    }
}