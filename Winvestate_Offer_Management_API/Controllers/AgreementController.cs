using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Enums.Offer;
using Winvestate_Offer_Management_Models.Mespact;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgreementController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("Signed")]
        public void Insert([FromBody] DocumentToSign pDocumentToSign)
        {
            var loOffer = GetData.GetOfferByAgreementId(pDocumentToSign.erp_id);
            if (loOffer == null) return;

            var loAsset = GetData.GetAssetById(loOffer.asset_uuid.ToString());
            loOffer.offer_state_type_system_type_id =
                loAsset.guarantee_amount.HasValue && loAsset.guarantee_amount.Value > 0 ? (int)OfferStateTypes.WaitingGuarantee : (int)OfferStateTypes.WaitingOffer;

            loOffer.row_update_date = DateTime.Now;
            loOffer.row_update_user = Guid.Empty;
            //loOffer.mespact_session_uuid = pDocumentToSign.row_guid;

            Crud<Offer>.Update(loOffer, out _);

            if (loAsset.guarantee_amount.HasValue)
            {
                var loCustomer = GetData.GetCustomerById(loOffer.owner_uuid.ToString());
                var loMessageContent =
                    string.Format(
                        "Değerli müşterimiz, E-Şartname'yi imzaladığınız için teşekkür ederiz. E-Teklif verebilmek için teminat bedelinizi ({0} TL) GM No: {2} açıklamasıyla  Turk Hava Kurumu Genel Başkanlığı Vakıfbank İban: TR800001500158007312568057 numaralı hesaba yatırınız. Mesaj Tarihi:{1}", loAsset.guarantee_amount.Value.ToString("C0"), DateTime.Now, loAsset.company_prefix + loAsset.asset_no.ToString());

                RestCalls.SendSms(loMessageContent, loCustomer.phone);
            }
        }
    }
}
