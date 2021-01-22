using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Enums.Offer;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfferController : ControllerBase
    {
        [HttpGet("Summary")]
        public ActionResult<GenericResponseModel> GetOfferSummary()
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetOfferSummary();

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpPost("Confirm")]
        public ActionResult<GenericResponseModel> ConfirmSubmit([FromBody] OfferDto pOffer)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetOfferById(pOffer.row_guid.ToString());

            if (loResult == null)
            {
                loGenericResponse.Message = "Kayıtlı teklif bulunamadı!";
                return loGenericResponse;
            }

            var loCustomer = GetData.GetCustomerById(loResult.owner_uuid.ToString());

            if (loCustomer == null)
            {
                loGenericResponse.Message = "Kayıtlı teklif bulunamadı!";
                return loGenericResponse;
            }

            loResult.offer_state_type_system_type_id = (int)OfferStateTypes.WaitingOffer;
            loResult.row_update_date=DateTime.Now;
            loResult.row_update_user = loUserId;

            //Crud<Offer>.Update(loResult, out _);

            var loPassword = HelperMethods.RandomOtp();
            loCustomer.password = HelperMethods.Md5OfString(loPassword);
            loCustomer.row_update_date = loResult.row_update_date;
            loCustomer.row_update_user = loUserId;

            Crud<Offer>.Update(loResult, out _);
            Crud<Customer>.Update(loCustomer, out _);

            var loMessageContent =
                string.Format(
                    "Sayın müşterimiz teklif vermek istediğiniz gayrimenkule ait başvurunuz onaylaşnmıştır. Teklif vermek için {0} adresini ziyaret edebilirsiniz. Sisteme giriş için kullanıcı adınız kimlik numaranız, şifreniz {1}'dir. Mesaj tarihi: {2}","https://winvestate.mesnetbilisim.com.tr/Account/Login",loPassword,DateTime.Now);

            RestCalls.SendSms(loMessageContent, loCustomer.phone);

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpPost("Resend")]
        public ActionResult<GenericResponseModel> ResendAgreementLink([FromBody] OfferDto pOffer)
        {
           return RestCalls.SendAgreementLinkAgain(pOffer.mespact_session_uuid);
        }
    }
}
