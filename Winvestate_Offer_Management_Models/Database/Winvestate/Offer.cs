using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_offer")]
    public class Offer:Entity
    {
        public decimal? price { get; set; }
        public int offer_state_type_system_type_id { get; set; }
        public Guid asset_uuid { get; set; }
        public Guid owner_uuid { get; set; }
        public Guid? agreement_uuid { get; set; }
        public string mespact_session_uuid { get; set; }
        public decimal? pre_offer_price { get; set; }

    }
}
