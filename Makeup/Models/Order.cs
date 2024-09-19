using Microsoft.AspNetCore.Identity;

namespace Makeup.Models
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public int OrderStatus { get; set; }
		public int Qty {  get; set; } 
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		//public virtual ICollection<OrderDetails> OrderDetails { get; set; }
	}
}
