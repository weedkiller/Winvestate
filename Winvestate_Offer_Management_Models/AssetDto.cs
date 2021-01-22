using System.Collections.Generic;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models
{
    public class AssetDto : Asset
    {
        public List<AssetPhoto> asset_photos{ get; set; }
        public string message{ get; set; }
        public string category{ get; set; }
        public string asset_type{ get; set; }
        public string last_announcement_date_str{ get; set; }
        public string first_announcement_date_str { get; set; }
        public string last_offer_date_str { get; set; }
        public string state { get; set; }
        public int state_id { get; set; }
        public string bank_name { get; set; }
        public string max_offer { get; set; }
    }
}
