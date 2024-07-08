using Finance_BlogPost.Models.Domain;

namespace Finance_BlogPost.Models.ViewModels
{
	public class NotificationViewModel
	{
		public List<Notification> Notifications { get; set; }
		public int UnreadCount { get; set; }
	}
}
