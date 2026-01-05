using Fortin.Common.Dtos;
using Fortin.Infrastructure.Interface;
using Fortin.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AdventureWorksDbContext _dbContext;
        public ProductRepository(AdventureWorksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductResourceParameter resourceParameter)
        {
            var collection = GetProductsAsync();

            //if (resourceParameter == null)
            //    return collection.ToList();

            if (!string.IsNullOrEmpty(resourceParameter.ProductName))
                collection = collection.Where(p => p.Name.Contains(resourceParameter.ProductName.Trim()));

            var productCollection = collection.AsNoTracking().Select(p => new ProductDto()
            {
                Id = p.ProductId,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                ListPrice = p.ListPrice,
                Color = p.Color ?? string.Empty,
                //ModelName = p.ProductModel != null ? p.ProductModel.Name ?? string.Empty : string.Empty // implicitly joins ProductModel table without using Include()
                ModelName = p.ProductModel!.Name ?? string.Empty
            });

            var products = await PagedList<ProductDto>.CreateAsync(productCollection, resourceParameter.Page, resourceParameter.PageSize);

            //if (resourceParameter.Page != 0 && resourceParameter.PageSize != 0)
            //    collection = collection.Skip(resourceParameter.PageSize * (resourceParameter.Page - 1))
            //                           .Take(resourceParameter.PageSize);
                        
            return products;
        }

        public IQueryable<Product> GetProductsAsync()
        {
            return _dbContext.Products;
        }

        public async Task UpdateProductAsync(int productId, ProductDto product)
        {
            var productEntity = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (productEntity != null)
            {
                productEntity.Name = product.Name;
                productEntity.ProductNumber = product.ProductNumber;
                productEntity.ListPrice = product.ListPrice;
                productEntity.Color = product.Color;

                _dbContext.SaveChanges();
            }
        }
    }
}
