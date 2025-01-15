using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Techa.DocumentGenerator.Application.Services.Interfaces.Ai;

namespace Techa.DocumentGenerator.Application.Services.Implementations.Ai
{
    public class AvalAiService : IAvalAiService
    {
        private readonly IOpenAIService _openAIService;

        public AvalAiService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        ///<inheritdoc/>
        public async Task<string> CreateCompletionAsync(List<ChatMessage> messages, string? model = "gpt-3.5-turbo")
        {
            var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = messages,
                Model = model,
            });
            if (completionResult.Successful)
            {
                return string.Join("\n", completionResult.Choices.Select(x => x.Message.Content ?? "no-data"));
            }
            return string.Join("\n", completionResult?.Error?.Messages ?? new List<string?>());
        }
    }
}
