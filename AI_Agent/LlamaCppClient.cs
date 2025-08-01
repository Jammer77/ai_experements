using System.Text;
using System.Text.Json;

public class LlamaCppClient
{
    private readonly HttpClient httpClient;
    private readonly string url;

    public LlamaCppClient(string url = "http://localhost:8080/v1/completions")
    {
        this.url = url;
        httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(3600);

    }

    public async Task<string> GenerateAsync(string prompt, int maxTokens = 4000, double temperature = 0.5)
    {
        var requestBody = new
        {
            prompt = prompt,
            max_tokens = maxTokens,
            temperature = temperature
        };

        string json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(responseStream);

        // Safe extraction
        if (doc.RootElement.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("text", out var text))
        {
            return text.GetString() ?? "";
        }

        return "";
    }
}
