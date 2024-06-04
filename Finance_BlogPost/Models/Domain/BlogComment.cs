namespace Finance_BlogPost.Models.Domain
{
	public class BlogComment
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public Guid BlogPostId { get; set; }
		public Guid UserId { get; set; }
		public DateTime PublishedDate { get; set; }

		public Guid ParentCommentId { get; set; }
	}
}
