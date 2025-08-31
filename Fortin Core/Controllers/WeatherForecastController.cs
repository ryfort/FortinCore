using Fortin_Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGitHubClient _gitHubClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserRepository userRepository, IHttpClientFactory httpClientFactory, IGitHubClient gitHubClient)
        {
            _logger = logger;
            _userRepository = userRepository;
            _client = httpClientFactory.CreateClient();
            _httpClientFactory = httpClientFactory;
            _gitHubClient = gitHubClient;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById(int id) 
        {
            var user = _userRepository.GetById(id);

            return Ok(user);
        }

        [HttpGet]
        [Route("GetGitHubFollowers")]
        public async Task<IActionResult> GetGitHubFollowers()
        {
            _client.BaseAddress = new Uri("https://api.github.com/");
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept,"application/vnd.github.v3+json");
            _client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpClientFactory");
            var httpResponseMessage = await _client.GetAsync("users/danpdc/followers");
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            var followers = JsonConvert.DeserializeObject<List<FollowersDto>>(response);

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
            var user = JsonConvert.DeserializeObject<GithubUserDto>(response);

            return Ok(user);
        }
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