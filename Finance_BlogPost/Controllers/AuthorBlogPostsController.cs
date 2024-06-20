using Amazon.Runtime.Internal;
using Finance_BlogPost.Models.Domain;
using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Finance_BlogPost.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;

namespace Finance_BlogPost.Controllers
{
    public class AuthorBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository, UserManager<IdentityUser> userManager)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {

            // get tags from repository
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {

            //Get the current signed in user
            var user = await _userManager.GetUserAsync(User);


            // Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                BlogImageUrl = addBlogPostRequest.BlogImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = user,
                Visible = addBlogPostRequest.Visible,
                Approval = BlogPostApproval.Pending
            };

            // Map Tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;


            await blogPostRepository.AddAsync(blogPost);
            TempData["success"] = "Blog Post has been added successfully";

            return RedirectToAction("List");
        }


        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(
                    string? searchQuery,
                    string? sortBy,
                    string? sortDirection,
                    int pageSize = 5,
                    int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            var totalRecords = await blogPostRepository.CountAuthorPostsAsync(user.Id);
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            if (pageNumber < 1)
            {
                pageNumber++;
            }

            var blogs = await blogPostRepository.GetAllAuthorPostsAsync(user.Id, searchQuery, sortBy, sortDirection, pageNumber, pageSize);


            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            
            return View(blogs);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve the result from the repository 
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                // map the domain model into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    BlogImageUrl = blogPost.BlogImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Approval = blogPost.Approval,
                    Author = blogPost.Author,
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
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                BlogImageUrl = editBlogPostRequest.BlogImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,
                Approval = editBlogPostRequest.Approval

            };

            // Map tags into domain model

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                // Show success notification
                TempData["success"] = "Blog Post has been updated successfully";
                return RedirectToAction("List");
            }

            // Show error notification
            TempData["error"] = "Failed to update the blog post.";
            return RedirectToAction("Edit");
        }




        #region API Calls

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

            if (deletedBlogPost != null)
            {
                // Show success notification
                TempData["success"] = "Blog Post has been deleted successfully";
                return Json(new { success = true, message = "Deleted Successfully" });

            }

            // Show an error notification
            TempData["error"] = "Failed to delete the blog post.";

            return Json(new { success = false, message = "Error while deleting" });
        }
        #endregion
    }
}
