namespace Makeup.ViewModel
{
	public class OrderFormVM
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public int OrderStatus { get; set; } = 0;
		public decimal TotalPrice { get; set; }
		public string UserId { get; set; }
	}
}
