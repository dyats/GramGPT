using GramGPT.Models;
using Newtonsoft.Json;
using System.Text;

namespace GramGPT;

public static class GPTService
{
    public static async Task<string> CallChatGPTAsync(string apiKey, List<Message> messages)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var request = new ChatGPTRequest { Model = "gpt-3.5-turbo", Messages = messages };
        var jsonContent = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var chatGPTResponse = JsonConvert.DeserializeObject<ChatGPTResponse>(responseBody);
            return chatGPTResponse.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "An error with communication occurred, please try again later";
        }
    }
}
