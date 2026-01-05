using Azure.Core;
using Fortin.Common.Configuration;
using Fortin.Common.Dtos;
using Fortin.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Proxy
{
    public class AdventureWorksClient : HttpProxy
    {
        private readonly ProxyBaseUrls _baseUrl;
        public AdventureWorksClient(HttpClient httpClient, IOptionsMonitor<ProxyBaseUrls> proxyBaseUrl) : base(httpClient)
        {
            _baseUrl = proxyBaseUrl.CurrentValue;
            ConfigureClient();
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductResourceParameter resourceParameter)
        {
            var url = "/products";

            if (resourceParameter != null)
            {
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(resourceParameter.ProductName))
                {
                    queryParams.Add($"productname={Uri.EscapeDataString(resourceParameter.ProductName)}");
                }

                queryParams.Add($"page={resourceParameter.Page}");
                queryParams.Add($"pageSize={resourceParameter.PageSize}");

                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);
            }

            var response = await GetAsync<PagedList<ProductDto>, object>(url, null);

            return response.Data;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var response = await GetAsync<ProductDto, object>($"/products/{productId}", null);

            return response.Data;
        }

        public async Task<ProductDto> GetProductsByModel(int modelId)
        {
            var response = await GetAsync<ProductDto, object>($"/productmodels/{modelId}/products", null);

            return response.Data;
        }

        public async Task UpdateProductAsync(int productId, ProductDto product)
        {
            await PutAsync<object, ProductDto>($"/products/{productId}", product);
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = new Uri(_baseUrl.FortinAPI);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
        }
    }
}
