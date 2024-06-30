using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Finance_BlogPost.Repositories
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly FinanceBlogDbContext dbContext;

		public BlogPostRepository(FinanceBlogDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<BlogPost> AddAsync(BlogPost blogPost)
		{
			await dbContext.AddAsync(blogPost);
			await dbContext.SaveChangesAsync();
			return blogPost;
		}

		public async Task<BlogPost?> DeleteAsync(Guid id)
		{
			var existingBlog = await dbContext.BlogPosts.FindAsync(id);

			if (existingBlog != null)
			{
				dbContext.BlogPosts.Remove(existingBlog);
				await dbContext.SaveChangesAsync();
				return existingBlog;
			}

			return null;
		}

    // Returns all blog posts from the database
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
      // Returns the list of blog posts from the database and includes the list of tags associated with each blog post
      return await dbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery,
							string? sortBy,
							string? sortDirection,
							int pageNumber = 1,
							int pageSize = 100)
		{
			var query = dbContext.BlogPosts
													 .Include(x => x.Author)
													 .Include(x => x.Tags) // Include Tags
													 .AsQueryable();

			// Filtering
			if (string.IsNullOrWhiteSpace(searchQuery) == false)
			{
				query = query.Where(x => x.Heading.Contains(searchQuery));
			}

			// Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

				if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Heading) : query.OrderBy(x => x.Heading);
				}
				else if (string.Equals(sortBy, "PublishedDate", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.PublishedDate) : query.OrderBy(x => x.PublishedDate);
				}
			}

			// Pagination
			var skipResults = (pageNumber - 1) * pageSize;
			query = query.Skip(skipResults).Take(pageSize);

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<BlogPost>> GetAllAuthorPostsAsync(string authorId, string? searchQuery,
							string? sortBy,
							string? sortDirection,
							string? status,
							int pageNumber = 1,
							int pageSize = 100)
		{
			var query = dbContext.BlogPosts
													 .Include(x => x.Author)
													 .Include(x => x.Tags) // Include Tags
													 .AsQueryable();

			// Filtering
			if (string.IsNullOrWhiteSpace(searchQuery) == false)
			{
				query = query.Where(x => x.Heading.Contains(searchQuery));
			}
			if (string.IsNullOrWhiteSpace(status) == false && !status.Equals("Deleted"))
			{
				if (!status.Equals("All"))
				{
					query = query.Where(x => x.AuthorId == authorId && x.Approval == status);
				}
			}

			query = query.Where(x => x.AuthorId == authorId);

			// Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

				if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Heading) : query.OrderBy(x => x.Heading);
				}
				else if (string.Equals(sortBy, "PublishedDate", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("Sorting by Published Date");
					query = isDesc ? query.OrderByDescending(x => x.PublishedDate) : query.OrderBy(x => x.PublishedDate);
				}
			}

			// Pagination
			var skipResults = (pageNumber - 1) * pageSize;
			query = query.Skip(skipResults).Take(pageSize);

			return await query.ToListAsync();
		}

		// Returns a specific blog post from the database by its id
		public async Task<BlogPost?> GetAsync(Guid id)
		{
			return await dbContext.BlogPosts
			.Include(x => x.Tags)
			.Include(x => x.Author)
			.FirstOrDefaultAsync(x => x.Id == id);

		}

		// Returns a specific blog post from the database by its url handle
		public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
		{
			// Returns the blog post from the database and includes the list of tags associated with the blog post
			return await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
		}

		public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
		{
			var existingBlog = await dbContext.BlogPosts.Include(x => x.Tags)
					.FirstOrDefaultAsync(x => x.Id == blogPost.Id);

			if (existingBlog != null)
			{
				existingBlog.Id = blogPost.Id;
				existingBlog.Heading = blogPost.Heading;
				existingBlog.PageTitle = blogPost.PageTitle;
				existingBlog.Content = blogPost.Content;
				existingBlog.ShortDescription = blogPost.ShortDescription;
				existingBlog.BlogImageUrl = blogPost.BlogImageUrl;
				existingBlog.UrlHandle = blogPost.UrlHandle;
				existingBlog.Visible = blogPost.Visible;
				existingBlog.PublishedDate = blogPost.PublishedDate;
				existingBlog.Tags = blogPost.Tags;
				existingBlog.Approval = blogPost.Approval;

				await dbContext.SaveChangesAsync();
				return existingBlog;
			}

			return null;
		}

		public async Task<int> CountAsync()
		{
			return await dbContext.BlogPosts.CountAsync();
		}

		public async Task<int> CountAuthorPostsAsync(string authorId)
		{
			return await dbContext.BlogPosts
														.Where(bp => bp.AuthorId == authorId).CountAsync();
		}

		public async Task<int> CountByStatusAsync(string status)
		{
			return await dbContext.BlogPosts
														.Where(bp => bp.Approval == status)
														.CountAsync();
		}



		public async Task<IEnumerable<BlogPost>> GetPendingApprovalAsync(string? searchQuery,
							string? sortBy,
							string? sortDirection,
							int pageNumber = 1,
							int pageSize = 100)
		{

			var query = dbContext.BlogPosts
												.Include(x => x.Author)
												.Include(x => x.Tags)
												 .Where(x => x.Approval == "Pending")
												.AsQueryable();

			// Filtering
			if (string.IsNullOrWhiteSpace(searchQuery) == false)
			{
				query = query.Where(x => x.Heading.Contains(searchQuery) ||
																 x.Author.UserName.Contains(searchQuery));
			}

			// Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

				if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Heading) : query.OrderBy(x => x.Heading);
				}

				if (string.Equals(sortBy, "Author", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Author.UserName) : query.OrderBy(x => x.Author.UserName);
				}
			}

			// Pagination
			var skipResults = (pageNumber - 1) * pageSize;
			query = query.Skip(skipResults).Take(pageSize);

			return await query.ToListAsync();

		}

	}
}
