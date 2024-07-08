using Finance_BlogPost.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finance_BlogPost.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{

			//add custom password validation
			ValidatePassword(registerViewModel);

			if (ModelState.IsValid)
			{
				var identityUser = new IdentityUser
				{
					UserName = registerViewModel.Username,
					Email = registerViewModel.Email
				};

				var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

				if (identityResult.Succeeded)
				{
					// assign this user the "User" role
					var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

					if (roleIdentityResult.Succeeded)
					{
						// Show success notification
						TempData["success"] = "Successful Registration";
						return RedirectToAction("Login");
					}
				}
			}

			// Show error notification
			TempData["error"] = "Unsuccessfuly Registration";
			return View();
		}




		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
				loginViewModel.Password, false, false);

			if (signInResult != null && signInResult.Succeeded)
			{
                // Show success notification
                TempData["success"] = "Successful Login";
                var user = await userManager.GetUserAsync(User);
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "AdminDashboard");
                }
      
                else if (roles.Contains("Author"))
                {
                    return RedirectToAction("List", "AuthorBlogPosts");
                }
                else 
                {
                    return RedirectToAction("Index", "Home");
                }
                
			}

            // Show errors
            TempData["error"] = "Unsuccessfuly Login";
            return View();
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

        private void ValidatePassword(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password is not null)
            {
                if (registerViewModel.Password.Length < 8)
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long.");
                }
                if (!registerViewModel.Password.Any(char.IsDigit))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one digit.");
                }
                if (!registerViewModel.Password.Any(char.IsLower))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one lowercase letter.");
                }
                if (!registerViewModel.Password.Any(char.IsUpper))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one uppercase letter.");
                }
                if (!registerViewModel.Password.Any(c => !char.IsLetterOrDigit(c)))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one non-alphanumeric character.");
                }
                if (registerViewModel.Password.Distinct().Count() < registerViewModel.Password.Length - 1)
                {
                    ModelState.AddModelError("Password", "Password must contain at least one non-alphanumeric character.");
                }
            }
        }


        [HttpPost]
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("GoogleLoginConfirmation", new GoogleLoginConfirmationViewModel { Email = email });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLoginConfirmation(GoogleLoginConfirmationViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }


    }
}
