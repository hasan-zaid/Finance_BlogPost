using Finance_BlogPost.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_BlogPost.Models.ViewModels
{
	public class BlogDetailsViewModel
	{
		public Guid Id { get; set; }
		public string Heading { get; set; }
		public string PageTitle { get; set; }
		public string Content { get; set; }
		public string ShortDescription { get; set; }
		public string BlogImageUrl { get; set; }
		public string UrlHandle { get; set; }
		public DateTime PublishedDate { get; set; }

		public string AuthorId { get; set; }

		[ForeignKey("AuthorId")]
		[ValidateNever]
		public IdentityUser Author { get; set; }
		public bool Visible { get; set; }
		public string Approval { get; set; }
		public ICollection<Domain.Tag> Tags { get; set; }

		// props for the number of likes the blog has
		public int TotalLikes { get; set; }
		// props for whether the user has liked the blog
		public bool Liked { get; set; }
		// props for storing the text content of a single comment
		public string CommentDescription { get; set; }
		// props for storing multiple comments associated with a blog post
		public Guid? ParentCommentId { get; set; }
		public IEnumerable<BlogComment> Comments { get; set; }
	}
}
