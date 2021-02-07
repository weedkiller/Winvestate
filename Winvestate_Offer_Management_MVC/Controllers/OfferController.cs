using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class OfferController : Controller
    {
        [OnlyAdmin]
        public IActionResult List()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }

        [OnlyAdmin]
        public IActionResult Signed()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }

        public IActionResult DownloadDocument(string pId)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            var loDocument = RestCalls.GetSignedDocument(pId, loUser.token);

            if (loDocument != null && !string.IsNullOrEmpty(loDocument.pdf_content))
            {
                return File(Convert.FromBase64String(loDocument.pdf_content), "application/pdf", pId + ".pdf");
            }

            return RedirectToAction("Error", "Home");
        }
    }
}
