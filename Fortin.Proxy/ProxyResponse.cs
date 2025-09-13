using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Proxy
{
    public class ProxyResponse<T>
    {
        public T Data { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
