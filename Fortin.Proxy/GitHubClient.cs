
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;

namespace Fortin.Proxy
{
    public class GitHubClient : HttpProxy
    {
        //private readonly HttpClient _client;

        public GitHubClient(HttpClient client) : base(client)
        {
            //_client = client;
            ConfigureClient();
        }

        public async Task<int> GetFollowersCount()
        {
            var httpResponseMessage = await GetAsync<GithubUserDto[], object>("users/danpdc/followers", null);

            return httpResponseMessage?.Data.Length ?? 0;
        }

        public async Task<GithubUserDto[]> GetFollowers()
        {
            var httpResponseMessage = await GetAsync<GithubUserDto[], object>("users/danpdc/followers", null);

            return httpResponseMessage?.Data;
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = new Uri("https://api.github.com/");
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            _client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
        }
    }
}
