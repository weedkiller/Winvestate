using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_callback_record")]
    public class CallbackRecord : Entity
    {
        public string applicant_name { get; set; }
        public string applicant_surname { get; set; }
        public string applicant_phone { get; set; }
        public string note { get; set; }
        public Guid? asset_uuid { get; set; }
        public int callback_record_state_type_system_type_id { get; set; }
        public string company_name { get; set; }
        public string city { get; set; }
        public string district { get; set; }
    }
}
