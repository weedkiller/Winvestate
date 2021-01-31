using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class OfferController : Controller
    {
        public IActionResult List()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }
    }
}
