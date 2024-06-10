namespace Finance_BlogPost.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBlogPosts { get; set; }
        public int TotalTags { get; set; }
        public int TotalUsers { get; set; }
        public int PendingApprovals { get; set; }
        public int RejectedPosts { get; set; }
        public List<BlogPostSummary> RecentBlogPosts { get; set; }
        public List<TagSummary> TagUsage { get; set; }
    }

    public class BlogPostSummary
    {
        public string Heading { get; set; }
        public DateTime PublishedDate { get; set; }
        public string AuthorName { get; set; }
    }

    public class TagSummary
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }
    }

}
