using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class BankController : Controller
    {
        [HttpPost]
        [SessionTimeout]
        [OnlyWinvestate]
        public Bank Save([FromBody] BankDto pBank)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            var loBankToSave = pBank.id <= 0 ? RestCalls.SaveNewBank(pBank, loUser.token) : RestCalls.UpdateBank(pBank, loUser.token);
            if (loBankToSave.id > 0)
            {
                loUser.banks = RestCalls.GetAllBanks(loUser.token);
                HttpContext.Session.SetObject("User", loUser);
            }

            return loBankToSave;
        }

        [OnlyWinvestate]
        public IActionResult List()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }
    }
}
