using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_offer_history")]
    public class OfferHistory
    {
        [Key]
        public int id { get; set; }
        public Guid offer_uuid { get; set; }
        public DateTime row_create_date { get; set; }
        public decimal amount { get; set; }
    }
}
