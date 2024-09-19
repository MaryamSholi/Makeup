
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Makeup.ViewModel
{
    public class ProductFormVM
    {
        public int id {  get; set; }
        [MaxLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; } = DateTime.Now;
        [Display(Name = "Choose Image...")]
        public IFormFile? ImageUrl { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public List<SelectListItem>? Brand { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<SelectListItem>? Category { get; set; }

    }
}
