using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Winvestate_Offer_Management_API.Api.Model;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Mespact;

namespace Winvestate_Offer_Management_API.Api
{
    public static class RestCalls
    {
        private static readonly string _addressApiUrl = "https://information.mesnetbilisim.com.tr/api/Address/";
        private static readonly string _mespactApiKey = "1605935e-3c83-494c-a77e-3e5ea8203b3e";
        private static readonly string _mespactApiUrl = "https://mespactapi-prod.mesnetbilisim.com.tr/api/v2";


        public static string GetMespactToken()
        {
            var client = new RestClient(_mespactApiUrl + "/Token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loMyBody = new
            {
                api_key = _mespactApiKey,
                language = "tr",
            };

            var loModel = JsonConvert.SerializeObject(loMyBody);
            request.AddJsonBody(loModel);

            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loToken = JsonConvert.DeserializeObject<GeneratedToken>(result.Content);

            return loToken.token;
        }

        public static GenericResponseModel SendCustomerAgreement(SendToSign pSendToSignModel)
        {
            var client = new RestClient(_mespactApiUrl + "/Document/SendToSign");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + GetMespactToken());
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loModel = JsonConvert.SerializeObject(pSendToSignModel);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);

            return loGenericResult;
        }

        public static GenericResponseModel SendAgreementLinkAgain(string pMespactSessionId)
        {
            var client = new RestClient(_mespactApiUrl + "/Document/SignLink/"+ pMespactSessionId);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + GetMespactToken());
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);

            return loGenericResult;
        }

        public static List<DocumentType> GetMespactDocumentTypes()
        {
            var client = new RestClient(_mespactApiUrl + "/Document/Type/Participant/" + Common.MespactWinvestateUser);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + GetMespactToken());
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loDocuments = new List<DocumentType>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loDocuments = JsonConvert.DeserializeObject<List<DocumentType>>(loGenericResult.Data.ToString());
                return loDocuments;
            }

            return loDocuments;
        }
        public static List<AddressKeyValue> GetCities()
        {
            var loGenericResult = new List<AddressKeyValue>();
            var loApiUrl = _addressApiUrl + "City";
            var client = new RestClient(loApiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);

            var loResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content) ?? new GenericResponseModel();
            loGenericResult = JsonConvert.DeserializeObject<List<AddressKeyValue>>(loResult.Data?.ToString()) ??
                              new List<AddressKeyValue>();


            loGenericResult.RemoveAll(x => x.value == "");
            loGenericResult = loGenericResult.OrderBy(x => x.text).ToList();

            return loGenericResult;
        }

        public static List<AddressKeyValue> GetDistricts(string pId)
        {
            var loGenericResult = new List<AddressKeyValue>();
            var loApiUrl = _addressApiUrl + "Distrcit/" + pId;
            var client = new RestClient(loApiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);

            var loResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content) ?? new GenericResponseModel();
            loGenericResult = JsonConvert.DeserializeObject<List<AddressKeyValue>>(loResult.Data?.ToString()) ??
                              new List<AddressKeyValue>();


            loGenericResult.RemoveAll(x => x.value == "");
            loGenericResult = loGenericResult.OrderBy(x => x.text).ToList();

            return loGenericResult;
        }

        public static long SendSms(string pMessage, string pPhone)
        {
            var loSmsModel = new SmsModel
            {
                Message = pMessage,
                Phones = pPhone,
                Provider = 2, //Tescom
                Username = Common.SmsUsername,
                Password = Common.SmsPassword,
                Header = Common.SmsHeader
            };

            var client = new RestClient(Common.InfoUrl + "Information/SendSms");
            var request = new RestRequest(Method.POST);
            var loToken = GetToken("mesnetapi", "5158898A39CC7B544969DBF80261A0AF", Common.InfoUrl + "GetToken");
            request.AddHeader("Authorization", "Bearer " + loToken);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loModel = JsonConvert.SerializeObject(loSmsModel);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status == "Ok")
            {
                return (long)loGenericResult.Data;
            }

            return 0;
        }

        public static string GetToken(string pUserName, string pPassword, string pUrl, string pBankId = "")
        {
            var loSmsModel = new ApiUserService
            {
                UserName = pUserName,
                Password = pPassword,
                ApiKey = pPassword,
                BankId = pBankId
            };

            var client = new RestClient(pUrl);
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loModel = JsonConvert.SerializeObject(loSmsModel);
            request.AddJsonBody(loModel);
            return JsonConvert.DeserializeObject<GeneratedToken>(client.Execute(request).Content).token;
        }
    }
}