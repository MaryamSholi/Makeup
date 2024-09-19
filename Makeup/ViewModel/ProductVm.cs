namespace Makeup.ViewModel
{
	public class ProductVm
	{
		public int id { get; set; }
		public string ProductName { get; set; } = null!;
		public DateTime PublishDate { get; set; } = DateTime.Now;
		public string ImageUrl { get; set; }
		public string Description { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public string Brand { get; set; }
		public string Category { get; set; }

	}
}
