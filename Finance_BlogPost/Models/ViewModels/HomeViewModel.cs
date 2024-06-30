//using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Models.ViewModels
{
  public class HomeViewModel
  {
    public IEnumerable<Domain.BlogPost>? BlogPosts { get; set; }

    public IEnumerable<Domain.Tag>? Tags { get; set; }
  }
}