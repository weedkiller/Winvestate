using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;
using Winvestate_Offer_Management_Models.Database.Log;

namespace Winvestate_Offer_Management_Models.Database
{
    [Table("crashlog")]
    public class CrashlogService
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ScreenName { get; set; }

        public int Line { get; set; }

        public DateTime ExceptionTime { get; set; }

        public string Exception { get; set; }

        public string ApplicationName { get; set; }

        public static CrashlogService GetModel(Crashlog v)
        {
            return new CrashlogService
            {
                Id = v.id,
                UserId = v.user_id,
                ScreenName = v.screen_name,
                Line = v.line,
                ExceptionTime = v.exception_time,
                Exception = v.exception,
                ApplicationName = v.application_name,
            };
        }
    }
}
