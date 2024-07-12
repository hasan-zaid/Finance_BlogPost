using Finance_BlogPost.Models.Domain;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
	public class BookmarkPostController : Controller
	{
		private readonly IBookmarkPostRepository bookmarkPostRepository;
		private readonly IBlogPostRepository blogPostRepository;
		private readonly UserManager<IdentityUser> userManager;

		public BookmarkPostController(IBookmarkPostRepository bookmarkPostRepository, UserManager<IdentityUser> userManager, IBlogPostRepository blogPostRepository)
		{
			this.bookmarkPostRepository = bookmarkPostRepository;
			this.userManager = userManager;
			this.blogPostRepository = blogPostRepository;
		}

		public async Task<IActionResult> Index()
		{
			var userId = this.userManager.GetUserId(User);
			// Retrieve all bookmarked post associated with the user asynchronously
			var bookmarks = await this.bookmarkPostRepository.GetUserBookmarkPostsAsyc(userId);

			// Extract the BlogPostIds from the bookmarks
			var blogPostIds = bookmarks.Select(b => b.BlogPostId).Distinct().ToList();

			// Fetch each BlogPost based on the BlogPostId
			List<BlogPost> blogPosts = new List<BlogPost>();
			foreach (var blogPostId in blogPostIds)
			{
				var blogPost = await blogPostRepository.GetAsync((Guid)blogPostId);
				if (blogPost != null)
				{
					blogPosts.Add(blogPost);
				}

			}
			return View(blogPosts);
		}

		[HttpPost]
		public async Task<IActionResult> AddBookmark(Guid blogPostId)
		{
			var userId = this.userManager.GetUserId(User);
			var bookmark = new BookmarkPost
			{
				Id = Guid.NewGuid(),
				Date = DateTime.Now,
				BlogPostId = blogPostId,
				UserId = userId
			};
			await this.bookmarkPostRepository.AddAsync(bookmark);
			return Json(new { success = true });
		}

		[HttpPost]
		public async Task<IActionResult> RemoveBookmark(Guid blogPostId)
		{
			var userId = this.userManager.GetUserId(User);
			var bookmark = await this.bookmarkPostRepository.GetUserBlogPostBookmarkAsyc(userId, blogPostId);
			if (bookmark != null)
			{
				await this.bookmarkPostRepository.DeleteAsync(bookmark.Id);
			}

			return Json(new { success = true });
		}
	}
}
