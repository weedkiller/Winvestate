using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : ControllerBase
    {
        [HttpPut]
        public ActionResult<GenericResponseModel> Update([FromBody] Bank pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loObj = GetData.GetBankById(pObject.row_guid.ToString());

            if (loObj == null)
            {
                loGenericResponse.Message = "Kurum bulunamadı!";
                return loGenericResponse;
            }

            loObj.name = pObject.name ?? loObj.name;
            loObj.mespact_agreement_uuid = pObject.mespact_agreement_uuid ?? loObj.mespact_agreement_uuid;
            loObj.is_active = pObject.is_active ?? loObj.is_active;
            loObj.is_deleted = pObject.is_deleted ?? loObj.is_deleted;
            loObj.name = loObj.name.ToUpper();
            loObj.row_update_date = DateTime.Now;
            loObj.row_update_user = loUserId;

            if (!Crud<Bank>.Update(loObj, out _)) return loGenericResponse;

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loObj;

            return loGenericResponse;
        }

        [HttpPost]
        public ActionResult<GenericResponseModel> Insert([FromBody] Bank pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (string.IsNullOrEmpty(pObject.name))
            {
                loGenericResponse.Message = "Kurum adı girilmeden işleme devam edilemez";
                return loGenericResponse;
            }
            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_deleted = false;
            pObject.is_active = true;
            pObject.name = pObject.name.ToUpper();
            pObject.name = pObject.name.ToUpper();
            var loResult = Crud<Bank>.Insert(pObject, out _);


            if (loResult > 0)
            {
                pObject.id = (int)loResult;
                loGenericResponse.Data = pObject;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geçersiz Kullanıcı adı veya Şifre";
            }

            return loGenericResponse;
        }

        [HttpGet]
        public ActionResult<GenericResponseModel> GetAll()
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetAllBanks();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı kurum bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("{pId}")]
        public ActionResult<GenericResponseModel> GetById(string pId)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetBankById(pId);

            if (loResult == null)
            {
                loGenericResponse.Message = "Kayıtlı kurum bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }
    }
}
