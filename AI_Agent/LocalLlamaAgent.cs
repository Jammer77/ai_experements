using System.Text;
using AutoGen.Core;

public class LocalLlamaAgent : IAgent
{
    public string Name => name;
    private readonly LlamaCppClient client;
    private readonly string name;

    public LocalLlamaAgent(string name = "llama")
    {
        this.name = name;
        client = new LlamaCppClient();
    }

    public Task<IMessage> GenerateReplyAsync(IEnumerable<IMessage> messages, GenerateReplyOptions? options = null, CancellationToken cancellationToken = default)
    {
          var result = client.GenerateAsync(messages).Result;
          IMessage message = new TextMessage(Role.Assistant, result);
          return Task.FromResult(message);
    }


}
