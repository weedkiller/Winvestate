using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models
{
    public class OfferHistoryDto : OfferHistory
    {
        public Guid asset_uuid { get; set; }
        public string customer_full_name { get; set; }
        public string customer_phone { get; set; }
    }
}
