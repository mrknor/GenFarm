using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenFarm.Services
{
    public class HeaderGenerationAgent
    {
        private readonly HttpClient _httpClient; // HTTP client for API calls
        private readonly string _openAIApiKey; // OpenAI API key

        public HeaderGenerationAgent(IHttpClientFactory httpClientFactory, string openAIApiKey)
        {
            _httpClient = httpClientFactory.CreateClient();
            _openAIApiKey = openAIApiKey;
        }

        public async Task<List<string>> GenerateSubHeaders(string seoPhrase)
        {
            try
            {
                // Prepare the request data for OpenAI API
                var requestData = new
                {
                    prompt = $"Generate 5-10 sub-headers for a blog post about: {seoPhrase}",
                    max_tokens = 150 // Adjust as needed
                };

                // Configure the HTTP request
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(requestData))
                };
                requestMessage.Headers.Add("Authorization", $"Bearer {_openAIApiKey}");

                // Send the request to OpenAI API
                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                // Extract the sub-headers from the response
                var responseBody = await response.Content.ReadAsStringAsync();
                var subHeaders = JsonConvert.DeserializeObject<List<string>>(responseBody);

                return subHeaders;
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Console.WriteLine($"Error generating sub-headers: {ex.Message}");
                throw; // Optionally rethrow to signal failure
            }
        }
    }
}
