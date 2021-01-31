using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models
{
    public class LoginDto
    {
        public string phone { get; set; }
        public string password { get; set; }
        public Guid row_guid{ get; set; }
        public int user_type { get; set; }
    }
}
