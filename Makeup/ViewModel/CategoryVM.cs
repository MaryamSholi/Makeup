using Makeup.Models;
using System.ComponentModel.DataAnnotations;

namespace Makeup.ViewModel
{
	public class CategoryVM
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Please Enter Category Name")]
		[MaxLength(50, ErrorMessage = "max length should be 100 characters")]
		public string Name { get; set; } = null!;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;
		public virtual ICollection<Product> Products { get; set; } = new List<Product>();

	}
}
