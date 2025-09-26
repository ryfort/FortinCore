using Fortin.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Implementation
{
    public class ProductModelRepository : IProductModelRepository
    {
        private readonly AdventureWorksDbContext _dbContext;

        public ProductModelRepository(AdventureWorksDbContext adventureWorksDbContext)
        {
            _dbContext = adventureWorksDbContext;    
        }

        public async Task<IEnumerable<Models.ProductModel>> GetProductModelsAsync()
        {
            var productModels = _dbContext.ProductModels.Include(m => m.Products);

            return productModels;
        }
    }
}
