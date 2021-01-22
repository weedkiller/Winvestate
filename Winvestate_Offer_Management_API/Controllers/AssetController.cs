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
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetController : ControllerBase
    {
        [HttpPost]
        public ActionResult<GenericResponseModel> Insert([FromBody] AssetDto pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_deleted = false;
            pObject.is_active = true;
            pObject.city = pObject.city.ToUpper();
            pObject.district = pObject.district?.ToUpper();
            pObject.address = pObject.address?.ToUpper();
            pObject.share = pObject.share?.ToUpper();
            pObject.explanation = pObject.explanation?.ToUpper();
            pObject.asset_name = pObject.asset_name?.ToUpper();

            if (pObject.first_announcement_date == null)
            {
                if (DateTime.TryParse(pObject.first_announcement_date_str, out var loFirstAnnonce))
                    pObject.first_announcement_date = loFirstAnnonce;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan yayın başlangıç tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if (DateTime.Now > pObject.first_announcement_date.Value && DateTime.Now.ToShortDateString() != pObject.first_announcement_date.Value.ToShortDateString())
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Geçmiş tarihili ilan başlatılamaz.";
                    return loGenericResponse;
                }
            }

            if (pObject.last_announcement_date == null)
            {
                if (DateTime.TryParse(pObject.last_announcement_date_str, out var loLastAnnonce))
                    pObject.last_announcement_date = loLastAnnonce;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan yayın bitiş tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if (pObject.last_announcement_date < pObject.first_announcement_date)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlanın bitiş tarihi başlangıç tarihinden önce olamaz";
                    return loGenericResponse;
                }
            }

            if (pObject.last_offer_date == null)
            {
                if (DateTime.TryParse(pObject.last_offer_date_str, out var loOffer))
                    pObject.last_offer_date = loOffer;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan son teklif tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if (pObject.last_offer_date < pObject.last_announcement_date)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Son teklif tarihi, ilan bitiş tarihinden önce olamaz";
                    return loGenericResponse;
                }
            }

            var loResult = Crud<AssetDto>.InsertAsset(pObject);

            if (loResult > 0)
            {
                pObject.id = loResult;
                loGenericResponse.Data = pObject;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Gayrimenkul kaydı başarısız";
            }

            return loGenericResponse;
        }

        [HttpPut]
        public ActionResult<GenericResponseModel> Update([FromBody] AssetDto pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            var loData = GetData.GetAssetById(pObject.row_guid.ToString());
            if (loData == null)
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Gayrimenkul bulunamadı!";
                return loGenericResponse;
            }

            if (pObject.first_announcement_date == null)
            {
                if (DateTime.TryParse(pObject.first_announcement_date_str, out var loFirstAnnonce))
                    pObject.first_announcement_date = loFirstAnnonce;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan yayın başlangıç tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if ((loData.row_create_date.Value - pObject.first_announcement_date.Value).TotalMinutes > 2)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Geçmiş tarihili ilan başlatılamaz.";
                    return loGenericResponse;
                }
            }

            if (pObject.last_announcement_date == null)
            {
                if (DateTime.TryParse(pObject.last_announcement_date_str, out var loLastAnnonce))
                    pObject.last_announcement_date = loLastAnnonce;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan yayın bitiş tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if (pObject.last_announcement_date < pObject.first_announcement_date)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlanın bitiş tarihi başlangıç tarihinden önce olamaz";
                    return loGenericResponse;
                }
            }

            if (pObject.last_offer_date == null)
            {
                if (DateTime.TryParse(pObject.last_offer_date_str, out var loOffer))
                    pObject.last_offer_date = loOffer;
                else
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "İlan son teklif tarihi girilmeden işleme devam edilemez.";
                    return loGenericResponse;
                }

                if (pObject.last_offer_date < pObject.last_announcement_date)
                {
                    loGenericResponse.Status = "Fail";
                    loGenericResponse.Code = -1;
                    loGenericResponse.Message = "Son teklif tarihi, ilan bitiş tarihinden önce olamaz";
                    return loGenericResponse;
                }
            }

            loData.is_deleted = pObject.is_deleted ?? loData.is_deleted;
            loData.is_active = pObject.is_active ?? loData.is_active;
            loData.asset_no = pObject.asset_no ?? loData.asset_no;
            loData.city_id = pObject.city_id ?? loData.city_id;
            loData.city = pObject.city ?? loData.city;
            loData.district_id = pObject.district_id ?? loData.district_id;
            loData.district = pObject.district ?? loData.district;
            loData.category_type_system_type_id = pObject.category_type_system_type_id ?? loData.category_type_system_type_id;
            loData.asset_type_system_type_id = pObject.asset_type_system_type_id ?? loData.asset_type_system_type_id;
            loData.size = pObject.size ?? loData.size;
            loData.block_number = pObject.block_number ?? loData.block_number;
            loData.plot_number = pObject.plot_number ?? loData.plot_number;
            loData.share = pObject.share ?? loData.share;
            loData.explanation = pObject.explanation ?? loData.explanation;
            loData.starting_amount = pObject.starting_amount ?? loData.starting_amount;
            loData.max_offer_amount = pObject.max_offer_amount ?? loData.max_offer_amount;
            loData.minimum_increate_amout = pObject.minimum_increate_amout ?? loData.minimum_increate_amout;
            loData.guarantee_amount = pObject.guarantee_amount ?? loData.guarantee_amount;
            loData.is_compatible_for_credit = pObject.is_compatible_for_credit ?? loData.is_compatible_for_credit;
            loData.thumb_path = pObject.thumb_path ?? loData.thumb_path;
            loData.last_announcement_date = pObject.last_announcement_date ?? loData.last_announcement_date;
            loData.last_offer_date = pObject.last_offer_date ?? loData.last_offer_date;
            loData.agreement_guid = pObject.agreement_guid ?? loData.agreement_guid;
            loData.first_announcement_date = pObject.first_announcement_date ?? loData.first_announcement_date;
            loData.asset_name = pObject.asset_name ?? loData.asset_name;
            loData.bank_guid = pObject.bank_guid ?? loData.bank_guid;
            loData.row_update_date = DateTime.Now;
            loData.row_update_user = loUserId;
            loData.city = pObject.city.ToUpper();
            loData.district = pObject.district?.ToUpper();
            loData.address = pObject.address?.ToUpper();
            loData.share = pObject.share?.ToUpper();
            loData.explanation = pObject.explanation?.ToUpper();
            loData.asset_name = pObject.asset_name?.ToUpper();

            var loResult = Crud<Asset>.Update(loData, out _);

            if (loResult)
            {
                loGenericResponse.Data = pObject;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Gayrimenkul kaydı başarısız";
            }

            return loGenericResponse;
        }

        [HttpGet("OfferList")]
        public ActionResult<GenericResponseModel> GetAllForOfferList()
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetAssetForListing();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı gayrimenkul bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("List")]
        public ActionResult<GenericResponseModel> GetAllList()
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetAssetForListing();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı gayrimenkul bulunamadı";
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

            var loResult = GetData.GetAssetById(pId);

            if (loResult == null)
            {
                loGenericResponse.Message = "Kayıtlı gayrimenkul bulunamadı";
                return loGenericResponse;
            }

            loResult.asset_photos = GetData.GetAssetPhotos(loResult.row_guid.ToString());

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }
    }
}
