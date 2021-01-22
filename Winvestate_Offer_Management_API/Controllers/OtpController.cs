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

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OtpController : ControllerBase
    {
        //[HttpPost("Validate")]
        [HttpPost("Send")]
        public ActionResult<GenericResponseModel> SendOtp([FromBody] Otp pOtpService)
        {
            var loGenericResponse = new GenericResponseModel();
            //var loParticipant = GetData.GetParticipantWithId(pOtpService.participant_phone);
            var loErrorMessage = "";

            if (string.IsNullOrEmpty(pOtpService.phone) ||
                string.IsNullOrWhiteSpace(pOtpService.phone))
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Lütfen telefon numaranızı doldurunuz";
                return loGenericResponse;
            }

            pOtpService.phone = HelperMethods.SerializePhone(pOtpService.phone);
            //if (loParticipant == null)
            //{
            //    loGenericResponse.Status = "Fail";
            //    loGenericResponse.Code = -1;
            //    loGenericResponse.Message = "No such participant";
            //    return loGenericResponse;
            //}
            //if (string.IsNullOrEmpty(loParticipant.phone) ||
            //    string.IsNullOrWhiteSpace(loParticipant.phone))
            //{
            //    loGenericResponse.Status = "Fail";
            //    loGenericResponse.Code = -1;
            //    loGenericResponse.Message = "Participant mobile phone can not be empty";
            //    return loGenericResponse;
            //}

            var loValidate = GetData.CheckUserAndWorkorderHaveUnvalidatedOtp(pOtpService);

            if (loValidate?.id > 0 && loValidate.row_create_date != null && (DateTime.Now - (DateTime)loValidate.row_create_date).TotalSeconds < 180)
            {
                var loRemainingTime = 180 - (DateTime.Now - (DateTime)loValidate.row_create_date).Seconds;
                Crud<Otp>.Update(pOtpService, out _);
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = 0;
                loGenericResponse.Data = loRemainingTime;
                loGenericResponse.Message = "Aktif bekleyen bir şifreniz mevcut, şifrenizi giriniz ya da " + " " + loRemainingTime.ToString() + " saniye sonra tekrar deneyiniz. ";
                return loGenericResponse;
            }

            
            pOtpService.row_create_date = DateTime.Now;
            pOtpService.validation_state = 0;

            var loOtpContent = "123456";

            //#if !PROD


            //            pOtpService.otp_hash = Helper.Md5OfString(loOtpContent);
            //            pOtpService.sms_id = "123456";

            //            var loId = Crud<Otp>.Insert(pOtpService, out _);
            //            pOtpService.id = (int)loId;
            //            loGenericResponse.Data = pOtpService;
            //            loGenericResponse.Status = "Ok";
            //            loGenericResponse.Code = 200;
            //            return loGenericResponse;
            //#endif

            loOtpContent = HelperMethods.RandomOtp();
            pOtpService.otp_hash = HelperMethods.Md5OfString(loOtpContent).ToUpper();

            var loMessageContent = HelperMethods.GetOtpContent(pOtpService.message_type_system_type_id, loOtpContent);
            var loMessageResult = RestCalls.SendSms(loMessageContent, pOtpService.phone);

            if (loMessageResult > 0)

            {
                pOtpService.sms_id = loMessageResult.ToString();
                var loMyId = Crud<Otp>.Insert(pOtpService, out _);
                pOtpService.id = (int)loMyId;
                loGenericResponse.Data = pOtpService;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message ="İşleminiz esnasında bir problem oluştu lütfen tekrar deneyiniz." + loErrorMessage;
            }

            return loGenericResponse;

        }

        [HttpPost("Validate")]
        public ActionResult<GenericResponseModel> ValidateOtp([FromBody] Otp pOtpService)
        {
            var loGenericResponse = new GenericResponseModel();
            pOtpService.phone = HelperMethods.SerializePhone(pOtpService.phone);
            var loValidate = GetData.ValidateOtp(pOtpService);


            if (loValidate?.id > 0)
            {
                //var loApiUserId = Helper.GetApiUserIdFromToken(HttpContext.User.Identity);
                pOtpService.row_update_date = DateTime.Now;
                pOtpService.row_create_date = loValidate.row_create_date;
                pOtpService.id = loValidate.id;
                pOtpService.validation_state = 3;
                pOtpService.sms_id = loValidate.sms_id;
                pOtpService.message_type_system_type_id = loValidate.message_type_system_type_id;


                if (loValidate.row_create_date != null && (DateTime.Now - (DateTime)loValidate.row_create_date).TotalSeconds > 180)
                {
                    Crud<Otp>.Update(pOtpService, out _);
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Gönderilen şifrenin süresi dolmuş. Lütfen yeni şifre isteyiniz.";
                    return loGenericResponse;
                }

                pOtpService.validation_state = 1;

                var loErrorMessage = "";
                if (Crud<Otp>.Update(pOtpService, out loErrorMessage))
                {
                    loGenericResponse.Data = pOtpService;
                    loGenericResponse.Status = "Ok";
                    loGenericResponse.Code = 200;
                }
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İşleminiz esnasında bir problem oluştu lütfen tekrar deneyiniz.";
                }
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geçersiz şifre girdiniz. Lütfen tekrar deneyiniz.";
            }

            return loGenericResponse;

        }
    }
}
