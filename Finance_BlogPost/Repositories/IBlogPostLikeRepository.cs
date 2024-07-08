using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
	public interface IBlogPostLikeRepository
	{
		// Get total number of likes for a specific blog post
		Task<int> GetTotalLikes(Guid blogPostId);

		// Get all likes for a specific blog post
		Task<IEnumerable<BlogLike>> GetLikesForBlog(Guid blogPostId);

		// Add a like for a blog post
		Task<BlogLike> AddLikeForBlog(BlogLike blogLike);

		// Remove a like for a blog post
		Task<BlogLike?> RemoveLikeForBlog(BlogLike blogLike);
	}
}
