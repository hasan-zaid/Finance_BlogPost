using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Finance_BlogPost.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_BlogPost.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IUserRepository userRepository;

        public AdminDashboardController(IUserRepository userRepository, ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }


        public async Task<IActionResult> Index()
        {
            // Fetch counts
            var totalBlogPosts = await blogPostRepository.CountAsync();
            var totalTags = await tagRepository.CountAsync();
            var totalUsers = await userRepository.CountAsync();
            var pendingApprovals = await blogPostRepository.CountByStatusAsync(BlogPostApproval.Pending);
            var rejectedPosts = await blogPostRepository.CountByStatusAsync(BlogPostApproval.Rejected);

            // Fetch recent blog posts and related author names
            var recentBlogPostsData = await blogPostRepository.GetAllAsync(null, null, null, 1, 100);
            var recentBlogPosts = new List<BlogPostSummary>();
            foreach (var bp in recentBlogPostsData.OrderByDescending(bp => bp.PublishedDate).Take(5))
            {
                var author = await userRepository.GetAsync(bp.AuthorId);
                recentBlogPosts.Add(new BlogPostSummary
                {
                    Heading = bp.Heading,
                    PublishedDate = bp.PublishedDate,
                    AuthorName = author.UserName
                });
            }

            // Fetch tags and process tag usage
            var tagData = await tagRepository.GetAllAsync();
            var tagUsage = tagData.Select(tag => new TagSummary
            {
                Name = tag.Name,
                UsageCount = tag.BlogPosts.Count
            })
            .OrderByDescending(ts => ts.UsageCount)
            .Take(5)
            .ToList();

            var model = new AdminDashboardViewModel
            {
                TotalBlogPosts = totalBlogPosts,
                TotalTags = totalTags,
                TotalUsers = totalUsers,
                PendingApprovals = pendingApprovals,
                RejectedPosts = rejectedPosts,
                RecentBlogPosts = recentBlogPosts,
                TagUsage = tagUsage
            };

            return View(model);
        }



    }
}
