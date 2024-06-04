using System.ComponentModel.DataAnnotations;

namespace Finance_BlogPost.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

	
	}
}
