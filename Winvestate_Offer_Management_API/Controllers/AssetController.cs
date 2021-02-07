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

            if (pObject.asset_photos == null || !pObject.asset_photos.Any())
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Gayrimenkul fotoğraflarında bir problem oldu!. Tekrar yükleme yapın!";
                return loGenericResponse;
            }

            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_deleted = false;
            pObject.is_active = true;
            pObject.city = pObject.city.ToUpper();
            pObject.district = pObject.district?.ToUpper();
            pObject.address = pObject.address?.ToUpper();
            pObject.share = pObject.share?.ToUpper();
            pObject.asset_name = pObject.asset_name?.ToUpper();
            pObject.asset_no = pObject.asset_no?.ToUpper();

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

            //if (pObject.first_announcement_date == null)
            //{
            //    if (DateTime.TryParse(pObject.first_announcement_date_str, out var loFirstAnnonce))
            //        pObject.first_announcement_date = loFirstAnnonce;
            //    else
            //    {
            //        loGenericResponse.Status = "Fail";
            //        loGenericResponse.Code = -1;
            //        loGenericResponse.Message = "İlan yayın başlangıç tarihi girilmeden işleme devam edilemez.";
            //        return loGenericResponse;
            //    }

            //    if ((loData.row_create_date.Value - pObject.first_announcement_date.Value).TotalMinutes > 2)
            //    {
            //        loGenericResponse.Status = "Fail";
            //        loGenericResponse.Code = -1;
            //        loGenericResponse.Message = "Geçmiş tarihili ilan başlatılamaz.";
            //        return loGenericResponse;
            //    }
            //}

            if (pObject.last_announcement_date == null)
            {
                if (DateTime.TryParse(pObject.last_announcement_date_str, out var loLastAnnonce))
                {
                    loData.last_announcement_date = loLastAnnonce;
                    if (loData.last_announcement_date < loData.first_announcement_date)
                    {
                        loGenericResponse.Status = "Fail";
                        loGenericResponse.Code = -1;
                        loGenericResponse.Message = "İlanın bitiş tarihi başlangıç tarihinden önce olamaz";
                        return loGenericResponse;
                    }
                }
            }

            if (pObject.last_offer_date == null)
            {
                if (DateTime.TryParse(pObject.last_offer_date_str, out var loOffer))
                {
                    loData.last_offer_date = loOffer;
                    if (loData.last_offer_date < loData.last_announcement_date)
                    {
                        loGenericResponse.Status = "Fail";
                        loGenericResponse.Code = -1;
                        loGenericResponse.Message = "Son teklif tarihi, ilan bitiş tarihinden önce olamaz";
                        return loGenericResponse;
                    }
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
            loData.agreement_guid = pObject.agreement_guid ?? loData.agreement_guid;
            loData.is_sold = pObject.is_sold ?? loData.is_sold;
            loData.free_text_no = pObject.free_text_no ?? loData.free_text_no;
            loData.show_last_offer_date = pObject.show_last_offer_date ?? loData.show_last_offer_date;
            //ilan başlangıç tarihi değiştirilemez loData.first_announcement_date = pObject.first_announcement_date ?? loData.first_announcement_date;
            loData.asset_name = pObject.asset_name ?? loData.asset_name;
            loData.bank_guid = pObject.bank_guid ?? loData.bank_guid;
            loData.row_update_date = DateTime.Now;
            loData.row_update_user = loUserId;
            loData.city = loData.city.ToUpper();
            loData.district = loData.district?.ToUpper();
            loData.address = loData.address?.ToUpper();
            loData.share = loData.share?.ToUpper();
            loData.asset_name = loData.asset_name?.ToUpper();
            loData.asset_no = loData.asset_no?.ToUpper();

            if (pObject.asset_photos != null && pObject.asset_photos.Any())
            {
                loData.asset_photos = pObject.asset_photos;
            }


            var loResult = Crud<Asset>.UpdateAsset(loData);

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

        [HttpGet("Offered")]
        public ActionResult<GenericResponseModel> GetOfferedAssets()
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loUserType = HelperMethods.GetUserTypeFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetOfferedAssets();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı gayrimenkul bulunamadı";
                return loGenericResponse;
            }


            loResult = loUserType switch
            {
                4 => loResult.FindAll(x => x.bank_guid == loUserId),
                _ => loResult
            };

            foreach (var assetDto in loResult)
            {
                assetDto.history = GetData.GetOfferHistoryByAssetId(assetDto.row_guid.ToString());
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("List")]
        public ActionResult<GenericResponseModel> GetAllList()
        {
            var loUserType = HelperMethods.GetUserTypeFromToken(HttpContext.User.Identity);
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = loUserType switch
            {
                1 => GetData.GetAllAssets(),
                2 => GetData.GetAllAssetsForUser(loUserId.ToString()),
                _ => GetData.GetAllAssetsForCompany(loUserId.ToString())
            };

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

        [HttpGet("Sold")]
        public ActionResult<GenericResponseModel> GetAllSoldAssets()
        {
            var loUserType = HelperMethods.GetUserTypeFromToken(HttpContext.User.Identity);
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetOfferSummary();

            loResult = loUserType switch
            {
                3 => loResult.FindAll(x => x.owner_uuid == loUserId),
                4 => loResult.FindAll(x => x.bank_guid == loUserId),
                _ => loResult
            };

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı gayrimenkul bulunamadı";
                return loGenericResponse;
            }

            loResult = loResult.FindAll(x => x.asset_state_id == 5);

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
