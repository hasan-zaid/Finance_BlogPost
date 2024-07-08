using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_BlogPost.Models.Domain
{
	public class BookmarkPost
	{
		public Guid Id { get; set; }

		public DateTime Date { get; set; }

		public Guid? BlogPostId { get; set; }
		[ForeignKey("BlogPostId")]
		[ValidateNever]
		public BlogPost BlogPost { get; set; }
		public string? UserId { get; set; }

		[ForeignKey("UserId")]
		[ValidateNever]
		public IdentityUser User { get; set; }
	}
}
