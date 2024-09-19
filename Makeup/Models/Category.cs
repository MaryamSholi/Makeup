using Microsoft.EntityFrameworkCore;

namespace Makeup.Models
{
	[Index(nameof(Name), IsUnique = true)]
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;
		public virtual ICollection<Product> Products { get; set; }

	}
}
