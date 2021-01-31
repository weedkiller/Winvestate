using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    public class AddressController : ControllerBase
    {
        [HttpGet("City")]
        public ActionResult<GenericResponseModel> GetCities([FromQuery] bool pIsLand)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Message = "Kayıt Bulunamadı"
            };
            var loCities = GetData.GetCities();

            if (!loCities.Any()) return loGenericResponse;

            loGenericResponse.Code = 200;
            loGenericResponse.Message = "Ok";
            loGenericResponse.Data = loCities;

            return loGenericResponse;
        }

        [HttpGet("Distrcit")]
        public ActionResult<GenericResponseModel> GetDistrcits([FromQuery] int pId, [FromQuery] bool pIsLand)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Message = "Kayıt Bulunamadı"
            };
            var loCities = GetData.GetDistricts(pId.ToString());

            if (!loCities.Any()) return loGenericResponse;

            loGenericResponse.Code = 200;
            loGenericResponse.Message = "Ok";
            loGenericResponse.Data = loCities;

            return loGenericResponse;
        }

    }
}
