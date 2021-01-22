using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
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
        }
    }
}
