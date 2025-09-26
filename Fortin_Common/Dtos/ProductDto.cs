using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Common.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}
