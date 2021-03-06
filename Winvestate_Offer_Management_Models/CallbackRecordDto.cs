﻿using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models
{
    public class CallbackRecordDto:CallbackRecord
    {
        public string message { get; set; }
        public string asset_name { get; set; }
        public string asset_no { get; set; }
        public string company_prefix { get; set; }
        public string callback_record_state { get; set; }
        public string applicant_full_name { get; set; }
    }
}
