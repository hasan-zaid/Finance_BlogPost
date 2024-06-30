using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
	// BlogsController is responsible for displaying the blog posts
	// It interacts with the IBlogPostRepository to access blog post data, which in turn uses the DbContext to communicate with the database

	public class BlogsController : Controller
	{
		// Repository for accessing blog posts
		private readonly IBlogPostRepository blogPostRepository;

		// Constructor to initialize the blog post repository
		public BlogsController(IBlogPostRepository blogPostRepository)
		{
			this.blogPostRepository = blogPostRepository;
		}

		// Action method to handle GET requests with a URL parameter (e.g., /Blogs?urlHandle=some-handle)
		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			// Retrieve the blog post by its URL handle asynchronously
			var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

			// Pass the blog post to the view
			return View(blogPost);
		}
	}
}
