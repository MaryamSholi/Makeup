using System.ComponentModel.DataAnnotations;

namespace Makeup.ViewModel
{
	public class CategoryFormVM
	{
		public int Id { get; set; }
		[MaxLength(50, ErrorMessage = "The Name Should Be 50 Characters")]
		public string Name { get; set; } = null!;
	}
}
