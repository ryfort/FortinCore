using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;

namespace Fortin.Proxy
{
    public class HttpProxy
    {
        protected readonly HttpClient _client;

        public HttpProxy(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProxyResponse<TResponse>> GetAsync<TResponse, TRequest>(string url, TRequest request)
        {
            var httpResponseMessage = await _client.GetAsync(url);
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();

            return new ProxyResponse<TResponse>
            {
                Data = response,
                Status = httpResponseMessage.StatusCode.ToString()
            };
        }                
    }
}
