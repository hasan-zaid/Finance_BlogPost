using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery,
                  string? sortBy,
                  string? sortDirection,
                  int pageNumber = 1,
                  int pageSize = 100);

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);

        Task<int> CountAsync();
    }
}
