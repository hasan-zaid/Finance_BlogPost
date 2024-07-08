using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
	[Authorize]
	public class NotificationController : Controller
	{
		private readonly INotificationRepository notificationRepository;
		private readonly UserManager<IdentityUser> userManager;

		public NotificationController(INotificationRepository notificationRepository, UserManager<IdentityUser> userManager)
		{
			this.notificationRepository = notificationRepository;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);
			var notifications = await notificationRepository.GetUserAsync(user.Id);
			var unreadCount = await notificationRepository.CountUnreadAsync(user.Id);

			var viewModel = new NotificationViewModel
			{
				Notifications = notifications.ToList(),
				UnreadCount = unreadCount
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> MarkAsRead(Guid id)
		{
			var user = await userManager.GetUserAsync(User);
			var notification = await notificationRepository.GetAsync(id);
			if (notification != null && notification.UserId == user.Id)
			{
				notification.IsRead = true;
				notificationRepository.Update(notification);
			}
			return RedirectToAction("Index");
		}
	}
}
