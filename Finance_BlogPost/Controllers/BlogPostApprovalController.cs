using Finance_BlogPost.Models.Domain;
using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Finance_BlogPost.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finance_BlogPost.Controllers
{
    public class BlogPostApprovalController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRejectionRepository blogPostRejectionRepository;


        public BlogPostApprovalController(IBlogPostRepository blogPostRepository, ITagRepository tagRepository, IBlogPostRejectionRepository blogPostRejectionRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            this.blogPostRejectionRepository = blogPostRejectionRepository;
        }


        [HttpGet]   
        public async Task<IActionResult> List(
                   string? searchQuery,
                   string? sortBy,
                   string? sortDirection,
                   int pageSize = 3,
                   int pageNumber = 1)
        {
            var totalRecords = await blogPostRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }


            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            var tags = await blogPostRepository.GetPendingApprovalAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // Retrieve the result from the repository
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                // map the domain model into the view model
                var model = new BlogPostDetails
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    BlogImageUrl = blogPost.BlogImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Approval = blogPost.Approval,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);

            }

            // pass data to view
            return View(null);

        }

        [HttpPost]
        public async Task<IActionResult> Approve(BlogPostDetails postDetails)
        {
            var blogPost = await blogPostRepository.GetAsync(postDetails.Id);
            if (blogPost != null)
            {
                blogPost.Approval = BlogPostApproval.Approved;
                var updatedBlog = await blogPostRepository.UpdateAsync(blogPost);
                if (updatedBlog != null)
                {
                    // Show success notification
                    TempData["success"] = "The blog post has been successfully approved";
                    return RedirectToAction("List");
                }

            }
            TempData["error"] = "Failed to approve the blog post";
            return RedirectToAction("Details", new { id = postDetails.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Reject(BlogPostDetails postDetails)
        {
            var blogPost = await blogPostRepository.GetAsync(postDetails.Id);
            if (blogPost != null)
            {
                // Create a new BlogPostRejection entry
                var blogPostRejection = new BlogPostRejection
                {
                    Id = Guid.NewGuid(),
                    Reason = postDetails.Reason, 
                    BlogPostID = blogPost.Id,
                    BlogPost = blogPost
                };

                // Add the rejection entry to the BlogPostRejection table
                var rejectionAdded = await blogPostRejectionRepository.AddAsync(blogPostRejection);

                if (rejectionAdded != null)
                {
                    // Update the blog post approval status
                    blogPost.Approval = BlogPostApproval.Rejected;
                    var updatedBlog = await blogPostRepository.UpdateAsync(blogPost);

                    if (updatedBlog != null)
                    {
                        // Show success notification
                        TempData["success"] = "The blog post has been successfully rejected";
                        return RedirectToAction("List");
                    }
                }
            }

            TempData["error"] = "Failed to reject the blog post";
            return RedirectToAction("Details", new { id = postDetails.Id });
        }

    }
}
