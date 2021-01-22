using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models
{
    [Table("apilog")]
    public class ApiUserService
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ApiKey { get; set; }

        public string DeviceId { get; set; }

        public string Language { get; set; }

        public string BankId { get; set; }

        public static ApiUserService GetModel(ApiUser v)
        {
            return new ApiUserService
            {
                Id = v.id,
                ApiKey = v.api_key,
                DeviceId = v.device_id,
                Language = v.language,
                UserName = v.user_name,
                Password = v.password,

            };
        }
    }
}
