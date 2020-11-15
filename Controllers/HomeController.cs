using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weatherApp_v2.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace weatherAPI.Controllers
{
    public class HomeController : Controller
    {
        // DEPENDANCY INJECT appsettings.json configuration -------------------------------------------------------------------------------------
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // API CONNECTION ------------------------------------------------------------------------------------------------------------------------
        public HttpClient apiConnection()
        {
            var APIclient = new HttpClient();
            APIclient.BaseAddress = new Uri("http://api.openweathermap.org");

            return APIclient;
        }
        // (GET) INDEX ---------------------------------------------------------------------------------------------------------------------------
        [HttpGet("{name?}")]
        public async Task<IActionResult> Index(string name)
        {
            var defaultCity = "London";
            var openWeatherApiKey = _configuration.GetValue<string>("openWeatherApiKey");
            var response = await apiConnection().GetAsync($"/data/2.5/weather?q={name ?? defaultCity}&appid={openWeatherApiKey}&units=metric");

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(result);

            return View(weatherData);
        }
        // (POST) INDEX --------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult IndexPost(string name)
        {
            return RedirectToAction("Index", name);
        }



    }
}
