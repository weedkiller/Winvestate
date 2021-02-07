using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models
{
    public class OfferDto : Offer
    {
        public string customer_name { get; set; }
        public DateTime last_offer_date{ get; set; }
        public string customer_surname { get; set; }
        public string company_name { get; set; }
        public string customer_full_name { get; set; }
        public string customer_mail { get; set; }
        public string customer_phone { get; set; }
        public string asset_name { get; set; }
        public string asset_no { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public decimal minimum_increate_amout { get; set; }
        public decimal max_offer_amount { get; set; }
        public string max_offer { get; set; }
        public decimal guarantee_amount { get; set; }
        public decimal starting_amount { get; set; }
        public string offer_state { get; set; }
        public string bank_name { get; set; }
        public string company_prefix { get; set; }
        public int offer_state_id { get; set; }
        public int asset_state_id { get; set; }
        public DateTime last_operation_date { get; set; }
        public DateTime asset_update_date { get; set; }
        public Guid bank_guid { get; set; }
        public List<OfferHistoryDto> history { get; set; }
    }
}
