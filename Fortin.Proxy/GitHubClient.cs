
using Fortin.Common.Dtos;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;

namespace Fortin.Proxy
{
    public class GitHubClient : HttpProxy
    {
        public GitHubClient(HttpClient client) : base(client)
        {
            ConfigureClient();
        }

        public async Task<int> GetFollowersCount(string username)
        {
            var users = await GetFollowers(username);

            return users?.Length ?? 0;
        }

        public async Task<GithubUserDto[]> GetFollowers(string username)
        {
            var httpResponseMessage = await GetAsync<GithubUserDto[], object>($"users/{username}/followers", null);

            return httpResponseMessage?.Data;
        }

        public async Task<GithubUserDto> GetUserByUsername(string username)
        {
            var httpResponseMessage = await GetAsync<GithubUserDto, object>($"users/{username}", null);

            return httpResponseMessage?.Data;
        }

        public async Task<GithubUserRepo[]> GithubUserRepos(string username)
        {
            var httpResponseMessage = await GetAsync<GithubUserRepo[], object>($"users/{username}/repos", null);

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
