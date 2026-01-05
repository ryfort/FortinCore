using Fortin.Common.Dtos;
using Fortin.Infrastructure.Entities;
using Fortin.Infrastructure.Interface;
using Fortin.Infrastructure.Mappings;
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

        public async Task<User> AddUserAsync(AddUserDto newUser)
        {
            var employee = new User
            {
                Username = newUser.Username,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Enabled = true
            };

            _appDbContext.Users.Add(employee);
            _appDbContext.SaveChanges();

            return employee;
        }

        public async Task<UserDto?> GetUserById(long id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return user.ToDto();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(UserResourceParameter userResourceParameter)
        {
            var users = _appDbContext.Users as IQueryable<User>;

            if (string.IsNullOrEmpty(userResourceParameter.FirstName) && string.IsNullOrEmpty(userResourceParameter.LastName) && string.IsNullOrEmpty(userResourceParameter.Username))
                return await users.ToListAsync();

            if (!string.IsNullOrEmpty(userResourceParameter.FirstName))
                users = users.Where(c => c.FirstName.Contains(userResourceParameter.FirstName.Trim()));

            if (!string.IsNullOrEmpty(userResourceParameter.LastName))
                users = users.Where(c => c.LastName.Contains(userResourceParameter.LastName.Trim()));

            if (!string.IsNullOrEmpty(userResourceParameter.Username))
                users = users.AsNoTracking().Where(c => c.Username == userResourceParameter.Username.Trim());

            return await users.ToListAsync();
        }

        public async Task DeleteUserAsync(long id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
         
            if (user != null)
            {
                //_appDbContext.Users.Remove(user);
                user.Enabled = false;
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(long userId, UpdateUserDto user)
        {
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Enabled = user.Enabled;

                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
