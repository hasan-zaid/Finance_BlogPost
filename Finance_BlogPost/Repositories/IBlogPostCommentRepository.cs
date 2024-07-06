using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
	public interface IBlogPostCommentRepository
	{
		// Adds a new comment for a specific blog post to the database.
		Task<BlogComment> AddAsync(BlogComment blogComment);

		// Returns all comments for a specific blog post from the database.
		Task<IEnumerable<BlogComment>> GetCommentsByBlogId(Guid blogPostId);
	}
}
