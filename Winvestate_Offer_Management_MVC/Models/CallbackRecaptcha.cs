using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Classes;

namespace Winvestate_Offer_Management_MVC.Models
{
    public class CallbackRecaptcha : GoogleReCaptchaModelBase
    {
        public string applicant_name { get; set; }
        public string applicant_surname { get; set; }
        public string applicant_phone { get; set; }
        public Guid? asset_uuid { get; set; }
        public AssetDto asset { get; set; }
    }
}
