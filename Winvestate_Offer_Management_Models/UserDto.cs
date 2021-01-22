using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Mespact;

namespace Winvestate_Offer_Management_Models
{
    public class UserDto : User
    {
        public bool remember_me { get; set; }
        public string session_id { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public AllTypes sys_definitions { get; set; }
        public List<DocumentType> contracts { get; set; }
        public List<Bank> banks { get; set; }
    }
}
