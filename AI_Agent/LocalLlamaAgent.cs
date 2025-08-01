using System.Text;
using AutoGen.Core;

public class LocalLlamaAgent : IAgent
{
    private readonly LlamaCppClient client;
    private readonly string name;

    public LocalLlamaAgent(string name = "llama")
    {
        this.name = name;
        client = new LlamaCppClient();
    }

    public string Name => name;

    public Task<IMessage> GenerateReplyAsync(IEnumerable<IMessage> messages, GenerateReplyOptions? options = null, CancellationToken cancellationToken = default)
    {
          string prompt = MessageToPrompt(messages);
          var result = client.GenerateAsync(prompt).Result;
          IMessage message = new TextMessage(Role.Assistant, result);
          return Task.FromResult(message);
    }


    private string MessageToPrompt(IEnumerable<IMessage> messages)
    {
        var sb = new StringBuilder();

        foreach (TextMessage msg in messages)
        {
            if (msg.Role == Role.User)
            {
                sb.AppendLine($"[INST] {msg.Content} [/INST]");
            }
            else if (msg.Role == Role.Assistant)
            {
                sb.AppendLine($"<|assistant|> {msg.Content}");
            }
            else if (msg.Role == Role.System)
            {
                throw new NotImplementedException();
            }
        }

        //sb.Append("<|assistant|>");
        var result = sb.ToString();
        return result;
    }
}
