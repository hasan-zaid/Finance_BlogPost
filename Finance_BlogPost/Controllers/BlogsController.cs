using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
	// BlogsController is responsible for handling blog related operations
	// It interacts with the IBlogPostRepository to access blog post data, which in turn uses the DbContext to communicate with the database
	// It interacts with the IBlogPostLikeRepository to access blog post like data, which in turn uses the DbContext to communicate with the database
	// It interacts with the SignInManager service to manage user sign-in
	// It interacts with the UserManager service to manage identity users

	public class BlogsController : Controller
	{
		// Repository for accessing blog posts
		private readonly IBlogPostRepository blogPostRepository;
		private readonly IBlogPostLikeRepository blogPostLikeRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		// Constructor to initialize the blog post repository, blog post like repository, sign-in manager, and user manager
		public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
		{
			this.blogPostRepository = blogPostRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		// Action method to handle GET requests with a URL parameter (e.g., /Blogs?urlHandle=some-handle)
		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			// Retrieve the blog post by its URL handle asynchronously
			var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

			// Create a boolean variable to indicate if the user has liked the blog post
			var liked = false;

			// Create a new BlogDetailsViewModel which has the TotalLikes property. Hence, the blogpost and the total likes can be accessed in the view
			var blogDetailsViewModel = new BlogDetailsViewModel();

			// Check if the blog post exists
			if (blogPost != null)
			{
				// Get the total number of likes for the blog post
				var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

				// Check if the user is signed in
				if (signInManager.IsSignedIn(User))
				{
					// Get the likes for the blog post
					var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

					// Get the user ID from the user manager
					var userId = userManager.GetUserId(User);

					// Check if the user has liked the blog post
					if (userId != null)
					{
						// Find the like for the user
						var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == userId);
						// Assign the result to the liked variable when the like is found
						liked = likeFromUser != null;
					}
				}

				// Map the retrieved blog post to the BlogDetailsViewModel
				blogDetailsViewModel = new BlogDetailsViewModel
				{
					Id = blogPost.Id,
					Content = blogPost.Content,
					PageTitle = blogPost.PageTitle,
					Author = blogPost.Author,
					BlogImageUrl = blogPost.BlogImageUrl,
					Heading = blogPost.Heading,
					PublishedDate = blogPost.PublishedDate,
					ShortDescription = blogPost.ShortDescription,
					UrlHandle = blogPost.UrlHandle,
					Visible = blogPost.Visible,
					Tags = blogPost.Tags,
					TotalLikes = totalLikes,
					Liked = liked,
				};
			}

			// Return the view with the BlogDetailsViewModel
			return View(blogDetailsViewModel);
		}
	}
}
