using System.ComponentModel.DataAnnotations;

namespace AbcRetail.Models.ViewModels
{
    public class ProductCreateViewModels
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public int Stock { get; set; }
        public IFormFile Image { get; set; }
    }
}
