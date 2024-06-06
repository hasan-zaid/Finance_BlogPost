using Microsoft.AspNetCore.Identity;

namespace Finance_BlogPost.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync(string? searchQuery = null,
        string? sortBy = null,
        string? sortDirection = null,
        int pageNumber = 1,
        int pageSize = 100);

        Task<int> CountAsync();
    }
}
