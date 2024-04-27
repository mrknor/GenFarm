using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenFarm.Common
{
    public class OpenAIClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _openAIApiKey;

        public OpenAIClient(HttpClient httpClient, string openAIApiKey)
        {
            _httpClient = httpClient;
            _openAIApiKey = openAIApiKey;
        }

        private HttpRequestMessage CreateOpenAIRequest(string prompt, int maxTokens = 150, double temperature = 0.7)
        {
            var requestData = new
            {
                prompt = prompt,
                max_tokens = maxTokens,
                temperature = temperature
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions")
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("Authorization", $"Bearer {_openAIApiKey}");
            return requestMessage;
        }

        public async Task<List<string>> GenerateSubHeaders(string seoPhrase)
        {
            var requestMessage = CreateOpenAIRequest($"Generate 5-10 sub-headers for a blog post about: {seoPhrase}");

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var subHeaders = JsonConvert.DeserializeObject<List<string>>(content);

            return subHeaders;
        }

        public async Task<string> GenerateBodyContent(string subHeader)
        {
            var requestMessage = CreateOpenAIRequest($"Generate detailed content for: {subHeader}", maxTokens: 250);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var bodyText = content;

            return bodyText;
        }

        public async Task<string> GenerateCustomContent(string customPrompt, int maxTokens = 150)
        {
            var requestMessage = CreateOpenAIRequest(customPrompt, maxTokens: maxTokens);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
