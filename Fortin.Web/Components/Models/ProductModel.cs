using System.ComponentModel.DataAnnotations;

namespace Fortin.Web.Components.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ProductNumber { get; set; } = string.Empty;
        [Required]
        public decimal ListPrice { get; set; }
        [Required]
        public string Color { get; set; } = string.Empty;
        [Required]
        public string ModelName { get; set; } = string.Empty;
    }
}
