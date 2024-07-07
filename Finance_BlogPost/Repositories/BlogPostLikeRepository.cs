using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
	public class BlogPostLikeRepository : IBlogPostLikeRepository
	{
		private readonly FinanceBlogDbContext financeBlogDbContext;

		public BlogPostLikeRepository(FinanceBlogDbContext financeBlogDbContext)
		{
			this.financeBlogDbContext = financeBlogDbContext;
		}

		// Adds a like for a specific blog post to the database.
		public async Task<BlogLike> AddLikeForBlog(BlogLike blogLike)
		{
			await financeBlogDbContext.Likes.AddAsync(blogLike);
			await financeBlogDbContext.SaveChangesAsync();

			return blogLike;
		}

		// Retrieves from the database all the likes for a specific blog post.
		public async Task<IEnumerable<BlogLike>> GetLikesForBlog(Guid blogPostId)
		{
			return await financeBlogDbContext.Likes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
		}

		// Retrieves from the database the total number of likes for a specific blog post.
		public async Task<int> GetTotalLikes(Guid blogPostId)
		{
			return await financeBlogDbContext.Likes.CountAsync(x => x.BlogPostId == blogPostId);
		}

		public async Task<BlogLike?> RemoveLikeForBlog(BlogLike blogLike)
		{
			// Check if the user has already liked the blog post.
			var likeToRemove = await financeBlogDbContext.Likes.FirstOrDefaultAsync(x => x.BlogPostId == blogLike.BlogPostId && x.UserId == blogLike.UserId);

			// If the user liked the blog post, remove it from the database.
			if (likeToRemove != null)
			{
				financeBlogDbContext.Likes.Remove(likeToRemove);
				await financeBlogDbContext.SaveChangesAsync();
				return likeToRemove;
			}
			return null;
		}
	}
}
