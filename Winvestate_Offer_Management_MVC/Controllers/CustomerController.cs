using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class CustomerController : Controller
    {

        [HttpPost]
        public Customer Save([FromBody] CustomerDto pBank)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            var loCustomer= RestCalls.SaveNewCustomer(pBank);
            return loCustomer;
        }
    }
}
 