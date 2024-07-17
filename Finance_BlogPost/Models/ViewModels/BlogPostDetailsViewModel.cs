using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Finance_BlogPost.Models.ViewModels
{
    public class BlogPostDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string BlogImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public IdentityUser Author { get; set; }
        public bool Visible { get; set; }

        public string Approval { get; set; }

        public string Reason { get; set; }


        // Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }
        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
