using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_customer")]
    public class Customer : Entity
    {
        public int user_type_system_type_id { get; set; }
        public string company_name { get; set; }
        public string iban { get; set; }
        public string tax_office { get; set; }
        public string tax_no { get; set; }
        public string customer_name { get; set; }
        public string customer_surname { get; set; }
        public string identity_no { get; set; }
        public DateTime? birth_date { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string password { get; set; }
    }
}
