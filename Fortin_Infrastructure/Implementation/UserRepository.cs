using Fortin.Common;
using Fortin.Common.Configuration;
using Fortin.Infrastructure.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClientFactory;
        private readonly ConnectionStrings _connectionStrings;

        public UserRepository(IHttpClientFactory httpClientFactory, IOptionsMonitor<ConnectionStrings> options)
        {
            _httpClientFactory = httpClientFactory.CreateClient();
            _connectionStrings = options.CurrentValue;
        }

        public async Task<User> GetById(int id)
        {
            User user = new();

            using (SqlConnection connection = new SqlConnection(_connectionStrings.FortinCommon))
            {
                string queryStatement = $"SELECT * FROM dbo.[Users] where Id={id} ORDER BY Id";

                using (SqlCommand command = new SqlCommand(queryStatement, connection))
                {
                    await connection.OpenAsync();

                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            user = new User()
                            {
                                Id = (long)reader["Id"],
                                Username = reader["Username"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString()
                            };
                        }

                    }
                }
            }

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> userList = new();

            using(SqlConnection connection = new SqlConnection(_connectionStrings.FortinCommon))
            {
                string queryStatement = $"SELECT * FROM dbo.[Users] ORDER BY Id";

                using(SqlCommand command = new SqlCommand(queryStatement, connection))
                {
                    await connection.OpenAsync();

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        User user = new()
                        {
                            Id = (long)reader["Id"],
                            Username = reader["Username"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        };

                        userList.Add(user);
                    }
                }
            }

            return userList;
        }
    }
}
