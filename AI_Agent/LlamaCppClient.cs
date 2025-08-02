using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using AutoGen.Core;

public class LlamaCppClient
{
    private readonly HttpClient httpClient;
    private readonly string url;

    public LlamaCppClient(string url = "http://localhost:8080/v1/chat/completions")
    {
        this.url = url;
        httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(3600);
    }

    public async Task<string> GenerateAsync(IEnumerable<IMessage> messages, int maxTokens = 4000, double temperature = 0.2)
    {
        var messages_array = messages.Cast<TextMessage>().Select(c => new { role = c.Role.ToString(), content = c.Content }).ToArray();

        var requestBody = new
        {
            model = "openhermes",
            temperature = temperature,
            max_tokens = maxTokens,
            messages = messages_array
        };

        string json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(responseStream);

        string result = "";

        if (doc.RootElement.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var message) &&
            message.TryGetProperty("content", out var answer)
            )
        {
            result = answer.GetString() ?? String.Empty;
        }

        return result; 
    }
}
