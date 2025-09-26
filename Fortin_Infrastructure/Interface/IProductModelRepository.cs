using Fortin.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Interface
{
    public interface IProductModelRepository
    {
        Task<IEnumerable<ProductModel>> GetProductModelsAsync();
    }
}
