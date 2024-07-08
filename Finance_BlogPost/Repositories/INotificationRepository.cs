using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();

        Task<IEnumerable<Notification>> GetUserAsync(string userId);

        Task<Notification?> GetAsync(Guid id);

        Task<int> CountAsync();

        Task<int> CountUnreadAsync(string userId);

        void Update(Notification notification);

        Notification Add(Notification flashcard);

    }
}
