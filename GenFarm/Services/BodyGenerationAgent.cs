using System;
using System.Net.Http;
using System.Threading.Tasks;
using GenFarm.Common;
using Newtonsoft.Json;

namespace GenFarm.Services
{
    public class BodyGenerationAgent
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAIClient _openAIClient;

        public BodyGenerationAgent(
            IHttpClientFactory httpClientFactory,
            OpenAIClient openAIClient
            )
        {
            _httpClient = httpClientFactory.CreateClient();
            _openAIClient = openAIClient;
        }

        public async Task<string> GenerateBodyContent(string header, string prompt)
        {
            var detailedPrompt = $"Write a detailed blog section for the header '{header}'. Focus on: {prompt}";
            var requestMessage = _openAIClient.CreateOpenAIRequest(detailedPrompt, maxTokens: 300);  // Increased token limit for more depth

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
