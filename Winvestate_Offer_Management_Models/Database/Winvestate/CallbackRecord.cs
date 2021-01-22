using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    public class CallbackRecord : Entity
    {
        public string applicant_name { get; set; }
        public string applicant_surname { get; set; }
        public string applicant_phone { get; set; }
        public Guid? asset_uuid{ get; set; }
    }
}
