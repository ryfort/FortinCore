using Fortin.Common;
using Fortin.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Domain.Interface
{
    public interface ILoginService
    {
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto login);
    }
}
