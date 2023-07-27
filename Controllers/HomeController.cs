using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using RunGroopWebApp.Helper;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;

namespace RunGroopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepostiroy clubRepostiroy;

        public HomeController(ILogger<HomeController> logger,IClubRepostiroy clubRepostiroy)
        {
            _logger = logger;
            this.clubRepostiroy = clubRepostiroy;
        }

        public async Task<IActionResult> Index()
        {
            var ipINfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
           
                string url = "https://ipinfo.io/116.106.6.206?token=067db117317455 ";
                var info = new WebClient().DownloadString(url);
                ipINfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipINfo.Country);
                ipINfo.Country = myRI1.EnglishName;
                homeViewModel.City = ipINfo.City;
                homeViewModel.State = ipINfo.Region;
                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await clubRepostiroy.GetClubByCity(homeViewModel.City);
                }
                else
                {
                    homeViewModel.Clubs = null;
                }
                return View(homeViewModel);
            }
         
  
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}