using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_bank")]
    public class Bank:Entity
    {
        public string bank_name { get; set; }
        public string company_prefix { get; set; }
        public string authorized_name { get; set; }
        public string authorized_surname { get; set; }
        public string authorized_phone { get; set; }
        public string authorized_mail { get; set; }
        public string authorized_password { get; set; }
        public string authorized_second_phone { get; set; }
        public string authorized_dial_code { get; set; }
        public Guid? mespact_agreement_uuid { get; set; }
        public bool? sale_in_company { get; set; }
    }
}
