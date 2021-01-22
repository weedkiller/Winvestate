using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_bank")]
    public class Bank:Entity
    {
        public string name { get; set; }
        public Guid? mespact_agreement_uuid { get; set; }
    }
}
