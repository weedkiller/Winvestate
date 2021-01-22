using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            var loUser = GetData.ValidateUser(pUser.phone, pUser.password);

            if (loUser != null)
            {
                loUser.password = "";
                loUser.token = HelperMethods.GenerateToken(loUser.row_guid.ToString());
                loUser.sys_definitions = HelperMethods.GetSysDefinitions();
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
        public ActionResult<GenericResponseModel> UpdatePassword([FromBody] User pUser)
        {
            var loUserId = HelperMethods.GetApiUserIdFromToken(HttpContext.User.Identity);
            var loGenericResponse = new GenericResponseModel
            {
                Code = -1,
                Status = "Fail"
            };

            pUser.phone = HelperMethods.SerializePhone(pUser.phone);
            var loUser = GetData.GetUserByPhone(pUser.phone);

            if (loUser == null)
            {
                loGenericResponse.Message = "No such user!";
                return loGenericResponse;
            }
            loUser.password = pUser.password ?? loUser.password;
            loUser.password = loUser.password.ToUpper();
            loUser.row_update_date = DateTime.Now;
            loUser.row_update_user = loUserId;

            if (!Crud<User>.Update(loUser, out _)) return loGenericResponse;

            loGenericResponse.Code = 200;
            loGenericResponse.Status = "OK";
            loGenericResponse.Data = loUser;

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
