using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Winvestate_Offer_Management_API.Classes
{
    public static class Bitly
    {
        private static string _bitlyApiAdress = @"https://api-ssl.bitly.com/shorten?access_token={0}&longUrl={1}";
        private static string _accessToken = "d4bbed60e7a7786929a2f43da3f264dc8e8a6f46";

        public static bool CheckAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken))
                return false;
            string temp = string.Format(_bitlyApiAdress, _accessToken, "google.com");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = client.GetAsync(temp).Result;
                return res.IsSuccessStatusCode;
            }
        }

        public static string Shorten(string long_url)
        {
            if (CheckAccessToken())
            {
                using (HttpClient client = new HttpClient())
                {
                    string temp = string.Format(_bitlyApiAdress, _accessToken, WebUtility.UrlEncode(long_url));
                    var res = client.GetAsync(temp).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        var message = res.Content.ReadAsStringAsync().Result;
                        dynamic obj = JsonConvert.DeserializeObject(message);

                        string loResult = obj.results[long_url].shortUrl.ToString();
                        return loResult.Replace("http:", "https:");
                    }
                    else
                    {
                        return "Can not short URL";
                    }
                }
            }
            else
            {
                return "Can not short URL";
            }
        }
    }
}
