using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models
{
    [Table("sys_otp_message")]
    public class OtpMessage
    {
        [Key]
        public int id { get; set; }
        public int otp_type_system_type_id { get; set; }
        public string otp_content { get; set; }

    }
}
