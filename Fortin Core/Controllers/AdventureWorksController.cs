using Fortin.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Fortin.Common.Dtos;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AdventureWorksController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductModelRepository _productModelRepository;
        public AdventureWorksController(IProductRepository productRepository, IProductModelRepository productModelRepository)
        {
            _productRepository = productRepository;
            _productModelRepository = productModelRepository;
        }

        [HttpGet("/products")]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] ProductResourceParameter? query)
        {
            var products = await _productRepository.GetProductsAsync(query); 

            return products;
        }

        [HttpGet("/products/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var products = await _productRepository.GetProductsAsync();
            var product = products.Where(p => p.ProductId == productId);

            return Ok(product);
        }

        [HttpGet("/productmodels/{productModelId}/products")]
        public async Task<IActionResult> GetProductsByProductModel(int productModelId)
        {
            var productModels = await _productModelRepository.GetProductModelsAsync();
            var products = productModels.Where(m => m.ProductModelId == productModelId)
                                        .Select(s =>
                                            {

                                                var products = s.Products.Select(p =>
                                                {
                                                    var product = new ProductDto()
                                                        {
                                                            Name = p.Name,
                                                            ProductNumber = p.ProductNumber,
                                                            ListPrice = p.ListPrice,
                                                            Color = p.Color?? string.Empty
                                                    };

                                                        return product;
                                                });

                                                return products;
                                            });

            return Ok(products);
        }
    }
}
