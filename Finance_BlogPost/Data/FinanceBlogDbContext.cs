using Finance_BlogPost.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Data
{
    public class FinanceBlogDbContext : DbContext
    {
        public FinanceBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
