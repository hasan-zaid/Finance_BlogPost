using Finance_BlogPost.Models.Domain;
using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
	// This attribute defines the base route for all actions in this controller.
	[Route("api/[controller]")]
	// This attribute specifies that this controller will handle API requests.
	[ApiController]
	public class BlogPostLikeController : ControllerBase
	{
		// Dependency injection of the IBlogPostLikeRepository to interact with the data layer.
		private readonly IBlogPostLikeRepository blogPostLikeRepository;

		// Constructor that initializes the repository through dependency injection.
		public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
		{
			this.blogPostLikeRepository = blogPostLikeRepository;
		}

		// This action method handles HTTP POST requests to the "api/BlogPostLike/Add" route.
		// The [FromBody] attribute specifies that the request body should be deserialized into an AddLikeRequest object.
		// The AddLikeForBlog method of the IBlogPostLikeRepository is called to add a like to the blog post in the database.
		[HttpPost]
		[Route("Add")]
		public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
		{
			// Create a new BlogPostLike object using the data from the request body.
			var model = new BlogLike
			{
				BlogPostId = addLikeRequest.BlogPostId,
				UserId = addLikeRequest.UserId.ToString()
			};

			// Call the repository method to add a like to the blog post in the database.
			await blogPostLikeRepository.AddLikeForBlog(model);

			// Return an HTTP 200 OK response to indicate success.
			return Ok();
		}

		// This action method handles HTTP GET requests to the "api/BlogPostLike/GetTotalLikes" route.
		// The [FromRoute] attribute specifies that the route parameter "blogPostId" should be deserialized into a Guid object.
		// The GetTotalLikes method of the IBlogPostLikeRepository is called to retrieve the total number of likes for the blog post with the specified ID.
		[HttpGet]
		[Route("{blogPostId:Guid}/totalLikes")]
		public async Task<IActionResult> GetTotalLikes([FromRoute] Guid blogPostId)
		{
			var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);

			return Ok(totalLikes);
		}

		// This action method handles HTTP POST requests to the "api/BlogPostLike/Remove" route.
		// The [FromBody] attribute specifies that the request body should be deserialized into an AddLikeRequest object.
		// The RemoveLikeForBlog method of the IBlogPostLikeRepository is called to remove a like from the blog post in the database.
		[HttpPost]
		[Route("Remove")]
		public async Task<IActionResult> RemoveLike([FromBody] AddLikeRequest addLikeRequest)
		{
			// Create a new BlogLike object using the data from the request body.
			var model = new BlogLike
			{
				BlogPostId = addLikeRequest.BlogPostId,
				UserId = addLikeRequest.UserId.ToString()
			};

			// Call the repository method to remove a like from the blog post in the database.
			await blogPostLikeRepository.RemoveLikeForBlog(model);

			// Return an HTTP 200 OK response to indicate success.
			return Ok();
		}
	}
}
