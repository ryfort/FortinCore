using System.ComponentModel.DataAnnotations;

namespace Fortin.Common
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool Enabled { get; set; }
    }
}