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
            var collection = await GetProductsAsync();

            //if (resourceParameter == null)
            //    return collection.ToList();

            if (!string.IsNullOrEmpty(resourceParameter.ProductName))
                collection = collection.Where(p => p.Name.Contains(resourceParameter.ProductName.Trim()));

            var productCollection = collection.Select(p => new ProductDto()
            {
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                ListPrice = p.ListPrice,
                Color = p.Color ?? string.Empty
            });

            var products = await PagedList<ProductDto>.CreateAsync(productCollection, resourceParameter.Page, resourceParameter.PageSize);

            //if (resourceParameter.Page != 0 && resourceParameter.PageSize != 0)
            //    collection = collection.Skip(resourceParameter.PageSize * (resourceParameter.Page - 1))
            //                           .Take(resourceParameter.PageSize);
                        
            return products;
        }

        public async Task<IQueryable<Product>> GetProductsAsync()
        {
            return _dbContext.Products;
        }
    }
}
