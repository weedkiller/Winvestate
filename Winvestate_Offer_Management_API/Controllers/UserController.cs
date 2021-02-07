using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpPost("Validate")]
        [AllowAnonymous]
        public ActionResult<GenericResponseModel> ValidateUser([FromBody] User pUser)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (string.IsNullOrEmpty(pUser.phone))
            {
                loGenericResponse.Message = "Lütfen kullanıcı adınızı giriniz!";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pUser.password))
            {
                loGenericResponse.Message = "Lütfen şifrenizi giriniz!";
                return loGenericResponse;
            }

            pUser.phone = HelperMethods.SerializePhone(pUser.phone);
            pUser.password = pUser.password.ToUpper();
            var loLogin = GetData.ValidateUser(pUser.phone, pUser.password);

            if (loLogin != null)
            {
                UserDto loUser = new UserDto();
                if (loLogin.user_type == 1 || loLogin.user_type == 2) //1 Winvestate admin 2 Winvestate user
                {
                    loUser = GetData.GetUserById(loLogin.row_guid.ToString());
                    loUser.password = "";
                    loUser.token = HelperMethods.GenerateToken(loUser.row_guid.ToString(), loLogin.user_type);
                    loUser.sys_definitions = HelperMethods.GetSysDefinitions();
                    loUser.user_type = loLogin.user_type;
                }
                else if (loLogin.user_type == 3) //Müşteri
                {
                    var loCustomer = GetData.GetCustomerById(loLogin.row_guid.ToString());
                    loUser.password = "";
                    loUser.token = HelperMethods.GenerateToken(loLogin.row_guid.ToString(), loLogin.user_type);
                    loUser.sys_definitions = HelperMethods.GetSysDefinitions();
                    loUser.name = loCustomer.customer_name;
                    loUser.surname = loCustomer.customer_surname;
                    loUser.phone = loCustomer.phone;
                    loUser.row_guid = loCustomer.row_guid;
                    loUser.id = loCustomer.id;
                    loUser.user_type = loLogin.user_type;
                }
                else // 4 Kurum
                {
                    var loCompany = GetData.GetBankById(loLogin.row_guid.ToString());
                    loUser.password = "";
                    loUser.token = HelperMethods.GenerateToken(loLogin.row_guid.ToString(), loLogin.user_type);
                    loUser.sys_definitions = HelperMethods.GetSysDefinitions();
                    loUser.name = loCompany.authorized_name;
                    loUser.surname = loCompany.authorized_surname;
                    loUser.phone = loCompany.authorized_phone;
                    loUser.row_guid = loCompany.row_guid;
                    loUser.id = loCompany.id;
                    loUser.user_type = loLogin.user_type;
                }

                loGenericResponse.Data = loUser;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geçersiz Kullanıcı adı veya Şifre";
            }

            return loGenericResponse;
        }

        [HttpPut("Password")]
        public ActionResult<GenericResponseModel> UpdatePassword([FromBody] UserDto pUser)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            if (pUser.user_type == 1 || pUser.user_type == 2) //1 Winvestate admin 2 Winvestate user
            {
                var loUser = GetData.GetUserById(pUser.row_guid.ToString());
                loUser.password = pUser.password ?? loUser.password;
                loUser.password = loUser.password.ToUpper();
                loUser.row_update_date = DateTime.Now;
                loUser.row_update_user = loUserId;
                if (!Crud<User>.Update(loUser, out _)) return loGenericResponse;
            }
            else if (pUser.user_type == 3) //Müşteri
            {
                var loCustomer = GetData.GetCustomerById(pUser.row_guid.ToString());
                loCustomer.password = pUser.password ?? loCustomer.password;
                loCustomer.password = loCustomer.password.ToUpper();
                loCustomer.row_update_date = DateTime.Now;
                loCustomer.row_update_user = loUserId;
                if (!Crud<Customer>.Update(loCustomer, out _)) return loGenericResponse;
            }
            else // 4 Kurum
            {
                var loBank = GetData.GetBankById(pUser.row_guid.ToString());
                loBank.authorized_password = pUser.password ?? loBank.authorized_password;
                loBank.authorized_password = loBank.authorized_password.ToUpper();
                loBank.row_update_date = DateTime.Now;
                loBank.row_update_user = loUserId;
                if (!Crud<Bank>.Update(loBank, out _)) return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";

            return loGenericResponse;
        }

        [HttpPut("Password/Forgot")]
        public ActionResult<GenericResponseModel> ForgottenPassword([FromBody] User pUser)
        {
            OtpController pOtp = new OtpController();
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            pUser.phone = HelperMethods.SerializePhone(pUser.phone);
            var loLogin = GetData.CheckUser(pUser.phone);

            if (loLogin == null) return loGenericResponse;

            var loOtp = new Otp();

            var loUser = new UserDto();
            loUser.row_guid = loLogin.row_guid;
            loUser.user_type = loLogin.user_type;

            if (loLogin.user_type == 1 || loLogin.user_type == 2) //1 Winvestate admin 2 Winvestate user
            {
                loOtp.phone = loLogin.phone;
                loUser.phone = loLogin.phone;
            }
            else if (loLogin.user_type == 3) //Müşteri
            {
                var loCustomer = GetData.GetCustomerById(loLogin.row_guid.ToString());
                loOtp.phone = loCustomer.phone;
                loUser.phone = loCustomer.phone;

            }
            else // 4 Kurum
            {
                var loCompany = GetData.GetBankById(loLogin.row_guid.ToString());
                loOtp.phone = loCompany.authorized_phone;
                loUser.phone = loCompany.authorized_phone;
            }

            loOtp.message_type_system_type_id = 2;
            var loResult = pOtp.SendOtp(loOtp);

            if (loResult.Value.Code != 200)
            {
                return loResult.Value;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Data = loUser;
            loGenericResponse.Message = "";
            loGenericResponse.Status = "ok";

            return loGenericResponse;
        }

        [HttpPut]
        public ActionResult<GenericResponseModel> Update([FromBody] User pUser)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            pUser.phone = HelperMethods.SerializePhone(pUser.phone);
            var loUser = GetData.GetUserById(pUser.row_guid.ToString());

            if (loUser == null)
            {
                loGenericResponse.Message = "Kullanıcı bulunamadı!";
                return loGenericResponse;
            }

            loUser.password = pUser.password ?? loUser.password;
            loUser.name = pUser.name ?? loUser.name;
            loUser.surname = pUser.surname ?? loUser.surname;
            loUser.mail = pUser.mail ?? loUser.mail;
            loUser.phone = pUser.phone ?? loUser.phone;
            loUser.is_active = pUser.is_active ?? loUser.is_active;
            loUser.is_deleted = pUser.is_deleted ?? loUser.is_deleted;
            loUser.user_type = pUser.user_type ?? loUser.user_type;
            loUser.password = loUser.password.ToUpper();
            loUser.name = loUser.name.ToUpper();
            loUser.surname = loUser.surname.ToUpper();
            loUser.mail = loUser.mail.ToLower();
            loUser.row_update_date = DateTime.Now;
            loUser.row_update_user = loUserId;

            if (!Crud<User>.Update(loUser, out _)) return loGenericResponse;

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loUser;

            return loGenericResponse;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<GenericResponseModel> Insert([FromBody] User pObject)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Status = "Fail",
                Code = -1
            };

            if (string.IsNullOrEmpty(pObject.phone))
            {
                loGenericResponse.Message = "Telefon numarası girilmeden işleme devam edilemez!";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pObject.password))
            {
                loGenericResponse.Message = "Şifre girilmeden işleme devam edilemez.";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pObject.mail))
            {
                loGenericResponse.Message = "Mail adresi girilmeden işleme devam edilemez.";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pObject.name))
            {
                loGenericResponse.Message = "İsim girilmeden işleme devam edilemez.";
                return loGenericResponse;
            }

            if (string.IsNullOrEmpty(pObject.surname))
            {
                loGenericResponse.Message = "Soyisim girilmeden işleme devam edilemez.";
                return loGenericResponse;
            }

            pObject.phone = HelperMethods.SerializePhone(pObject.phone);
            pObject.row_create_date = DateTime.Now;
            pObject.row_create_user = loUserId;
            pObject.row_guid = Guid.NewGuid();
            pObject.is_deleted = false;
            pObject.is_active = true;
            pObject.password = pObject.password.ToUpper();
            pObject.name = pObject.name.ToUpper();
            pObject.surname = pObject.surname.ToUpper();
            pObject.mail = pObject.mail.ToLower();
            var loResult = Crud<User>.Insert(pObject, out _);


            if (loResult > 0)
            {
                pObject.id = (int)loResult;
                loGenericResponse.Data = pObject;
                loGenericResponse.Status = "Ok";
                loGenericResponse.Code = 200;
            }
            else
            {
                loGenericResponse.Status = "Fail";
                loGenericResponse.Code = -1;
                loGenericResponse.Message = "Geçersiz Kullanıcı adı veya Şifre";
            }

            return loGenericResponse;
        }

        [HttpGet]
        public ActionResult<GenericResponseModel> GetAll()
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetAllUsers();

            if (!loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı kullanıcı bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("{pId}")]
        public ActionResult<GenericResponseModel> GetById(string pId)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = GetData.GetUserById(pId);

            if (loResult == null)
            {
                loGenericResponse.Message = "Kayıtlı kullanıcı bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }

        [HttpGet("Contracts")]
        public ActionResult<GenericResponseModel> GetUserContracts(string pId)
        {
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            var loResult = RestCalls.GetMespactDocumentTypes();

            if (loResult == null || !loResult.Any())
            {
                loGenericResponse.Message = "Kayıtlı doküman bulunamadı";
                return loGenericResponse;
            }

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loResult;

            return loGenericResponse;
        }
    }
}
