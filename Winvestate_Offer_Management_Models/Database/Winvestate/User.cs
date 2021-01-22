using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_user")]
    public class User : Entity
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string password { get; set; }

    }
}
