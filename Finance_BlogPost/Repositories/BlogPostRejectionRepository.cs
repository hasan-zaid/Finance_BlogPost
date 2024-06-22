using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
    public class BlogPostRejectionRepository : IBlogPostRejectionRepository
    {
        private readonly FinanceBlogDbContext dbContext;

        public BlogPostRejectionRepository(FinanceBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPostRejection> AddAsync(BlogPostRejection blogPostRejection)
        {
            await dbContext.AddAsync(blogPostRejection);
            await dbContext.SaveChangesAsync();
            return blogPostRejection;
        }

        public async Task<IEnumerable<BlogPostRejection>> GetRejectedPostsAsync(string authorId,
                    string? searchQuery,
                    string? sortBy,
                    string? sortDirection)
        {
            var query = dbContext.BlogPostRejections.Include(x => x.BlogPost).Include(x => x.BlogPost.Tags).Include(x => x.BlogPost.Author).Where(x => x.BlogPost.AuthorId == authorId && x.BlogPost.Approval == "Rejected").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.BlogPost.Heading.Contains(searchQuery));
            }
            
            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.BlogPost.Heading) : query.OrderBy(x => x.BlogPost.Heading);
                }
                else if (string.Equals(sortBy, "PublishedDate", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Sorting by Published Date");
                    query = isDesc ? query.OrderByDescending(x => x.BlogPost.PublishedDate) : query.OrderBy(x => x.BlogPost.PublishedDate);
                }
            }
            return await query.ToListAsync();
        }
    }
}
