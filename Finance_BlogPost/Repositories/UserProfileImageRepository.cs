using Finance_BlogPost.Data;
using Finance_BlogPost.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Repositories
{
    public class UserProfileImageRepository : IUserProfileImageRepository
    {
        private readonly FinanceBlogDbContext dbContext;

        public UserProfileImageRepository(FinanceBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserProfileImage> AddAsync(UserProfileImage image)
        {
            await dbContext.UserProfileImages.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<UserProfileImage?> DeleteAsync(Guid id)
        {
            var existingUserProfile = await dbContext.UserProfileImages.FindAsync(id);

            if (existingUserProfile != null)
            {
                dbContext.UserProfileImages.Remove(existingUserProfile);
                await dbContext.SaveChangesAsync();
                return existingUserProfile;
            }

            return null;
        }

        public async Task<IEnumerable<UserProfileImage>> GetAllAsync()
        {
            return await dbContext.UserProfileImages.Include(x => x.User).ToListAsync();
        }


        public Task<UserProfileImage?> GetAsync(Guid id)
        {
            return dbContext.UserProfileImages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserProfileImage?> UpdateAsync(UserProfileImage image)
        {
            var existingUserProfileImage = await dbContext.UserProfileImages.FindAsync(image.Id);

            if (existingUserProfileImage != null)
            {
                existingUserProfileImage.ProfileImageUrl = image.ProfileImageUrl;
                existingUserProfileImage.UserID = image.UserID;

                await dbContext.SaveChangesAsync();

                return existingUserProfileImage;
            }

            return null;
        }

        public Task<UserProfileImage?> GetByUserAsync(string userId)
        {
            return dbContext.UserProfileImages.FirstOrDefaultAsync(x => x.UserID == userId);
        }


    }
}
