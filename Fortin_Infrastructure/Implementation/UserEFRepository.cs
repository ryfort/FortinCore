using Fortin.Common.Dtos;
using Fortin.Infrastructure.Entities;
using Fortin.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Implementation
{
    public class UserEFRepository : IUserEFRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserEFRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<User> GetUserById(long id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(UserResourceParameter userResourceParameter)
        {
            var users = _appDbContext.Users as IQueryable<User>;

            if (string.IsNullOrEmpty(userResourceParameter.FirstName) && string.IsNullOrEmpty(userResourceParameter.LastName))
                return await users.ToListAsync();

            if (!string.IsNullOrEmpty(userResourceParameter.FirstName))
                users = users.Where(c => c.FirstName.Contains(userResourceParameter.FirstName.Trim()));

            if (!string.IsNullOrEmpty(userResourceParameter.LastName))
                users = users.Where(c => c.LastName.Contains(userResourceParameter.LastName.Trim()));

            return await users.ToListAsync();
        }
    }
}
