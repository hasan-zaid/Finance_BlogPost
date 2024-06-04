using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_BlogPost.Models.Domain
{
	public class BlogPostRejection
	{
		public Guid Id { get; set; }
		public string Reason { get; set; }

		public Guid BlogPostID { get; set; }

		[ForeignKey("BlogPostID")]
		[ValidateNever]
		public BlogPost BlogPost { get; set; }
	}
}
