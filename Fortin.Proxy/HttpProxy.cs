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

        public async Task<ProxyResponse<TResponse>> GetAsync<TResponse, TRequest>(string url, TRequest requestBoby)
        {
            var proxyResponse = new ProxyResponse<TResponse>();
            var httpResponseMessage = await _client.GetAsync(url);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                proxyResponse.Status = httpResponseMessage.StatusCode.ToString();
                proxyResponse.ErrorMessage = httpResponseMessage.ReasonPhrase ?? string.Empty;
                return proxyResponse;
            }

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();

            if (response is null)
            {
                proxyResponse.Status = "DeserializationError";
                proxyResponse.ErrorMessage = "Response content could not be deserialized.";
                return proxyResponse;
            }

            proxyResponse.Data = response;
            proxyResponse.Status = httpResponseMessage.StatusCode.ToString();

            return proxyResponse;
        }                
    }
}
