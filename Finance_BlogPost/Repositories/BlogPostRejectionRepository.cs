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
    }
}
