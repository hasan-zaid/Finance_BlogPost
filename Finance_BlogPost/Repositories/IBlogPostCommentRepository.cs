using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
	public interface IBlogPostCommentRepository
	{
		// Adds a new comment for a specific blog post to the database.
		Task<BlogComment> AddAsync(BlogComment blogComment);

		// Returns all comments for a specific blog post from the database.
		Task<IEnumerable<BlogComment>> GetCommentsByBlogId(Guid blogPostId);

		// Return a specific comment for a specific blog post from the database.
		Task<BlogComment?> GetAsync(Guid blogCommentId);

		// Deletes a comment for a specific blog post from the database.
		Task<bool> DeleteCommentWithRepliesAsync(Guid blogCommentId);

		// Updates a comment for a specific blog post in the database.
		Task<BlogComment?> UpdateAsync(BlogComment blogComment);
	}
}
