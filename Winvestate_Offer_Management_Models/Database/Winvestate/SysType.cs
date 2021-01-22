using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("sys_type")]
    public class SysType
    {
        [Key] public int id { get; set; }

        public string type_name { get; set; }

        [Write(false)] public string type_value { get; set; }

        public string type_value_tr { get; set; }

        public string type_value_en { get; set; }

        public int list_order { get; set; }
    }
}
