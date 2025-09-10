using Fortin.Infrastructure.Interface;
using Fortin.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GitHubClient _gitHubClient;

        public GitHubController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, GitHubClient gitHubClient)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _httpClientFactory = httpClientFactory;
            _gitHubClient = gitHubClient;
        }

        [HttpGet]
        [Route("GetGitHubFollowers")]
        public async Task<IActionResult> GetGitHubFollowers()
        {
            //simple httpclient
            //_client.BaseAddress = new Uri("https://api.github.com/"); 
            //_client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            //_client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
            //var httpResponseMessage = await _client.GetAsync("users/danpdc/followers");
            //var response = await httpResponseMessage.Content.ReadAsStringAsync();
            //var followers = JsonConvert.DeserializeObject<List<FollowersDto>>(response);

            var followers = await _gitHubClient.GetFollowers();

            return Ok(followers);
        }

        [HttpGet]
        [Route("GetGitHubFollowersCount")]
        public async Task<IActionResult> GetGitHubFollowersCount()
        {
            var followersCount = await _gitHubClient.GetFollowersCount(); //typed httpclient

            return Ok(followersCount);
        }

        [HttpGet]
        [Route("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername()
        {
            var namedHttpClient = _httpClientFactory.CreateClient("GitHub");//named httpclient

            var httpResponseMessage = await namedHttpClient.GetAsync("users/ryfort");
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<GithubUserDto>();
            //var user = JsonConvert.DeserializeObject<GithubUserDto>(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("GetUserRepository")]
        public async Task<IActionResult> GetUserRepository()
        {
            _client.BaseAddress = new Uri("https://api.github.com/");
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            _client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
            var httpResponseMessage = await _client.GetAsync("users/ryfort/repos");
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<List<GithubUserRepo>>(response);

            return Ok(user);
        }
    }

    public class GithubUserRepo
    {
        public long Id { get; set; }
        public string Node_Id { get; set; }
        public string Name { get; set; }
        public string Html_Url { get; set; }
        public string Description { get; set; }
        public GithubUserDto Owner { get; set; }
    }

    public class FollowersDto
    {
        public string Login { get; set; }
        public int Id { get; set; }
        public string Avatar_Url { get; set; }
    }

    public class GithubUserDto
    {
        public string Login { get; set; }
        public long Id { get; set; }
        public string Avatar_Url { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }

    }
}
