using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_API.Database
{
    public static class GetData
    {
        public static int CheckApiKey(string pPassword)
        {
            var loQuery = Queries.CheckApiUser;
            loQuery = loQuery.Replace("@P01", pPassword);

            using var connection = Connection.ConnectionMesnet();

            var loUsers = connection.Query<ApiUser>(loQuery).ToList();
            var loUserId = loUsers.Count > 0 ? loUsers.First().id : 0;
            return !Connection.OpenConnection(connection) ? 0 : loUserId;
        }
        public static UserDto ValidateUser(string pPhone, string pPassword)
        {
                var loQuery = Queries.ValidateUser;
                loQuery = loQuery.Replace("@P01", pPhone);
                loQuery = loQuery.Replace("@P02", pPassword.ToUpper());

                using var connection = Connection.ConnectionWinvestate();

                if (!Connection.OpenConnection(connection)) return null;

                var result = connection.Query<UserDto>(loQuery).ToList();
                return !result.Any() ? null : result.FirstOrDefault();
        }
        public static UserDto GetUserById(string pId)
        {
            var loQuery = Queries.GetUserById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<UserDto>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }
        public static List<UserDto> GetAllUsers()
        {
            var loQuery = Queries.GetAllUsers;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<UserDto>(loQuery).ToList();
            return !result.Any() ? new List<UserDto>(): result;
        }
        public static User GetUserByPhone(string pPhone)
        {
            var loQuery = Queries.GetUserByPhone;
            loQuery = loQuery.Replace("@P01", pPhone);

            using (var connection = Connection.ConnectionWinvestate())
            {
                if (!Connection.OpenConnection(connection)) return null;

                var result = connection.Query<User>(loQuery).ToList();
                return !result.Any() ? null : result.FirstOrDefault();
            }
        }
        public static Otp ValidateOtp(Otp pOtpService)
        {
            var loQuery = Queries.ValidateOtp;
            loQuery = loQuery.Replace("@P01", pOtpService.phone);
            loQuery = loQuery.Replace("@P02", pOtpService.otp_hash.ToUpper());

            using (var connection = Connection.ConnectionWinvestate())
            {
                if (!Connection.OpenConnection(connection)) return null;

                var result = connection.Query<Otp>(loQuery).ToList();
                return !result.Any() ? null : result.FirstOrDefault();
            }
        }
        public static Otp CheckUserAndWorkorderHaveUnvalidatedOtp(Otp pOtpService)
        {
            var loQuery = Queries.CheckOtp;
            loQuery = loQuery.Replace("@P01", pOtpService.phone);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Otp>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }
        public static SysType GetTypeWithId(int pOtpTypeId)
        {
            var loQuery = Queries.GetTypeWithId;
            loQuery = loQuery.Replace("@P01", pOtpTypeId.ToString());

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<SysType>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }
        public static Bank GetBankById(string pId)
        {
            var loQuery = Queries.GetBankById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Bank>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }
        public static List<Bank> GetAllBanks()
        {
            var loQuery = Queries.GetAllBanks;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Bank>(loQuery).ToList();
            return !result.Any() ? new List<Bank>() : result;
        }

        public static List<SysType> GetAllTypes()
        {
            var loQuery = Queries.GetAllTypes;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<SysType>(loQuery).ToList();
            return !result.Any() ? new List<SysType>() : result;
        }

        public static List<AssetDto> GetAssetForListing()
        {
            var loQuery = Queries.GetAllAssets;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<AssetDto>(loQuery).ToList();
            return !result.Any() ? new List<AssetDto>() : result;
        }

        public static List<AssetDto> GetAllAssets()
        {
            var loQuery = Queries.GetAllAssets;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<AssetDto>(loQuery).ToList();
            return !result.Any() ? new List<AssetDto>() : result;
        }
        public static AssetDto GetAssetById(string pId)
        {
            var loQuery = Queries.GetAssetById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<AssetDto>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static List<AssetPhoto> GetAssetPhotos(string pId)
        {
            var loQuery = Queries.GetAssetPhotosById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<AssetPhoto>(loQuery).ToList();
            return !result.Any() ? new List<AssetPhoto>() : result;
        }

        public static CustomerDto GetCustomerByIdentity(string pId)
        {
            var loQuery = Queries.GetCustomerByIdentity;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<CustomerDto>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static CustomerDto GetCustomerById(string pId)
        {
            var loQuery = Queries.GetCustomerById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<CustomerDto>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static Offer GetOfferByOwnerAndAssetId(string pOwnerId,string pAssetId)
        {
            var loQuery = Queries.GetOfferByOwnerAndAssetId;
            loQuery = loQuery.Replace("@P01", pOwnerId);
            loQuery = loQuery.Replace("@P02", pAssetId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Offer>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static Offer GetOfferById(string pId)
        {
            var loQuery = Queries.GetOfferById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Offer>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static Offer GetOfferByAgreementId(string pId)
        {
            var loQuery = Queries.GetOfferByAgreementId;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<Offer>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }

        public static List<OfferDto> GetOfferSummary()
        {
            var loQuery = Queries.GetOfferSummary;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<OfferDto>(loQuery).ToList();
            return !result.Any() ? new List<OfferDto>() : result;
        }

        public static List<CallbackRecord> GetActiveCallbackRecords()
        {
            var loQuery = Queries.GetActiveCallBackRecords;

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<CallbackRecord>(loQuery).ToList();
            return !result.Any() ? new List<CallbackRecord>() : result;
        }

        public static CallbackRecord GetCallbackRecordById(string pId)
        {
            var loQuery = Queries.GetCallbackRecordById;
            loQuery = loQuery.Replace("@P01", pId);

            using var connection = Connection.ConnectionWinvestate();
            if (!Connection.OpenConnection(connection)) return null;

            var result = connection.Query<CallbackRecord>(loQuery).ToList();
            return !result.Any() ? null : result.FirstOrDefault();
        }
    }
}
