using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("sys_otp")]
    public class Otp
    {
        [Key]
        public int id { get; set; }
        public string otp_hash { get; set; }
        public string phone { get; set; }
        public DateTime row_create_date { get; set; }
        public DateTime row_update_date { get; set; }
        public int message_type_system_type_id { get; set; }
        public int validation_state { get; set; }
        public string sms_id { get; set; }
    }
}
