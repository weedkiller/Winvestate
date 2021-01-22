using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winvestate_Offer_Management_MVC.Api.Model
{
    public class SmsModel
    {
        public string Message { get; set; }

        public string Phones { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Provider { get; set; }

        public string Header { get; set; }

    }
}
