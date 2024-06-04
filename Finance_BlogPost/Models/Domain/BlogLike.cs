namespace Finance_BlogPost.Models.Domain
{
	public class BlogLike
	{
		public Guid Id { get; set; }
		public Guid BlogPostId { get; set; }
		public Guid UserId { get; set; }
	}
}
