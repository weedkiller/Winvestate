using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models
{
    public class ApiUser
    {
        [Key]
        public int id { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public string api_key { get; set; }

        public string device_id { get; set; }

        public string language { get; set; }

        public int tenant_id { get; set; }
    }
}
