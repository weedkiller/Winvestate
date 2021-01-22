using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Winvestate_Offer_Management_MVC.Classes
{
    public static class Common
    {
        public static string ApiUrl = "http://localhost:5001/api";
        //public static string ApiUrl = "http://fibimi-api.fibimi.com/api";
        public static string AddressApiUrl = "http://fibimi-api.fibimi.com/api";
        public static string UpdateUserUrl = ApiUrl + "/User";
        public static string SendOtpUrl = ApiUrl + "/Otp/Send";
        public static string ValidateOtpUrl = ApiUrl + "/Otp/Validate";
        public static readonly string MespactLinkUrl = "https://panel.esozlesme.com.tr";

        [Conditional("DEBUG")]
        public static void SetVariablesForDebug()
        {
            ApiUrl = "http://localhost:5001/api";
        }

        [Conditional("TEST")]
        public static void SetVariablesForTest()
        {
            ApiUrl = "https://winvestate-api.mesnetbilisim.com.tr/api";
        }


        [Conditional("PROD")]
        public static void SetVariablesForProd()
        {
            ApiUrl = "http://fibimi-api.fibimi.com/api";
        }
    }
}
