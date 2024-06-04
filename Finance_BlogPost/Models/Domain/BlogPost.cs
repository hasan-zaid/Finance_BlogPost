using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_BlogPost.Models.Domain
{
    public class BlogPost
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
        public IdentityUser  Author { get; set; }
        public bool Visible { get; set; }
        public string Approval { get; set; }

        // Navigation property
        public ICollection<Tag> Tags { get; set; }
		public ICollection<BlogLike> Likes { get; set; }
		public ICollection<BlogComment> Comments { get; set; }
	}
}
