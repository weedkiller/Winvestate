using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_asset")]
    public class Asset : Entity
    {
        [Key]
        public long? asset_no { get; set; }
        public string asset_name { get; set; }
        public int? city_id { get; set; }
        public string city{ get; set; }
        public int? district_id { get; set; }
        public string district{ get; set; }
        public int? category_type_system_type_id { get; set; }
        public int? asset_type_system_type_id { get; set; }
        public decimal? size { get; set; }
        public string address { get; set; }
        public string block_number { get; set; }
        public string plot_number { get; set; }
        public string share { get; set; }
        public string explanation { get; set; }
        public decimal? starting_amount { get; set; }
        public string thumb_path { get; set; }
        public decimal? max_offer_amount { get; set; }
        public bool? is_compatible_for_credit { get; set; }
        public decimal? minimum_increate_amout { get; set; }
        public decimal? guarantee_amount { get; set; }
        public DateTime? last_announcement_date { get; set; }
        public DateTime? first_announcement_date { get; set; }
        public DateTime? last_offer_date { get; set; }
        public Guid? agreement_guid { get; set; }
        public Guid? bank_guid { get; set; }
    }
}
