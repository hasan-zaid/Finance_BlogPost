using System.ComponentModel.DataAnnotations;

namespace Finance_BlogPost.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
        [MinLength(8, ErrorMessage = "Password has to be at least 8 characters")]
        public string Password { get; set; }
	}
}
