using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Makeup.Models
{
	public class ApplicationUser : IdentityUser
	{
		[MaxLength(30)]
		public string Address { get; set; }
		public bool IsDeleted { get; set; } = false;
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime UpdatedOn { get; set; } = DateTime.Now;
	}
}
