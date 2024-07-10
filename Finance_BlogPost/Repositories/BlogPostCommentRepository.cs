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

		// Retrive from the database a specific blog post comment
		public async Task<BlogComment?> GetAsync(Guid blogCommentId)
		{
			return await financeBlogDbContext.Comments.FirstOrDefaultAsync(x => x.Id == blogCommentId);
		}

		// Deletes a blog post comment and its replies from the database
		public async Task<bool> DeleteCommentWithRepliesAsync(Guid blogCommentId)
		{
			// Retrieve the comment and its replies
			var commentsToDelete = await financeBlogDbContext.Comments
					.Where(c => c.Id == blogCommentId || c.ParentCommentId == blogCommentId)
					.ToListAsync();

			if (commentsToDelete.Any())
			{
				financeBlogDbContext.Comments.RemoveRange(commentsToDelete);
				await financeBlogDbContext.SaveChangesAsync();
				return true;
			}
			return false;
		}


		// Updates a blog post comment in the database
		public async Task<BlogComment?> UpdateAsync(BlogComment blogComment)
		{
			var existingBlogComment = await financeBlogDbContext.Comments.FindAsync(blogComment.Id);

			if (existingBlogComment != null)
			{
				existingBlogComment.Id = blogComment.Id;
				existingBlogComment.Description = blogComment.Description;
				existingBlogComment.BlogPostId = blogComment.BlogPostId;
				existingBlogComment.UserId = blogComment.UserId;
				existingBlogComment.PublishedDate = blogComment.PublishedDate;
				// save changes
				await financeBlogDbContext.SaveChangesAsync();
				return existingBlogComment;
			}
			return null;
		}
	}
}
