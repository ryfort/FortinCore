using Fortin.Common.Dtos;
using Fortin.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Interface
{
    public interface IProductRepository
    {
        Task<IQueryable<Product>> GetProductsAsync();
        Task<PagedList<ProductDto>> GetProductsAsync(ProductResourceParameter productResourceParameter);
    }
}
