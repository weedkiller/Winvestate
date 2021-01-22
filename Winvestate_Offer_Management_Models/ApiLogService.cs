using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models
{
    [Table("apilog")]
    public class ApiLogService
    {
        [Key]
        public long Id { get; set; }

        public DateTime RequestTime { get; set; }

        public long ResponseMills { get; set; }

        public int StatusCode { get; set; }

        public string Method { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public string ApplicationName { get; set; }

        public string ApiCaller { get; set; }

        public string IpAddress { get; set; }

        public static ApiLogService GetModel(ApiLog v)
        {
            return new ApiLogService
            {
                Id = v.id,
                RequestTime = v.request_time,
                ResponseMills = v.response_millis,
                StatusCode = v.status_code,
                Method = v.method,
                Path = v.path,
                QueryString = v.query_string,
                RequestBody = v.request_body,
                ResponseBody = v.response_body,
                ApplicationName = v.application_name,
                ApiCaller = v.api_caller,

            };
        }

    }
}
