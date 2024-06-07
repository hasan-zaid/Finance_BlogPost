using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Finance_BlogPost.Models.ViewModels
{
    public class UserViewProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [ValidateNever]
        public string ProfileImageUrl { get; set; }
    }
}
