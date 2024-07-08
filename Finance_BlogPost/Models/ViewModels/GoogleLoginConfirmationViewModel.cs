using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Finance_BlogPost.Models.ViewModels
{
    public class GoogleLoginConfirmationViewModel
    {
        public string Email { get; set; }
        [ValidateNever]
        public string ReturnUrl { get; set; }
    }
}
