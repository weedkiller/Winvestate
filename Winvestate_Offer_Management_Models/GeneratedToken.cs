using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models
{
    public class GeneratedToken
    {
        public int id { get; set; }

        public int api_user_id { get; set; }

        public string token { get; set; }

        public DateTime generation_time { get; set; }

        public DateTime expire_time { get; set; }
    }
}
