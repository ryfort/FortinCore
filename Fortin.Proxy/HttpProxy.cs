using Azure;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Fortin.Proxy
{
    public class HttpProxy
    {
        protected readonly HttpClient _client;

        public HttpProxy(HttpClient client)
        {
            _client = client;
        }

        protected async Task<ProxyResponse<TResponse>> GetAsync<TResponse, TRequest>(string url, TRequest requestBody)
        {
            var proxyResponse = new ProxyResponse<TResponse>();

            try
            {
                using HttpResponseMessage httpResponseMessage = await _client.GetAsync(url);

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
            }
            catch (Exception ex) { }

            return proxyResponse;
        }

        protected async Task<ProxyResponse<TResponse>> PostAsync<TResponse, TRequest>(string uri, TRequest content)
        {
            var proxyResponse = new ProxyResponse<TResponse>();

            try
            {
                var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await _client.PostAsync(uri, serialized);

                if (!response.IsSuccessStatusCode)
                {
                    proxyResponse.Status = response.StatusCode.ToString();
                    proxyResponse.ErrorMessage = response.ReasonPhrase ?? string.Empty;

                    return proxyResponse;
                }

                var responseMessage = await response.Content.ReadFromJsonAsync<TResponse>();

                if (responseMessage is null)
                {
                    proxyResponse.Status = "DeserializationError";
                    proxyResponse.ErrorMessage = "Response content could not be deserialized.";
                    return proxyResponse;
                }

                proxyResponse.Data = responseMessage;
                proxyResponse.Status = response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                
            }

            return proxyResponse;
        }
    }
}
