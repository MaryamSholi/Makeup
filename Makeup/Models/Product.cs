namespace Makeup.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public string Description { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public int BrandId { get; set; }
		public Brand Brand { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;
	}
}
