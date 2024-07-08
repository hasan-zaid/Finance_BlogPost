using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FinanceBlogDbContext dbContext;

        public NotificationRepository(FinanceBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await dbContext.Notifications.ToListAsync();

        }


        public async Task<IEnumerable<Notification>> GetUserAsync(string userId)
        {
            return await dbContext.Notifications
                            .Where(n => n.UserId == userId)
                            .ToListAsync();
        }


        public async Task<Notification?> GetAsync(Guid id)
        {
            return await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await dbContext.Notifications.CountAsync();
        }

        public async Task<int> CountUnreadAsync(string userId)
        {
            return await dbContext.Notifications.Where(n => n.UserId == userId && !n.IsRead).CountAsync();
        }

        public void Update(Notification notification)
        {
            dbContext.Notifications.Update(notification);
            dbContext.SaveChanges();
        }

        public Notification Add(Notification notification)
        {
            dbContext.Add(notification);
            dbContext.SaveChanges();
            return notification;
        }
    }
}
