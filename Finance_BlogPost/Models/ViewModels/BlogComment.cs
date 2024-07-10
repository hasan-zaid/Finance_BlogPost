namespace Finance_BlogPost.Models.ViewModels
{
	public class BlogComment
	{
		public Guid CommentId { get; set; }
		public string Description { get; set; }
		public DateTime PublishedDate { get; set; }
		public string Username { get; set; }
		public Guid? ParentCommentId { get; set; }

	}
}
