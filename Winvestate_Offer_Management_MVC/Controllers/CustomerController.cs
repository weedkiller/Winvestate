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
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class CustomerController : Controller
    {

        [HttpPost]
        public Customer Save([FromBody] CustomerDto pBank)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            var loCustomer = RestCalls.SaveNewCustomer(pBank);
            return loCustomer;
        }

        [HttpPost]
        public int Check([FromBody] CallbackRecaptcha pCallback)
        {
            //return 1;
            if (ModelState.IsValid)
            {
                return 1;
            }
            return -1;
        }

        public CallbackRecordDto Callback([FromBody] CallbackRecaptcha pCallback)
        {
            if (ModelState.IsValid)
            {
                var loResult = RestCalls.SaveNewCallback(pCallback);
                return loResult;
            }

            var loCallbackRecord = new CallbackRecordDto
            {
                id = -2
            };

            return loCallbackRecord;
        }
    }
}
