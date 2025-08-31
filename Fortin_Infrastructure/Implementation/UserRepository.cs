using Fortin.Common;
using Fortin_Infrastructure.Interface;
using Microsoft.Data.SqlClient;
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

        public UserRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory.CreateClient();
        }

        public async Task<User> GetById(int id)
        {
            User user = new();
            string connectionString = "Data Source=localhost;Initial Catalog=Fortin_Common;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            using (SqlConnection connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=Fortin_Common;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                string queryStatement = $"SELECT * FROM dbo.User where Id={id} ORDER BY Id";

                using (SqlCommand command = new SqlCommand(queryStatement, connection))
                {
                    await connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
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
    }
}
