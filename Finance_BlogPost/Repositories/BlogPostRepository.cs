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

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery,
                  string? sortBy,
                  string? sortDirection,
                  int pageNumber = 1,
                  int pageSize = 100)
        {
            var query = dbContext.BlogPosts
                                 .Include(x=>x.Author)
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
                // Add other sorting conditions here if needed
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }


        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await dbContext.BlogPosts
            .Include(x => x.Tags)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id);

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
