
using Fortin.API.Controllers;
using Microsoft.Data.SqlClient.Diagnostics;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Fortin.API
{
    public class GitHubClient : IGitHubClient
    {
        private readonly HttpClient _client;

        public GitHubClient(HttpClient client)
        {
            _client = client;
            ConfigureClient();
        }

        public async Task<int> GetFollowersCount()
        {
            var httpResponseMessage = await _client.GetAsync("users/danpdc/followers");
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<object[]>();

            return response?.Length ?? 0;
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = new Uri("https://api.github.com/");
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            _client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
        }
    }
}
