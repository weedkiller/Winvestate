﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Winvestate_Offer_Management_Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
