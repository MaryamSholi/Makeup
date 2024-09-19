
using Makeup.Models;

namespace Makeup.ViewModel
{
	public class OrderVM
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public int OrderStatus { get; set; }
		public int Qty { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }
		public string Product { get; set; }


	}
}
