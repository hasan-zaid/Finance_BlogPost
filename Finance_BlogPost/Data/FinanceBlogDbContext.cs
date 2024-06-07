using Finance_BlogPost.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Data
{
    public class FinanceBlogDbContext : IdentityDbContext
	{
        public FinanceBlogDbContext(DbContextOptions<FinanceBlogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }

		public DbSet<BlogComment> Comments { get; set; }

		public DbSet<BlogLike> Likes { get; set; }

		public DbSet<BlogPostRejection> BlogPostRejections { get; set; }
        public DbSet<UserProfileImage> UserProfileImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			// Seed Roles (User, Admin, Author)

			var adminRoleId = "3206215b-93fb-4b5a-8f8e-119f04ffa4b8";
			var authorRoleId = "83d2e9d0-1f2f-45c1-bfad-5af285149618";
			var userRoleId = "a5894f61-62c2-4c1b-9b28-2f7935004df3";

			var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Name= "Admin",
					NormalizedName = "Admin",
					Id = adminRoleId,
					ConcurrencyStamp = adminRoleId
				},
				new IdentityRole
				{
					Name = "Author",
					NormalizedName = "Author",
					Id = authorRoleId,
					ConcurrencyStamp = authorRoleId
				},
				new IdentityRole
				{
					Name = "User",
					NormalizedName = "User",
					Id = userRoleId,
					ConcurrencyStamp = userRoleId
				}
			};

			builder.Entity<IdentityRole>().HasData(roles);

			// Seed AdminUser
			var adminId = "9a5a7e6e-a0d9-4454-9484-3e4b0a105271";
			var adminUser = new IdentityUser
			{
				UserName = "admin@finblog.com",
				Email = "admin@finblog.com",
				NormalizedEmail = "admin@finblog.com".ToUpper(),
				NormalizedUserName = "admin@finblog.com".ToUpper(),
				Id = adminId
			};

			adminUser.PasswordHash = new PasswordHasher<IdentityUser>()
				.HashPassword(adminUser, "admin@123");


			builder.Entity<IdentityUser>().HasData(adminUser);


			// Add admin role to admin
			var adminRoles = new List<IdentityUserRole<string>>
			{
				new IdentityUserRole<string>
				{
					RoleId = adminRoleId,
					UserId = adminId
				}
			};



			builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);


            //Seed Author

            var authorId = "7d9cfb34-fa14-42f6-b39f-0064019a10ec";
            var author = new IdentityUser
            {
                UserName = "author@finblog.com",
                Email = "author@finblog.com",
                NormalizedEmail = "author@finblog.com".ToUpper(),
                NormalizedUserName = "author@finblog.com".ToUpper(),
                Id = authorId
            };

            author.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(author, "author@123");


            builder.Entity<IdentityUser>().HasData(author);


            // Add role to Author
            var authorRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = authorRoleId,
                    UserId = authorId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(authorRoles);

        }
	}
}
