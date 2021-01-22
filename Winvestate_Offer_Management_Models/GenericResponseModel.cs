using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models
{
    public class GenericResponseModel
    {
        public string Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
