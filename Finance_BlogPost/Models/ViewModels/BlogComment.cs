﻿namespace Finance_BlogPost.Models.ViewModels
{
	public class BlogComment
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public DateTime PublishedDate { get; set; }
		public string Username { get; set; }
		public Guid UserId { get; set; }
	}
}
