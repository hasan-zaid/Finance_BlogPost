using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
	public class BookmarkPostRepository : IBookmarkPostRepository
	{
		private readonly FinanceBlogDbContext dbContext;

		public BookmarkPostRepository(FinanceBlogDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IEnumerable<BookmarkPost>> GetAllAsync()
		{
			return await dbContext.BookmarkPosts.ToListAsync();

		}

		public async Task<BookmarkPost?> DeleteAsync(Guid id)
		{
			var existingPost = await dbContext.BookmarkPosts.FindAsync(id);

			if (existingPost != null)
			{
				dbContext.BookmarkPosts.Remove(existingPost);
				await dbContext.SaveChangesAsync();
				return existingPost;
			}

			return null;
		}


		public async Task<BookmarkPost> AddAsync(BookmarkPost blogPost)
		{
			await dbContext.BookmarkPosts.AddAsync(blogPost);
			await dbContext.SaveChangesAsync();
			return blogPost;
		}

		public async Task<IEnumerable<BookmarkPost>> GetUserBookmarkPostsAsyc(string userId)
		{
			return await dbContext.BookmarkPosts.Where(b => b.User.Id == userId).ToListAsync();
		}


		public async Task<BookmarkPost> GetUserBlogPostBookmarkAsyc(string userId, Guid blogPostId)
		{
			return await dbContext.BookmarkPosts.Where(b => b.User.Id == userId && b.BlogPostId == blogPostId).Include(b => b.BlogPost).FirstOrDefaultAsync();
		}
	}
}
