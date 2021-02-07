using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [SessionTimeout]
        public IActionResult Dashboard()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            if (loUser.user_type < 3 || loUser.user_type == 4)
            {
                loUser.offered_assets = RestCalls.GetOfferedAssets(loUser.token);
            }

            return View(new HomeViewModel { User = loUser, Offers = RestCalls.GetAllOffers(loUser.token), ActiveCallbackRecords = RestCalls.GetNewCallbackRecords(loUser.token) });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
