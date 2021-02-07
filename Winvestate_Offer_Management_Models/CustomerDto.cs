using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models.Database
{
    public class CustomerDto : Customer
    {
        
        public string message { get; set; }
        public string birthdate { get; set; }
        public long identity { get; set; }
        public bool send_agreement{ get; set; }
        public Guid? asset_uuid{ get; set; }
        public decimal? pre_offer_price { get; set; }
    }
}
