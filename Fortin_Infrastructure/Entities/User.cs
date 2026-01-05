using Fortin.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Infrastructure.Entities
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public UserRole Role { get; set; } = UserRole.Customer;
        public bool Enabled { get; set; }
    }
}
