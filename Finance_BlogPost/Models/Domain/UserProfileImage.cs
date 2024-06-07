using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_BlogPost.Models.Domain
{
    public class UserProfileImage
    {
        public Guid Id { get; set; }
        public string ProfileImageUrl { get; set; }

        public string UserID { get; set; }

        [ForeignKey("UserID")]
        [ValidateNever]
        public IdentityUser User { get; set; }
    }
}
