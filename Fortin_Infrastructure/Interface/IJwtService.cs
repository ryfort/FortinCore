using Fortin.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int GetTokenExpirationMinutes();
        string? ValidateToken(string token);
    }
}
