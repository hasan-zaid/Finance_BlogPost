using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
	public interface IBookmarkPostRepository
	{
		Task<IEnumerable<BookmarkPost>> GetAllAsync();
		Task<BookmarkPost> AddAsync(BookmarkPost blogPost);
		Task<BookmarkPost?> DeleteAsync(Guid id);
		Task<IEnumerable<BookmarkPost>> GetUserBookmarkPostsAsyc(string userId);

		Task<BookmarkPost> GetUserBlogPostBookmarkAsyc(string userId, Guid blogPostId);

	}
}
