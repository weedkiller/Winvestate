using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models
{
    [Table("apilog")]
    public class ApiLog
    {
        [Key]
        public long id { get; set; }

        public DateTime request_time { get; set; }

        public long response_millis { get; set; }

        public int status_code { get; set; }

        public string method { get; set; }

        public string path { get; set; }

        public string query_string { get; set; }

        public string request_body { get; set; }

        public string response_body { get; set; }

        public string application_name { get; set; }

        public string api_caller { get; set; }

        public string ip_address { get; set; }

        public static ApiLog GetModel(ApiLogService v)
        {
            return new ApiLog
            {
                id = v.Id,
                request_time = v.RequestTime,
                response_millis = v.ResponseMills,
                status_code = v.StatusCode,
                method = v.Method,
                path = v.Path,
                query_string = v.QueryString,
                request_body = v.RequestBody,
                response_body = v.ResponseBody,
                application_name = v.ApplicationName,
                api_caller = v.ApiCaller,
                ip_address = v.IpAddress

            };
        }

    }
}
