using System.Diagnostics;
using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Models;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
  // HomeController is responsible for displaying the blog posts in the home page
  // It interacts with the IBlogPostRepository to access blog post data, which in turn uses the DbContext to communicate with the database
  // It uses the ILogger<HomeController> to log messages and errors

  public class HomeController : Controller
  {
    // Logger to log messages and errors
    private readonly ILogger<HomeController> _logger;
    // Repository for accessing blog posts
    private readonly IBlogPostRepository blogPostRepository;
    // Repository for accessing tags
    private readonly ITagRepository tagRepository;

    // Constructor to initialize the logger and repository
    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
    {
      _logger = logger;
      this.blogPostRepository = blogPostRepository;
      this.tagRepository = tagRepository;
    }

    // Action method for the Index page
    public async Task<IActionResult> Index()
    {
      // Retrieve all blog posts asynchronously
      var blogPosts = await blogPostRepository.GetAllAsync();

      // Retrieve all tags asynchronously
      var tags = await tagRepository.GetAllAsync();

      // Add the blog posts and tags to the view model
      var model = new HomeViewModel
      {
        BlogPosts = blogPosts,
        Tags = tags
      };

      // Pass the blog posts to the view
      return View(model);
    }

    // Action method for the Privacy page
    public IActionResult Privacy()
    {
      return View();
    }

    // Action method for the Error page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      // Create and return an ErrorViewModel with the current request ID
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
