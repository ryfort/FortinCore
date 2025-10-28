using Fortin.Common;
using Fortin.Common.Configuration;
using Fortin.Common.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Proxy
{
    public class FortinAPIClient : HttpProxy
    {
        private readonly ProxyBaseUrls _baseUrl;
        public FortinAPIClient(HttpClient client, IOptionsMonitor<ProxyBaseUrls> baseUrl) : base(client)
        {
            _baseUrl = baseUrl.CurrentValue;
            ConfigureClient();
        }

        public async Task<User[]> GetUsersAsync()
        {
            var httpResponseMessage = await GetAsync<User[], object>("users", null);

            return httpResponseMessage?.Data;
        }

        public async Task<User> CreateUserAsync(AddUserDto newUser)
        {
            var httpResponseMessage = await PostAsync<User, object> ("users", newUser);

            return httpResponseMessage?.Data;
        }

        public async Task<User> UpdateUserAsync(long userId, UpdateUserDto updateUser)
        {
            var httpResponseMessage = await PutAsync<User, object>($"users/{userId}", updateUser);

            return httpResponseMessage?.Data;
        }

        public async Task DeleteUserAsync(long userId)
        {
            var httpResponseMessage = await DeleteAsync<User>($"users/{userId}");
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = new Uri(_baseUrl.FortinAPI);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
        }
    }
}
