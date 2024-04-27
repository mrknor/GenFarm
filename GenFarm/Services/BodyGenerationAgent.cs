using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenFarm.Services
{
    public class BodyGenerationAgent
    {
        private readonly HttpClient _httpClient;
        private readonly string _openAIApiKey;

        public BodyGenerationAgent(HttpClient httpClient, string openAIApiKey)
        {
            _httpClient = httpClient;
            _openAIApiKey = openAIApiKey;
        }

        public async Task<string> GenerateBodyContent(string subHeader)
        {
            try
            {
                // Prepare the request data for OpenAI API
                var requestData = new
                {
                    prompt = $"Generate detailed content for: {subHeader}",
                    max_tokens = 250 // Adjust based on desired length
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData))
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {_openAIApiKey}");

                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody; // Extracted body content
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating body content: {ex.Message}");
                throw;
            }
        }
    }
}
