using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GenFarm.Services;
using GenFarm.Common;

namespace GenFarm.Services
{
    public class HeaderGenerationAgent
    {
        private readonly HttpClient _httpClient; // HTTP client for API calls
        private readonly OpenAIClient _openAIClient;

        public HeaderGenerationAgent(
            IHttpClientFactory httpClientFactory, 
            OpenAIClient openAIClient
            )
        {
            _httpClient = httpClientFactory.CreateClient();
            _openAIClient = openAIClient;
        }

        public async Task<Dictionary<string, string>> GenerateSubHeaders(string seoPhrase)
        {
            var prompt = $"Generate 5-10 sub-headers for a blog post about {seoPhrase}, and provide a brief prompt for each header in the format 'Header: Prompt'. Ensure each header and prompt pair is on a new line.";
            var requestMessage = _openAIClient.CreateOpenAIRequest(prompt);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var headersWithPrompts = ParseHeadersAndPrompts(content);

            return headersWithPrompts;
        }

        private Dictionary<string, string> ParseHeadersAndPrompts(string content)
        {
            var headersWithPrompts = new Dictionary<string, string>();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                var split = line.Split(new[] { ':' }, 2); // Split on the first colon
                if (split.Length == 2)
                {
                    var header = split[0].Trim();
                    var prompt = split[1].Trim();
                    if (!headersWithPrompts.ContainsKey(header))
                    {
                        headersWithPrompts[header] = prompt;
                    }
                }
            }

            return headersWithPrompts;
        }
    }
}
