using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Common.Dtos
{
    public class ProductResourceParameter
    {
        public string ProductName { get; set; } = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
