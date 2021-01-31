using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_Models.Enums.Mespact;
using Winvestate_Offer_Management_Models.Enums.Otp;
using Winvestate_Offer_Management_Models.Mespact;

namespace Winvestate_Offer_Management_API.Classes
{
    public class HelperMethods
    {
        public static string GetCallerFromToken(System.Security.Principal.IIdentity pIdentity)
        {
            var loUser = "";

            if (pIdentity is ClaimsIdentity identity)
            {
                loUser = identity.Claims.FirstOrDefault(x => x.Type.Contains("name"))?.Value;

            }
            return loUser;
        }

        public static Guid GetApiUserIdFromToken(System.Security.Principal.IIdentity pIdentity)
        {
            var loUserId = new Guid();

            if (pIdentity is ClaimsIdentity identity)
            {
                var loTemp = identity.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"))?.Value;
                if (!Guid.TryParse(loTemp, out loUserId))
                {
                    loUserId = Guid.Empty;
                }

            }
            return loUserId;
        }

        public static int GetUserTypeFromToken(System.Security.Principal.IIdentity pIdentity)
        {
            var loUserType = -1;

            if (!(pIdentity is ClaimsIdentity identity)) return loUserType;

            var loTemp = identity.Claims.FirstOrDefault(x => x.Type.ToLower().Contains("gender"))?.Value;
            if (!int.TryParse(loTemp, out loUserType))
            {
                loUserType = -1;
            }
            return loUserType;
        }

        public static string SerializePhone(string pPhone)
        {
            if (string.IsNullOrEmpty(pPhone))
            {
                return "";
            }

            pPhone = pPhone.Replace(")", "");
            pPhone = pPhone.Replace("(", "");
            pPhone = pPhone.Replace("-", "");
            pPhone = pPhone.Replace(" ", "");

            if (pPhone.StartsWith("+90"))
                return pPhone;
            if (pPhone.StartsWith("90"))
                return "+" + pPhone;
            if (pPhone.StartsWith("0"))
                return "+9" + pPhone;

            return "+90" + pPhone;
        }

        public static string GenerateToken(string pUserId, int pUserType)
        {
            var someClaims = new[]{
                new Claim(JwtRegisteredClaimNames.NameId,pUserId),
                new Claim(JwtRegisteredClaimNames.Birthdate,DateTime.Now.ToShortDateString()),
                new Claim(JwtRegisteredClaimNames.Gender,pUserType.ToString()),
            };

            var token = new JwtSecurityToken
            (
                "MesnetBilisim2021",
                "2020Winvestate-Api",
                someClaims,
                expires: DateTime.UtcNow.AddDays(30), // 30 gün geçerli olacak,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2020 MesnetBilisimSigningCridential For Winvestate Api")),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string DecryptString(string pCipherString, string pName)
        {
            byte[] toDecryptArray = Convert.FromBase64String(pCipherString);
            var loResult = DecryptData(toDecryptArray, pName);
            return Encoding.UTF8.GetString(loResult);
        }

        public static string EncryptString(string pToEncrypt, string pName)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(pToEncrypt);
            var loResultArray = EncryptData(toEncryptArray, pName);
            return Convert.ToBase64String(loResultArray, 0, loResultArray.Length);
        }

        public static byte[] EncryptData(byte[] pToEncryptArray, string pName)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            var loEncryptionKey = new byte[] { 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45, 0x20, 0xCB, 0xA5, 0x12, 0x4A, 0xAF, 0xF8, 0x54, 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45 };

            if (!string.IsNullOrEmpty(pName))
            {
                var loNameMd5 = Encoding.UTF8.GetBytes(Md5OfString(pName));
                var loMax = loNameMd5.Length > loEncryptionKey.Length ? loEncryptionKey.Length : loNameMd5.Length;

                for (var i = 0; i < loMax; i++)
                {
                    loEncryptionKey[i] = loNameMd5[i];

                }
            }

            //set the secret key for the tripleDES algorithm
            tdes.Key = loEncryptionKey;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                (pToEncryptArray, 0, pToEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return resultArray;
        }

        public static byte[] DecryptData(byte[] pToDecryptArray, string pName)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            var loEncryptionKey = new byte[] { 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45, 0x20, 0xCB, 0xA5, 0x12, 0x4A, 0xAF, 0xF8, 0x54, 0x85, 0x45, 0xDB, 0xC1, 0x5A, 0xCB, 0xD8, 0x45 };

            if (!string.IsNullOrEmpty(pName))
            {
                var loNameMd5 = Encoding.UTF8.GetBytes(Md5OfString(pName));
                var loMax = loNameMd5.Length > loEncryptionKey.Length ? loEncryptionKey.Length : loNameMd5.Length;

                for (var i = 0; i < loMax; i++)
                {
                    loEncryptionKey[i] = loNameMd5[i];

                }
            }

            //set the secret key for the tripleDES algorithm
            tdes.Key = loEncryptionKey;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                (pToDecryptArray, 0, pToDecryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return resultArray;
        }

        public static string Md5OfString(string pPlainText)
        {
            var md5 = new MD5CryptoServiceProvider();

            var btr = Encoding.UTF8.GetBytes(pPlainText);
            btr = md5.ComputeHash(btr);

            var sb = new StringBuilder();

            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            return sb.ToString().ToUpper();
        }

        public static string GetOtpContent(int pOtpType, string pOtpContent)
        {
            var loMessageContent = "";
            var loOtpMessage = GetData.GetTypeWithId(pOtpType);

            if (pOtpType == (int)OtpTypes.ResetPassword || pOtpType == (int)OtpTypes.ChangePhone || pOtpType == (int)OtpTypes.RegisterForSubmit)
            {
                loMessageContent = loOtpMessage.type_value_tr.Replace("@P01", pOtpContent);
                loMessageContent = loMessageContent.Replace("@P02", DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
            }

            return loMessageContent;
        }

        public static AllTypes GetSysDefinitions()
        {
            var loSystemTypes = GetData.GetAllTypes();
            var loAllTypes = new AllTypes
            {
                asset_category = loSystemTypes.FindAll(x => x.type_name == "asset_category").OrderBy(x => x.list_order)
                    .ToList(),
                portfolio_ground = loSystemTypes.FindAll(x => x.type_name == "portfolio_ground")
                    .OrderBy(x => x.list_order).ToList(),
                portfolio_house = loSystemTypes.FindAll(x => x.type_name == "portfolio_house")
                    .OrderBy(x => x.list_order).ToList(),
                portfolio_office = loSystemTypes.FindAll(x => x.type_name == "portfolio_office")
                    .OrderBy(x => x.list_order).ToList()
            };


            return loAllTypes;
        }

        public static string RandomOtp()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomNumber(100000, 999999));
            return ConvertTrCharToEnChar(builder.ToString());
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        private static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string ConvertTrCharToEnChar(string text)
        {
            return string.Join("", text.Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
        }

        public static string RemoveBetween(string sourceString, string startTag, string endTag)
        {
            Regex regex = new Regex(string.Format("{0}(.*?){1}", Regex.Escape(startTag), Regex.Escape(endTag)), RegexOptions.RightToLeft);
            return regex.Replace(sourceString, startTag + endTag).Replace(startTag, "").Replace(endTag, "");
        }

        public static void SendToDocumentToSign(Customer pCustomer, Offer pOffer)
        {
            var loAsset = GetData.GetAssetById(pOffer.asset_uuid.ToString());
            var loCompany = GetData.GetBankById(loAsset.bank_guid.ToString());
            var loSendToSignModel = new SendToSign
            {
                document_to_sign = new DocumentToSign
                {
                    participant_guid = Common.MespactWinvestateUser,
                    document_type_guid = loCompany.mespact_agreement_uuid.ToString(),
                    erp_id = pOffer.agreement_uuid.ToString(),
                    participant_callback_url = Common.CallbackUrl
                },
                document_sign_flows = new List<DocumentSignFlow>()
            };

            var loSignFlow = new DocumentSignFlow
            {
                participant_name = pCustomer.customer_name,
                participant_surname = pCustomer.customer_surname,
                identity_number = pCustomer.identity_no,
                mail = pCustomer.mail,
                phone_number = SerializePhone(pCustomer.phone),
                user_type_system_type_id = pCustomer.user_type_system_type_id == 2 ? 1 : 4,
                document_template_key = ((int)ThkAgreement.ThkSatisSartname.Imzaci).ToString()
            };
            loSendToSignModel.document_sign_flows.Add(loSignFlow);


            var loDocumentTemplateKeys = new List<DocumentTemplate>();
            var loDocumentTemplateKey1 = new DocumentTemplate();
            var loDocumentTemplateKey2 = new DocumentTemplate();
            var loDocumentTemplateKey3 = new DocumentTemplate();
            var loDocumentTemplateKey4 = new DocumentTemplate();
            var loDocumentTemplateKey5 = new DocumentTemplate();
            var loDocumentTemplateKey6 = new DocumentTemplate();
            var loDocumentTemplateKey7 = new DocumentTemplate();
            var loDocumentTemplateKey8 = new DocumentTemplate();
            var loDocumentTemplateKey9 = new DocumentTemplate();
            var loDocumentTemplateKey10 = new DocumentTemplate();
            var loDocumentTemplateKey11 = new DocumentTemplate();
            var loDocumentTemplateKey12 = new DocumentTemplate();

            loDocumentTemplateKey1.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Tarih).ToString();
            loDocumentTemplateKey1.document_template_value = DateTime.Now.ToString("dd.MM.yyyy");
            loDocumentTemplateKeys.Add(loDocumentTemplateKey1);

            loDocumentTemplateKey2.document_template_key = ((int)ThkAgreement.ThkSatisSartname.GmNo).ToString();
            loDocumentTemplateKey2.document_template_value = loAsset.company_prefix+loAsset.asset_no;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey2);

            loDocumentTemplateKey3.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Il).ToString();
            loDocumentTemplateKey3.document_template_value = loAsset.city;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey3);

            loDocumentTemplateKey4.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Ilce).ToString();
            loDocumentTemplateKey4.document_template_value = loAsset.district;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey4);

            loDocumentTemplateKey5.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Ada).ToString();
            loDocumentTemplateKey5.document_template_value = loAsset.block_number;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey5);

            loDocumentTemplateKey6.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Parsel).ToString();
            loDocumentTemplateKey6.document_template_value = loAsset.plot_number;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey6);

            loDocumentTemplateKey7.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Adres).ToString();
            loDocumentTemplateKey7.document_template_value = loAsset.address;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey7);

            loDocumentTemplateKey8.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Adsoyad).ToString();
            loDocumentTemplateKey8.document_template_value = string.IsNullOrEmpty(pCustomer.company_name)
                ? pCustomer.customer_name + " " + pCustomer.customer_surname
                : pCustomer.company_name;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey8);

            loDocumentTemplateKey9.document_template_key = ((int)ThkAgreement.ThkSatisSartname.TcKimlik).ToString();
            loDocumentTemplateKey9.document_template_value = string.IsNullOrEmpty(pCustomer.tax_no) ? pCustomer.identity_no : pCustomer.tax_no;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey9);

            loDocumentTemplateKey10.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Posta).ToString();
            loDocumentTemplateKey10.document_template_value = pCustomer.address ?? "";
            loDocumentTemplateKeys.Add(loDocumentTemplateKey10);

            loDocumentTemplateKey11.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Gsm).ToString();
            loDocumentTemplateKey11.document_template_value = pCustomer.phone;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey11);

            loDocumentTemplateKey12.document_template_key = ((int)ThkAgreement.ThkSatisSartname.Eposta).ToString();
            loDocumentTemplateKey12.document_template_value = pCustomer.mail;
            loDocumentTemplateKeys.Add(loDocumentTemplateKey12);


            loSendToSignModel.document_templates = loDocumentTemplateKeys;
            var loResult = RestCalls.SendCustomerAgreement(loSendToSignModel);
            if (loResult.Code == 200)
            {
                var loSendToSign = JsonConvert.DeserializeObject<SendToSign>(loResult.Data.ToString());
                var loOfferToUpdate = GetData.GetOfferById(pOffer.row_guid.ToString());
                loOfferToUpdate.mespact_session_uuid = loSendToSign.document_to_sign.row_guid;
                Crud<Offer>.Update(loOfferToUpdate, out _);
            }
        }

        public static void SendNewCallBackRecord(CallbackRecord pCallbackRecord)
        {
            var loAsset = GetData.GetAssetById(pCallbackRecord.asset_uuid.ToString());
            var loMailContent = File.ReadAllText("winvestate_new_cr_mail.html");
            loMailContent = loMailContent.Replace("@CustomerName@", pCallbackRecord.applicant_name + " " + pCallbackRecord.applicant_surname);
            loMailContent = loMailContent.Replace("@CustomerPhone@", pCallbackRecord.applicant_phone);
            loMailContent = loMailContent.Replace("@AssetName@", loAsset.asset_no + " " + loAsset.asset_name);
            loMailContent = loMailContent.Replace("@OperationDate@", pCallbackRecord.row_create_date.Value.ToString("f"));
            loMailContent = loMailContent.Replace("@AssetLink@", "https://e-teklif.winvestate.com/Asset/AssetDetail?pId="+pCallbackRecord.asset_uuid);

            RestCalls.SendMail(loMailContent, Common.InfoMailList, "Yeni Geri Aranma Talebi");
        }

        public static void SendNewOfferInformation(Guid pId)
        {
            var loOffer = GetData.GetOfferById(pId.ToString());

            SendNewOfferSmsInformation(loOffer);

            var loAsset = GetData.GetAssetById(loOffer.asset_uuid.ToString());
            var loCustomer = GetData.GetCustomerById(loOffer.owner_uuid.ToString());
            var loCompany = GetData.GetBankById(loAsset.bank_guid.ToString());

            var loMailContent = File.ReadAllText("winvestate_new_offer.html");
            loMailContent = loMailContent.Replace("@AssetNo@", loAsset.asset_no.ToString());
            loMailContent = loMailContent.Replace("@AssetName@", loAsset.asset_name);
            loMailContent = loMailContent.Replace("@CustomerName@", loCustomer.customer_name + " " + loCustomer.customer_surname);
            loMailContent = loMailContent.Replace("@CustomerPhone@", loCustomer.phone);
            loMailContent = loMailContent.Replace("@OperationDate@", loOffer.row_create_date.Value.ToString("f"));
            loMailContent = loMailContent.Replace("@OfferAmount@", loOffer.price.Value.ToString("C0") + " TL");

            var loSender = Common.InfoMailList + ";" + loCompany.authorized_mail;

            RestCalls.SendMail(loMailContent, loSender, loCustomer.customer_name + " " + loCustomer.customer_surname + "-" + loAsset.asset_no + " " + "Yeni Teklif");
        }

        public static void SendNewOfferSmsInformation(Offer pOffer)
        {
            var loAssetOffers = GetData.GetActiveOfferByAssetId(pOffer.asset_uuid.ToString());
            var loCustomer = GetData.GetCustomerById(pOffer.owner_uuid.ToString());

            foreach (var loAssetOffer in loAssetOffers)
            {
                if (loAssetOffer.customer_phone == loCustomer.phone) continue;

                RestCalls.SendSms(
                    "Değerli müşterimiz, teklif vermiş olduğunuz gayrimenkule yeni bir teklif verilmiştir. Yeni teklifi görüntülemek ve teklifinizi yükseltmek için lütfen web sitemizi ziyaret ediniz İlan bağlantısı : https://e-teklif.winvestate.com/Asset/AssetDetail?pId="+pOffer.asset_uuid+". Mesaj Tarihi: " + DateTime.Now,
                    loAssetOffer.customer_phone);
            }
        }
    }
}
