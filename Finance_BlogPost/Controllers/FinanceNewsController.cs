using Finance_BlogPost.Models.ViewModels;
using Finance_BlogPost.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BlogPost.Controllers
{
	public class FinanceNewsController : Controller
	{
		private readonly FinanceNewsService _newsService;

		public FinanceNewsController(FinanceNewsService newsService)
		{
			this._newsService = newsService;
		}

		public async Task<IActionResult> Index()
		{
			var news = await _newsService.GetTopHeadlinesAsync();
			var viewModel = new FinanceNewsViewModel
			{
				Articles = news.Results
			};
			return View(viewModel);
		}
	}
}
