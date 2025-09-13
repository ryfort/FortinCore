using Fortin.Common.Dtos;
using Fortin.Infrastructure.Interface;
using Fortin.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("github/users")]
    public class GitHubController : ControllerBase
    {
        private readonly ILogger<GitHubController> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GitHubClient _gitHubClient;

        public GitHubController(ILogger<GitHubController> logger, IHttpClientFactory httpClientFactory, GitHubClient gitHubClient)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _httpClientFactory = httpClientFactory;
            _gitHubClient = gitHubClient;
        }

        [HttpGet("{username}/followers")]
        public async Task<IActionResult> GetGitHubFollowers(string username)
        {
            //simple httpclient
            //_client.BaseAddress = new Uri("https://api.github.com/"); 
            //_client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            //_client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
            //var httpResponseMessage = await _client.GetAsync("users/danpdc/followers");
            //var response = await httpResponseMessage.Content.ReadAsStringAsync();
            //var followers = JsonConvert.DeserializeObject<List<FollowersDto>>(response);

            var followers = await _gitHubClient.GetFollowers(username);

            return Ok(followers);
        }

        [HttpGet]
        [Route("GetGitHubFollowersCount")]
        public async Task<IActionResult> GetGitHubFollowersCount(string username)
        {
            var followersCount = await _gitHubClient.GetFollowersCount(username);

            return Ok(followersCount);
        }

        //[HttpGet("{username}", Name = "GetUserByUsername")]
        //public async Task<IActionResult> GetUserByUsername()
        //{
        //    var namedHttpClient = _httpClientFactory.CreateClient("GitHub");//named httpclient

        //    var httpResponseMessage = await namedHttpClient.GetAsync("users/ryfort");
        //    var response = await httpResponseMessage.Content.ReadFromJsonAsync<GithubUserDto>();
        //    //var user = JsonConvert.DeserializeObject<GithubUserDto>(response);

        //    return Ok(response);
        //}

        [HttpGet("{username}", Name = "GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _gitHubClient.GetUserByUsername(username);

            return Ok(user);
        }

        //[HttpGet("{username}/repos", Name = "GetUserRepository")]
        //public async Task<IActionResult> GetUserRepository()
        //{
        //    _client.BaseAddress = new Uri("https://api.github.com/");
        //    _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
        //    _client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
        //    var httpResponseMessage = await _client.GetAsync("users/ryfort/repos");
        //    var response = await httpResponseMessage.Content.ReadAsStringAsync();
        //    var user = JsonConvert.DeserializeObject<List<GithubUserRepo>>(response);

        //    return Ok(user);
        //}

        [HttpGet("{username}/repos", Name = "GetUserRepository")]
        public async Task<IActionResult> GetUserRepository(string username)
        {
            var repos = await _gitHubClient.GithubUserRepos(username);

            return Ok(repos);
        }
    }
}
