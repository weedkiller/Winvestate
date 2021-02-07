using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winvestate_Offer_Management_API.Database
{
    public static class Queries
    {
        public static string ValidateUser = "select * from vw_logins where user_name = '@P01' and upper(password)= upper('@P02')";
        public static string CheckUser = "select * from vw_logins where user_name = '@P01'";
        public static string GetAllUsers = "select * from winvestate_user where is_deleted=false and is_active=true order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetUserById = "select * from winvestate_user where   row_guid::text='@P01' and is_deleted=false and is_active=true";
        public static string GetUserByPhone = "Select * from winvestate_user where phone='@P01' and is_deleted=false and is_active=true";
        public static string CheckApiUser = "Select * from apiuser where api_key='@P01' and application='WinvestateAPI'";
        public static string ValidateOtp = "Select * from sys_otp where validation_state=0 and phone='@P01' and upper(otp_hash)=upper('@P02') order by id desc";
        public static string CheckOtp = "Select * from sys_otp where validation_state=0 and phone='@P01' order by row_create_date desc";
        public static string GetTypeWithId = "Select * from sys_type where id=@P01";
        public static string GetAllBanks = "select * from winvestate_bank where is_deleted=false and is_active=true order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetBankById = "select * from winvestate_bank where   row_guid::text='@P01' and is_deleted=false and is_active=true";
        public static string GetCustomerByIdentity = "select * from winvestate_customer where   identity_no::text='@P01' and is_deleted=false and is_active=true";
        public static string GetCustomerById = "select * from winvestate_customer where row_guid::text='@P01' and is_deleted=false and is_active=true";
        public static string GetAssetById = "select * from vw_all_assets where  row_guid::text='@P01' and is_deleted=false and is_active=true";
        public static string GetAssetPhotosById = "select * from winvestate_asset_photos where asset_uuid::text='@P01'";
        public static string GetCallbackRecordById = "select * from winvestate_callback_record where row_guid::text='@P01'";
        public static string GetActiveCallBackRecords = "select * from winvestate_callback_record where is_active=true order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetAllTypes = "select id,type_value_tr as type_value,type_name,COALESCE( list_order,0) list_order from sys_type";
        public static string GetAssetsForListing = "select * from vw_asset_for_listing order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetAllAssets = "select * from vw_all_assets order by city";
        public static string GetAllAssetsForUser = "select * from vw_all_assets where row_create_user::text='@P01' order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetAllAssetsForCompany = "select * from vw_all_assets where bank_guid::text='@P01' order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetOfferByOwnerAndAssetId = "select * from winvestate_offer where  owner_uuid::text='@P01' and  asset_uuid::text='@P02' and  is_deleted=false and is_active=true";
        public static string GetOfferById = "select * from winvestate_offer where  row_guid::text='@P01' and is_deleted=false and is_active=true";
        public static string GetOfferByAgreementId = "select * from winvestate_offer where  agreement_uuid::text='@P01' and offer_state_type_system_type_id=37 and is_deleted=false and is_active=true";
        public static string GetOfferSummary = "select * from vw_offer_summary order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetAllActiveOffers = "select * from vw_active_offers order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetActiveOffersByCustomerId = "select * from vw_active_offers where owner_uuid::text ='@P01'order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetActiveOffersByBankId = "select * from vw_active_offers where bank_uuid::text ='@P01' order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetActiveOffersByAssetId = "select * from vw_active_offers where asset_uuid::text ='@P01' order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetOfferHistoryByOfferId = "select * from winvestate_offer_history where offer_uuid::text ='@P01' order by row_create_date desc";
        public static string GetOfferHistoryByAssetId = "select * from vw_offer_history where asset_uuid::text ='@P01' order by row_create_date desc";
        public static string GetNewCallbackRecords = "select * from vw_callback_records where callback_record_state_type_system_type_id in(43,44)  order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetAllCalbacks = "select * from vw_callback_records order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetOfferedAssets = "select * from vw_all_assets where max_offer_amount > 0  and state_id in (0,1) order by COALESCE(row_update_date,row_create_date) desc";
        public static string GetCities = "select * from mesnet_cities order by text";
        public static string GetDistricts = "select * from mesnet_districts where city_id='@P01' order by text";
    }
}
