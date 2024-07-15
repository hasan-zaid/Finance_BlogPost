using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
	public interface IBlogPostRepository
	{
    // Gets all posts
    Task<IEnumerable<BlogPost>> GetAllAsync();

    Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery,
							string? sortBy,
							string? sortDirection,
							int pageNumber = 1,
							int pageSize = 100);

        Task<IEnumerable<BlogPost>> GetAllApprovedAsync(string? searchQuery,
                                string? sortBy,
                                string? sortDirection,
                                int pageNumber = 1,
                                int pageSize = 100);

        Task<IEnumerable<BlogPost>> GetAllAuthorPostsAsync(string authorId,
							string? searchQuery,
							string? sortBy,
							string? sortDirection,
							string? status,
							int pageNumber = 1,
							int pageSize = 100);

		Task<int> CountAuthorPostsAsync(string authorId);

		Task<BlogPost?> GetAsync(Guid id);

		// Gets a single post by url handle
		Task<BlogPost?> GetByUrlHandleAsync(string urlHandle); // <>

		Task<BlogPost> AddAsync(BlogPost blogPost);

		Task<BlogPost?> UpdateAsync(BlogPost blogPost);

		Task<BlogPost?> DeleteAsync(Guid id);

		Task<int> CountAsync();

		Task<int> CountByStatusAsync(string status);

		Task<IEnumerable<BlogPost>> GetPendingApprovalAsync(string? searchQuery,
							string? sortBy,
							string? sortDirection,
							int pageNumber = 1,
							int pageSize = 100);
	}
}
