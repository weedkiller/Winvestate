using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Npgsql.Logging;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Enums.Offer;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        public ActionResult<GenericResponseModel> Insert([FromBody] CustomerDto pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (pObject.birth_date == null)
            {
                if (!DateTime.TryParse(pObject.birthdate, out var loBirthdate))
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Geçersiz doğum tarihi";
                    return loGenericResponse;
                }

                pObject.birth_date = loBirthdate;
            }

            var loIdentity = new Identity(pObject.identity, pObject.customer_name.ToUpper(), pObject.customer_surname.ToUpper(), pObject.birth_date.Value.Year);
            if (!loIdentity.CheckIdentity())
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Kimlik bilgileriniz doğrulanamadı.";
                return loGenericResponse;
            }

            var loCheckUserHasRegistered = GetData.GetCustomerByIdentity(pObject.identity.ToString());
            if (loCheckUserHasRegistered != null && loCheckUserHasRegistered.user_type_system_type_id == pObject.user_type_system_type_id)
            {
                loCheckUserHasRegistered.send_agreement = pObject.send_agreement;
                loCheckUserHasRegistered.asset_uuid = pObject.asset_uuid;
                pObject = loCheckUserHasRegistered;

                var loOfferToCheck =
                    GetData.GetOfferByOwnerAndAssetId(pObject.row_guid.ToString(), pObject.asset_uuid.ToString());

                if (loOfferToCheck != null)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Verdiğiniz bilgilerle daha önce kayıt oluşturulmuştur. Lütfen tarafınıza iletilen kullanıcı adı ve şifreniz ile sisteme giriş yaparak teklifinizi veriniz.";
                    return loGenericResponse;
                }
            }
            else
            {
                pObject.row_create_date = DateTime.Now;
                pObject.row_guid = Guid.NewGuid();
                pObject.row_create_user = pObject.row_guid;
                pObject.is_deleted = false;
                pObject.is_active = true;
                pObject.identity_no = pObject.identity.ToString().Trim();
                pObject.address = pObject.address?.ToUpper().Trim();
                pObject.customer_name = pObject.customer_name?.ToUpper().Trim();
                pObject.customer_surname = pObject.customer_surname?.ToUpper().Trim();
                pObject.company_name = pObject.company_name?.ToUpper().Trim();
                pObject.iban = pObject.iban?.ToUpper().Trim();
                pObject.tax_office = pObject.tax_office?.ToUpper().Trim();
                pObject.tax_no = pObject.tax_no?.ToUpper().Trim();
                pObject.phone = HelperMethods.SerializePhone(pObject.phone?.ToUpper());
                pObject.mail = pObject.mail?.ToLower();
            }

            Offer loOffer = null;
            if (pObject.send_agreement)
            {
                if (pObject.asset_uuid == null)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Gayrimenkul seçilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }


                loOffer = new Offer
                {
                    asset_uuid = pObject.asset_uuid.Value,
                    owner_uuid = pObject.row_guid,
                    offer_state_type_system_type_id = (int)OfferStateTypes.WaitingAgreement,
                    row_create_date = pObject.row_create_date,
                    row_create_user = pObject.row_create_user,
                    row_guid = Guid.NewGuid(),
                    agreement_uuid = Guid.NewGuid(),
                    pre_offer_price = pObject.pre_offer_price,
                    is_active = true,
                    is_deleted = false
                };
            }
            var loResult = Crud<Customer>.InsertCustomerWithOffer(pObject, loOffer);

            if (loResult > 0)
            {
                Task.Run(() => HelperMethods.SendToDocumentToSign(pObject, loOffer));
                pObject.id = (int)loResult;
                loGenericResponse.Data = pObject;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Kayıt başarısız";
            }

            return loGenericResponse;
        }

        [HttpPost("Check")]
        public ActionResult<GenericResponseModel> Check([FromBody] CustomerDto pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (pObject.birth_date == null)
            {
                if (!DateTime.TryParse(pObject.birthdate, out var loBirthdate))
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Geçersiz doğum tarihi";
                    return loGenericResponse;
                }

                pObject.birth_date = loBirthdate;
            }

            var loIdentity = new Identity(pObject.identity, pObject.customer_name.ToUpper(), pObject.customer_surname.ToUpper(), pObject.birth_date.Value.Year);
            if (!loIdentity.CheckIdentity())
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Kimlik bilgileriniz doğrulanamadı.";
                return loGenericResponse;
            }

            var loCustomer = GetData.GetCustomerByIdentity(pObject.identity.ToString());
            loGenericResponse.Code = 1;

            if (loCustomer != null)
            {
                loGenericResponse.Code =
                    GetData.GetOfferByOwnerAndAssetId(loCustomer.row_guid.ToString(), pObject.asset_uuid.ToString()) == null ? 1 : 0;//Bu adam bu iş için teklif verdiyse panele yönlendirelim.
            }




            return loGenericResponse;
        }

        [HttpPost("Callback")]
        public ActionResult<GenericResponseModel> InsertCallbackRecord([FromBody] CallbackRecord pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (string.IsNullOrEmpty(pObject.applicant_name) || string.IsNullOrEmpty(pObject.applicant_surname))
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Talebe ait ad soyad girilmesi zorunludur.";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pObject.applicant_phone))
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geri aranacak telefon numarası girilmesi zorunludur.";
                return loGenericResponse;
            }

            if (pObject.asset_uuid == null)
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Talebe ait gayrimenkul seçilmesi zorunludur.";
                return loGenericResponse;
            }

            pObject.applicant_phone = HelperMethods.SerializePhone(pObject.applicant_phone);
            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_active = true;
            pObject.is_deleted = false;
            pObject.applicant_name = pObject.applicant_name.ToUpper();
            pObject.applicant_surname = pObject.applicant_surname.ToUpper();
            pObject.callback_record_state_type_system_type_id = 43;

            var loResult = Crud<CallbackRecord>.Insert(pObject, out _);

            if (loResult < 0)
            {
                loGenericResponse.Message = "Kayıt yapılamadı, tekrar deneyiniz!";
                return loGenericResponse;
            }

            Task.Run(() => HelperMethods.SendNewCallBackRecord(pObject));
            loGenericResponse.Code = 200;
            loGenericResponse.Message = "";
            loGenericResponse.Data = pObject;
            loGenericResponse.Status = "Ok";

            return loGenericResponse;
        }

        [HttpPut("ChangeCallback")]
        public ActionResult<GenericResponseModel> ChangeCallbackRecord([FromBody] CallbackRecord pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            var loCallbackRecord = GetData.GetCallbackRecordById(pObject.row_guid.ToString());

            if (loCallbackRecord == null)
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geri aranma talebi bulunamadı.";
                return loGenericResponse;
            }

            loCallbackRecord.row_update_date = DateTime.Now;
            loCallbackRecord.row_update_user = loUserId;
            loCallbackRecord.callback_record_state_type_system_type_id =
                pObject.callback_record_state_type_system_type_id;
            loCallbackRecord.note = pObject.note;
            var loResult = Crud<CallbackRecord>.Update(loCallbackRecord, out _);

            if (!loResult)
            {
                loGenericResponse.Message = "Kayıt yapılamadı, tekrar deneyiniz!";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Message = "";
            loGenericResponse.Data = pObject;
            loGenericResponse.Status = "Ok";

            return loGenericResponse;
        }

        [HttpGet("ActiveCallback")]
        public ActionResult<GenericResponseModel> GetActiveCallbacks()
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetNewCallbackRecords();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı geri aranma talebi bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("Callback")]
        public ActionResult<GenericResponseModel> GetAllCallbacks()
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetAllCallbacks();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı geri aranma talebi bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }
    }
}
