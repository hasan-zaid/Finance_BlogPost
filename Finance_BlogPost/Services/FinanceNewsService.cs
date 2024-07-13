
using Newtonsoft.Json;

namespace Finance_BlogPost.Services
{
	public class FinanceNewsService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public FinanceNewsService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_apiKey = configuration["Newsdata:ApiKey"];
		}

		public async Task<NewsApiResponse> GetTopHeadlinesAsync()
		{
			var requestUri = $" https://newsdata.io/api/1/news?apikey={_apiKey}&q=finance&language=en&category=business";
			var response = await _httpClient.GetAsync(requestUri);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new Exception($"Failed to fetch top headlines: {errorContent}");
			}

			var content = await response.Content.ReadAsStringAsync();
			var newsResponse = JsonConvert.DeserializeObject<NewsApiResponse>(content);
			return newsResponse;
		}
	}

	public class NewsApiResponse
	{
		public string Status { get; set; }
		public int TotalResults { get; set; }
		public List<Article> Results { get; set; }
		public string NextPage { get; set; }
	}

	public class Article
	{
		[JsonProperty("article_id")]
		public string ArticleId { get; set; }
		public string Title { get; set; }
		public string Link { get; set; }
		public List<string> Keywords { get; set; }
		public List<string> Creator { get; set; }
		[JsonProperty("video_url")]
		public string VideoUrl { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
		[JsonProperty("pubDate")]
		public string PublishedAt { get; set; }
		[JsonProperty("image_url")]
		public string ImageUrl { get; set; }
		[JsonProperty("source_id")]
		public string SourceId { get; set; }
		[JsonProperty("source_priority")]
		public int SourcePriority { get; set; }
		[JsonProperty("source_url")]
		public string SourceUrl { get; set; }
		[JsonProperty("source_icon")]
		public string SourceIcon { get; set; }
		public string Language { get; set; }
		public List<string> Country { get; set; }
		public List<string> Category { get; set; }
	}
}
