using Finance_BlogPost.Models.Domain;
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
	// It interacts with the IBlogPostCommentRepository to access blog post comment data, which in turn uses the DbContext to communicate with the database

	public class BlogsController : Controller
	{
		// Repository for accessing blog posts
		private readonly IBlogPostRepository blogPostRepository;
		private readonly IBlogPostLikeRepository blogPostLikeRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IBlogPostCommentRepository blogPostCommentRepository;

		// Constructor to initialize the blog post repository, blog post like repository, sign-in manager, user manager, and the blog post comment repository
		public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
		{
			this.blogPostRepository = blogPostRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.blogPostCommentRepository = blogPostCommentRepository;
		}

		// Action method to handle GET requests with a URL parameter (e.g., /Blogs?urlHandle=some-handle)
		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			// ==============================================================================
			// METHOD SIGNATURE AND INITIALIZATION
			// ==============================================================================

			// Retrieve the blog post by its URL handle asynchronously
			var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

			// Create a boolean variable to indicate if the user has liked the blog post
			var liked = false;

			// Create a new BlogDetailsViewModel which has the TotalLikes property. Hence, the blogpost and the total likes can be accessed in the view
			var blogDetailsViewModel = new BlogDetailsViewModel();

			// ==============================================================================
			// BLOGPOST EXISTENCE CHECK
			// ==============================================================================

			// Check if the blog post exists
			if (blogPost != null)
			{

				// ==============================================================================
				// USER AUTHENTICATION CHECK, LIKED, AND TOTAL LIKES STATUS
				// ==============================================================================

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

				// ==============================================================================
				// BLOG COMMENTS RETRIEVAL AND TRANSFORMATION
				// ==============================================================================

				// Retrieve blog comments from the repository based on the given blog post Id
				var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogId(blogPost.Id);

				// Order the blog comments by published date in descending order
				blogCommentsDomainModel = blogCommentsDomainModel.OrderByDescending(comment => comment.PublishedDate);

				// Initialize a new list to hold the transformed blog comments for the view
				var blogCommentsForView = new List<Models.ViewModels.BlogComment>();

				// Iterate through each blog comment from the domain model and transform it for the view
				foreach (var blogComment in blogCommentsDomainModel)
				{
					// Create a new BlogComment object for the view with designated properties
					var blogCommentForView = new Models.ViewModels.BlogComment
					{
						Description = blogComment.Description, // Set the description of the blog comment
						PublishedDate = blogComment.PublishedDate, // Set the date the comment was added
						Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName // Retrieve and set the username associated with the comment's userId
					};

					// Add the transformed blog comment to the list for view
					blogCommentsForView.Add(blogCommentForView);
				}

				// ==============================================================================
				// VIEW MODEL MAPPING
				// ==============================================================================

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
					Comments = blogCommentsForView
				};
			}

			// Return the view with the BlogDetailsViewModel
			return View(blogDetailsViewModel);
		}

		// Action method to handle POST requests to submit a comment on a blog post
		[HttpPost]
		public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
		{
			// Check if the user is signed in
			if (signInManager.IsSignedIn(User))
			{
				// Create a new BlogComment domain model and set its properties from the view model
				var domainModel = new Models.Domain.BlogComment
				{
					BlogPostId = blogDetailsViewModel.Id,
					Description = blogDetailsViewModel.CommentDescription,
					UserId = userManager.GetUserId(User),
					PublishedDate = DateTime.Now
				};
				// Add the comment to the repository
				await blogPostCommentRepository.AddAsync(domainModel);
				// Redirect to the updated blog post details page
				return RedirectToAction("Index", new { urlHandle = blogDetailsViewModel.UrlHandle });
			}
			// Return a 403 Forbidden response if the user is not signed in
			return Forbid();
		}
	}
}
