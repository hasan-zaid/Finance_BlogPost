using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Finance_BlogPost.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [ValidateNever]
        public string ProfileImageUrl { get; set; }
    }
}
