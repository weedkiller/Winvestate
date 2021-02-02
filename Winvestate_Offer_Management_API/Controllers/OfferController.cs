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
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Enums.Offer;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
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

        [HttpGet("Active")]
        public ActionResult<GenericResponseModel> GetActiveOffers()
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loUserType = HelperMethods.GetUserTypeFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = new List<OfferDto>();

            if (loUserType == 1 || loUserType == 2)
            {
                loResult = GetData.GetAllActiveOffers();
                foreach (var offerDto in loResult)
                {
                    offerDto.history = GetData.GetOfferHistoryByAssetId(offerDto.asset_uuid.ToString());
                }
            }
            else if (loUserType == 3)
            {
                loResult = GetData.GetActiveOfferByCustomerId(loUserId.ToString());
                foreach (var offerDto in loResult)
                {
                    offerDto.history = GetData.GetOfferHistoryByOfferId(offerDto.row_guid.ToString());
                }
            }
            else if (loUserType == 4)
            {
                loResult = GetData.GetActiveOffersByBankId(loUserId.ToString());
                foreach (var offerDto in loResult)
                {
                    offerDto.history = GetData.GetOfferHistoryByAssetId(offerDto.asset_uuid.ToString());
                }
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpPost("ConfirmSubmit")]
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
            loResult.row_update_date = DateTime.Now;
            loResult.row_update_user = loUserId;

            //Crud<Offer>.Update(loResult, out _);

            var loPassword = "";
            var loMessageContent = "";
            var loUserNameType = loCustomer.user_type_system_type_id == 1 ? "kimlik numaranız" : "vergi numaranız";
            if (string.IsNullOrEmpty(loCustomer.password))
            {
                loPassword = HelperMethods.RandomOtp();
                loMessageContent =
                    string.Format(
                        "Sayın müşterimiz teklif vermek istediğiniz gayrimenkule ait başvurunuz onaylanmıştır. Teklif vermek için {0} adresini ziyaret edebilirsiniz. Sisteme giriş için kullanıcı adınız: {3}, şifreniz: {1}'dir. Mesaj tarihi: {2}", Common.CustomerUrl, loPassword, DateTime.Now, loUserNameType);

                loCustomer.password = HelperMethods.Md5OfString(loPassword);
                loCustomer.row_update_date = loResult.row_update_date;
                loCustomer.row_update_user = loUserId;
                Crud<Customer>.Update(loCustomer, out _);
            }
            else
            {
                loMessageContent =
                    string.Format(
                        "Sayın müşterimiz teklif vermek istediğiniz gayrimenkule ait başvurunuz onaylanmıştır. Teklif vermek için {0} adresini ziyaret edebilirsiniz. Sisteme giriş için kullanıcı adınız olarak {2} ve daha önce oluşturduğunuz şifreyi kullanabilirsiniz. Mesaj tarihi: {1}", Common.CustomerUrl, DateTime.Now, loUserNameType);

            }

            Crud<Offer>.Update(loResult, out _);

            RestCalls.SendSms(loMessageContent, loCustomer.phone);

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpPost("New")]
        public ActionResult<GenericResponseModel> NewOffer([FromBody] OfferHistory pOfferHistory)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loOffer = GetData.GetOfferById(pOfferHistory.offer_uuid.ToString());

            if (loOffer == null)
            {
                loGenericResponse.Message = "Kayıtlı teklif bulunamadı!";
                return loGenericResponse;
            }

            if (loOffer.price >= pOfferHistory.amount)
            {
                loGenericResponse.Message = "Lütfen güncel tekflinizden daha yüksek bir teklif veriniz.";
                return loGenericResponse;
            }

            var loAsset = GetData.GetAssetById(loOffer.asset_uuid.ToString());

            if (!loAsset.max_offer_amount.HasValue || pOfferHistory.amount >= loAsset.max_offer_amount + loAsset.minimum_increate_amout.Value)
            {
                loOffer.price = pOfferHistory.amount;
                loAsset.max_offer_amount = pOfferHistory.amount;
                pOfferHistory.row_create_date = DateTime.Now;
                loAsset.row_update_date = pOfferHistory.row_create_date;
                loOffer.row_update_date = pOfferHistory.row_create_date;


                var loId = Crud<OfferHistory>.InsertNewOffer(pOfferHistory, loOffer, loAsset);

                if (loId > 0)
                {
                    Task.Run(() => HelperMethods.SendNewOfferInformation(pOfferHistory.offer_uuid));
                    loGenericResponse.Code = 200;
                    loGenericResponse.Status = "OK";
                }
                else
                {
                    loGenericResponse.Message = "Teklif kaydı esnasında bir problem oluştu. Lütfen tekrar deneyiniz.";
                }

                return loGenericResponse;
            }

            loGenericResponse.Code = -2;
            loGenericResponse.Message = "Sizden önce teklif yükseltilmiş. Lütfen teklifinizi gözden geçiriniz.";
            return loGenericResponse;
        }

        [HttpGet("List/{pId}")]
        public ActionResult<GenericResponseModel> GetAssetOfferList(string pId)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loUserType = HelperMethods.GetUserTypeFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetOfferHistoryByAssetId(pId).OrderByDescending(x => x.row_create_date);

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
