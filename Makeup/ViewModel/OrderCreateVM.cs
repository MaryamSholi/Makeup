using Makeup.Models;

namespace Makeup.ViewModel
{
    public class OrderCreateVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }  // Unit price of the product
        public int Qty { get; set; }  // User-entered quantity
        public decimal TotalPrice => Price * Qty;  // Automatically calculated total price
        public string UserId { get; set; }

    }
}
