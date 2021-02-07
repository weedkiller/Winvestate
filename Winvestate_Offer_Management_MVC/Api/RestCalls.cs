using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;
using RestSharp;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Mespact;
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;

namespace Winvestate_Offer_Management_MVC.Api
{
    public class RestCalls
    {


        public static string GetToken()
        {
            var client = new RestClient(Common.ApiUrl + "/Token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loMyBody = new
            {
                api_key = Common.ApiKey,
                language = "tr",
            };

            var loModel = JsonConvert.SerializeObject(loMyBody);
            request.AddJsonBody(loModel);

            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loToken = JsonConvert.DeserializeObject<GeneratedToken>(result.Content);

            return loToken.token;
        }

        public static UserDto ValidateUser(User pUser)
        {
            var client = new RestClient(Common.ApiUrl + "/User/Validate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            var loModel = JsonConvert.SerializeObject(pUser);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                var loUser = JsonConvert.DeserializeObject<UserDto>(loGenericResult.Data.ToString());
                return loUser;
            }

            return new UserDto();
        }

        public static UserDto SaveNewUser(UserDto pUser, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/User");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            var loModel = JsonConvert.SerializeObject(pUser);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loUser = new UserDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loUser = JsonConvert.DeserializeObject<UserDto>(loGenericResult.Data.ToString());
                return loUser;
            }

            loUser.message = loGenericResult?.Message;
            return loUser;
        }

        public static UserDto UpdateUser(UserDto pUser, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/User");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            var loModel = JsonConvert.SerializeObject(pUser);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loUser = new UserDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loUser = JsonConvert.DeserializeObject<UserDto>(loGenericResult.Data.ToString());
                return loUser;
            }

            loUser.message = loGenericResult?.Message;
            return loUser;
        }

        public static BankDto SaveNewBank(Bank pBank, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Bank");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            var loModel = JsonConvert.SerializeObject(pBank);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loBank = new BankDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loBank = JsonConvert.DeserializeObject<BankDto>(loGenericResult.Data.ToString());
                return loBank;
            }

            loBank.message = loGenericResult?.Message;
            return loBank;
        }

        public static BankDto UpdateBank(Bank pBank, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Bank");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            var loModel = JsonConvert.SerializeObject(pBank);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loBank = new BankDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loBank = JsonConvert.DeserializeObject<BankDto>(loGenericResult.Data.ToString());
                return loBank;
            }

            loBank.message = loGenericResult?.Message;
            return loBank;
        }

        public static AssetDto SaveAsset(AssetDto pAsset, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Asset");
            var request = pAsset.id > 0 ? new RestRequest(Method.PUT) : new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            var loModel = JsonConvert.SerializeObject(pAsset);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loBank = new AssetDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loBank = JsonConvert.DeserializeObject<AssetDto>(loGenericResult.Data.ToString());
                return loBank;
            }

            loBank.message = loGenericResult?.Message;
            return loBank;
        }

        public static AssetDto GetAssetById(string pId, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Asset/" + pId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loBank = new AssetDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loBank = JsonConvert.DeserializeObject<AssetDto>(loGenericResult.Data.ToString());
                return loBank;
            }

            loBank.message = loGenericResult?.Message;
            return loBank;
        }

        public static CustomerDto SaveNewCustomer(CustomerDto pCustomer)
        {
            var client = new RestClient(Common.ApiUrl + "/Customer");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + GetToken());
            var loModel = JsonConvert.SerializeObject(pCustomer);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loCustomer = new CustomerDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loCustomer = JsonConvert.DeserializeObject<CustomerDto>(loGenericResult.Data.ToString());
                return loCustomer;
            }

            loCustomer.message = loGenericResult?.Message;
            return loCustomer;
        }

        public static CallbackRecordDto SaveNewCallback(CallbackRecaptcha pCallback)
        {
            var client = new RestClient(Common.ApiUrl + "/Customer/Callback");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + GetToken());
            var loModel = JsonConvert.SerializeObject(pCallback);
            request.AddJsonBody(loModel);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loCustomer = new CallbackRecordDto();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loCustomer = JsonConvert.DeserializeObject<CallbackRecordDto>(loGenericResult.Data.ToString());
                return loCustomer;
            }

            loCustomer.message = loGenericResult?.Message;
            return loCustomer;
        }

        public static List<DocumentType> GetContractTypes(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/User/Contracts");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loContracts = new List<DocumentType>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loContracts = JsonConvert.DeserializeObject<List<DocumentType>>(loGenericResult.Data.ToString());
                return loContracts;
            }

            return loContracts;
        }

        public static List<Bank> GetAllBanks(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Bank");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loBank = new List<Bank>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loBank = JsonConvert.DeserializeObject<List<Bank>>(loGenericResult.Data.ToString());
                return loBank;
            }


            return loBank;
        }

        public static List<OfferDto> GetAllOffers(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Offer/Summary");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loOffers = new List<OfferDto>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loOffers = JsonConvert.DeserializeObject<List<OfferDto>>(loGenericResult.Data.ToString());
                return loOffers;
            }

            return loOffers;
        }

        public static List<AddressKeyValue> GetCities(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Address/City");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loCities = new List<AddressKeyValue>();

            if (loGenericResult != null && loGenericResult.Code == 200)
            {
                loCities = JsonConvert.DeserializeObject<List<AddressKeyValue>>(loGenericResult.Data.ToString());
                return loCities;
            }

            return loCities;
        }

        public static List<OfferDto> GetActiveOffers(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Offer/Active");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loOffers = new List<OfferDto>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loOffers = JsonConvert.DeserializeObject<List<OfferDto>>(loGenericResult.Data.ToString());
                return loOffers;
            }

            return loOffers;
        }

        public static List<CallbackRecordDto> GetNewCallbackRecords(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Customer/ActiveCallback");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loCallbackRecords = new List<CallbackRecordDto>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loCallbackRecords = JsonConvert.DeserializeObject<List<CallbackRecordDto>>(loGenericResult.Data.ToString());
                return loCallbackRecords;
            }

            return loCallbackRecords;
        }

        public static List<AssetDto> GetOfferedAssets(string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Asset/Offered");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);
            var loGenericResult = JsonConvert.DeserializeObject<GenericResponseModel>(result.Content);
            var loOffers = new List<AssetDto>();

            if (loGenericResult != null && loGenericResult.Code == 200 && loGenericResult.Status.ToLower() == "ok")
            {
                loOffers = JsonConvert.DeserializeObject<List<AssetDto>>(loGenericResult.Data.ToString());
                return loOffers;
            }

            return loOffers;
        }

        public static PdfContent GetSignedDocument(string pId, string pToken)
        {
            var client = new RestClient(Common.ApiUrl + "/Offer/Signed/" + pId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + pToken);
            //var responseData = client.Execute(request).Content;
            var result = client.Execute(request);

            return JsonConvert.DeserializeObject<PdfContent>(result.Content);

        }



    }
}
