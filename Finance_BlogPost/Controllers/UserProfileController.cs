using Finance_BlogPost.Models;
using Finance_BlogPost.Models.Domain;
using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Finance_BlogPost.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileImageRepository userProfileImageRepository;
        private readonly UserManager<IdentityUser> userManager;

        public UserProfileController(IUserProfileImageRepository userProfileImageRepository, UserManager<IdentityUser> userManager)
        {
            this.userProfileImageRepository = userProfileImageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            // Get the current signed in user
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var userProfileImage = await userProfileImageRepository.GetByUserAsync(user.Id);

                var userProfile = new UserViewProfile
                {
                    Username = user.UserName,
                    Email = user.Email,
                    ProfileImageUrl = userProfileImage?.ProfileImageUrl // Set to null if userProfileImage is null
                };

                return View(userProfile);
            }

            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Details(UserViewProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                // Get the current signed in user
                var user = await userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Update user details
                    user.UserName = userProfile.Username;
                    user.Email = userProfile.Email;
                    var updateResult = await userManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        var userProfileImage = await userProfileImageRepository.GetByUserAsync(user.Id);
                        // Update user profile image if it is changed
                        if(userProfileImage != null)
                        {
                            if (userProfile.ProfileImageUrl != null)
                            {

                                userProfileImage.ProfileImageUrl = userProfile.ProfileImageUrl;
                                await userProfileImageRepository.UpdateAsync(userProfileImage);
                            }
                            else if (string.IsNullOrEmpty(userProfile.ProfileImageUrl))
                            {
                                // Remove profile image if URL is null or empty
                                await userProfileImageRepository.DeleteAsync(userProfileImage.Id);
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(userProfile.ProfileImageUrl))
                            {
                                // Create new UserProfileImage
                                userProfileImage = new UserProfileImage
                                {
                                    UserID = user.Id,
                                    ProfileImageUrl = userProfile.ProfileImageUrl
                                };
                                await userProfileImageRepository.AddAsync(userProfileImage);
                            }
                        }
   

                        return RedirectToAction("Details");
                    }
                    else
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(userProfile);
        }


    }
}
