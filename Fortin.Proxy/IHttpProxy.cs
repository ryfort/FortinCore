using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Proxy
{
    public interface IHttpProxy
    {
        Task<ProxyResponse<TResponse>> GetAsync<TResponse, TRequest>(string url, TRequest request);
    }
}
