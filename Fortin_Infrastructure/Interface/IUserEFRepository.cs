using Fortin.Common.Dtos;
using Fortin.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Interface
{
    public interface IUserEFRepository
    {
        Task<User> GetUserById(long id);
        Task<IEnumerable<User>> GetUsersAsync(UserResourceParameter userResourceParameter);
    }
}
