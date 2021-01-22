using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Winvestate_Offer_Management_MVC.Classes;

namespace Winvestate_Offer_Management_MVC.Session
{
    public static class SessionExtensions
    {
        private static string _password = "Yv04pwC3WhEw8brqcGEk2Q==";
        public static void SetObject(this ISession session, string key, object value)
        {
            var loPlainKey = JsonConvert.SerializeObject(value);
            session.SetString(key, Cipher.Encrypt(loPlainKey, _password));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            value = Cipher.Decrypt(value, _password);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
