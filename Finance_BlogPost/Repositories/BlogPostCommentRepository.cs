using System.Reflection.Metadata;
using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
	// BlogPostCommentRepository handles the data access logic for blog post comments.

	public class BlogPostCommentRepository : IBlogPostCommentRepository
	{
		// Database context for accessing the database
		private readonly FinanceBlogDbContext financeBlogDbContext;

		// Constructor to inject the database context
		public BlogPostCommentRepository(FinanceBlogDbContext financeBlogDbContext)
		{
			this.financeBlogDbContext = financeBlogDbContext;
		}

		// Adds a new blog post comment to the database
		public async Task<BlogComment> AddAsync(BlogComment blogComment)
		{
			await financeBlogDbContext.Comments.AddAsync(blogComment);
			await financeBlogDbContext.SaveChangesAsync();

			return blogComment;
		}

		// Retrieves from the database all the blog post comments for a specific blog post
		public async Task<IEnumerable<BlogComment>> GetCommentsByBlogId(Guid blogPostId)
		{
			return await financeBlogDbContext.Comments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
		}
	}
}
