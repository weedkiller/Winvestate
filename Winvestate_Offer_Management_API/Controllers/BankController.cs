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

            if (!string.IsNullOrEmpty(pObject.company_prefix) && pObject.company_prefix.Length > 5)
            {
                loGenericResponse.Message = "Kurum kısaltması 5 karakteri geçemez.";
                return loGenericResponse;
            }

            loObj.is_enable_pre_offer = pObject.is_enable_pre_offer;
            loObj.bank_name = pObject.bank_name ?? loObj.bank_name;
            loObj.company_prefix = pObject.company_prefix ?? loObj.company_prefix;
            loObj.authorized_mail = pObject.authorized_mail ?? loObj.authorized_mail;
            loObj.authorized_password = pObject.authorized_password ?? loObj.authorized_password;
            loObj.authorized_name = pObject.authorized_name ?? loObj.authorized_name;
            loObj.authorized_surname = pObject.authorized_surname ?? loObj.authorized_surname;
            loObj.authorized_phone = pObject.authorized_phone ?? loObj.authorized_phone;
            loObj.authorized_second_phone = pObject.authorized_second_phone ?? loObj.authorized_second_phone;
            loObj.authorized_dial_code = pObject.authorized_dial_code ?? loObj.authorized_dial_code;
            loObj.bank_name = pObject.bank_name ?? loObj.bank_name;
            loObj.agreement_link = pObject.agreement_link ?? loObj.agreement_link;
            loObj.mespact_agreement_uuid = pObject.mespact_agreement_uuid ?? loObj.mespact_agreement_uuid;
            loObj.sale_in_company = pObject.sale_in_company ?? loObj.sale_in_company;
            loObj.is_enable_pre_offer = pObject.is_enable_pre_offer ?? loObj.is_enable_pre_offer;
            loObj.is_active = pObject.is_active ?? loObj.is_active;
            loObj.is_deleted = pObject.is_deleted ?? loObj.is_deleted;
            loObj.bank_name = loObj.bank_name.ToUpper();
            loObj.authorized_name = loObj.authorized_name.ToUpper();
            loObj.authorized_surname = loObj.authorized_surname.ToUpper();
            loObj.authorized_mail = loObj.authorized_mail.ToLower();
            loObj.agreement_link = loObj.agreement_link.ToLower();
            loObj.authorized_phone = HelperMethods.SerializePhone(loObj.authorized_phone);
            loObj.authorized_second_phone = HelperMethods.SerializePhone(loObj.authorized_second_phone);
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

            if (string.IsNullOrEmpty(pObject.bank_name))
            {
                loGenericResponse.Message = "Kurum adı girilmeden işleme devam edilemez";
                return loGenericResponse;
            }

            if (!string.IsNullOrEmpty(pObject.company_prefix) && pObject.company_prefix.Length > 5)
            {
                loGenericResponse.Message = "Kurum kısaltması 5 karakteri geçemez.";
                return loGenericResponse;
            }

            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_deleted = false;
            pObject.is_active = true;
            pObject.bank_name = string.IsNullOrEmpty(pObject.bank_name) ? "" : pObject.bank_name.ToUpper();
            pObject.company_prefix = string.IsNullOrEmpty(pObject.company_prefix) ? "" : pObject.company_prefix.ToUpper();
            pObject.authorized_name = string.IsNullOrEmpty(pObject.authorized_name) ? "" :  pObject.authorized_name.ToUpper();
            pObject.authorized_surname = string.IsNullOrEmpty(pObject.authorized_surname) ? "" : pObject.authorized_surname.ToUpper();
            pObject.authorized_mail = string.IsNullOrEmpty(pObject.authorized_mail) ? "" : pObject.authorized_mail.ToLower();
            pObject.authorized_password = string.IsNullOrEmpty(pObject.authorized_password) ? "" : pObject.authorized_password.ToUpper();
            pObject.authorized_phone = string.IsNullOrEmpty(pObject.authorized_phone) ? "" : HelperMethods.SerializePhone(pObject.authorized_phone);
            pObject.authorized_second_phone = string.IsNullOrEmpty(pObject.authorized_second_phone) ? "" : HelperMethods.SerializePhone(pObject.authorized_second_phone);
            pObject.agreement_link = string.IsNullOrEmpty(pObject.agreement_link) ? "" : pObject.agreement_link.ToLower();
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
