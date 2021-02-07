using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            var loPassword = Request.Cookies["password"] ?? "";
            var loUserName = Request.Cookies["phone"] ?? "";

            if (!string.IsNullOrEmpty(loPassword))
            {
                loPassword = Cipher.DecryptString(loPassword);
            }

            if (!string.IsNullOrEmpty(loUserName))
            {
                loUserName = Cipher.DecryptString(loUserName);
            }

            var loUser = new UserDto
            {
                phone = loUserName,
                password = loPassword
            };

            return View(new HomeViewModel { User = loUser, Token = RestCalls.GetToken() });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public User Validate(UserDto pUser)
        {
            if (pUser.remember_me)
            {
                var loRememberedPassword = Request.Cookies["password"];

                if (string.IsNullOrEmpty(loRememberedPassword) || pUser.password != Cipher.DecryptString(loRememberedPassword))
                {
                    pUser.password = pUser.password.ToUpper();
                }
                else
                {
                    pUser.password = Cipher.DecryptString(loRememberedPassword);
                }
            }
            else
            {
                Response.Cookies.Delete("password");
            }

            var loUser = RestCalls.ValidateUser(pUser);
            if (loUser.id <= 0) return loUser;

            loUser.password = pUser.password;
            loUser.session_id = Guid.NewGuid().ToString();

            if (loUser.user_type != 3)
            {
                loUser.banks = RestCalls.GetAllBanks(loUser.token);
                loUser.contracts = RestCalls.GetContractTypes(loUser.token);
            }

            HttpContext.Session.SetObject("User", loUser);

            if (pUser.remember_me)
            {
                var cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1)
                };

                Response.Cookies.Append("phone", Cipher.EncryptString(pUser.phone), cookie);
                Response.Cookies.Append("password", Cipher.EncryptString(pUser.password), cookie);
            }
            else
            {
                Response.Cookies.Delete("phone");
                Response.Cookies.Delete("password");
            }

            return loUser;
        }

        [HttpPost]
        [SessionTimeout]
        [OnlyWinvestate]
        public UserDto Save([FromBody] UserDto pUser)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            var loUserToSave = pUser.id <= 0 ? RestCalls.SaveNewUser(pUser, loUser.token) : RestCalls.UpdateUser(pUser, loUser.token);
            return loUserToSave;
        }

        [OnlyWinvestate]
        public IActionResult List()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }
    }
}
