using Fortin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin_Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);
    }
}
