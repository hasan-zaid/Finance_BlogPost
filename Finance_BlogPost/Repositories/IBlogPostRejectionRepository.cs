using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
    public interface IBlogPostRejectionRepository
    {
        Task<BlogPostRejection> AddAsync(BlogPostRejection blogPostRejection);
        Task<IEnumerable<BlogPostRejection>> GetRejectedPostsAsync(string authorId,
                    string? searchQuery,
                    string? sortBy,
                    string? sortDirection);
    }
}
