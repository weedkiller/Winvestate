﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Winvestate_Offer_Management_MVC.Classes
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult("Google reCAPTCHA validation failed", new String[] { validationContext.MemberName }));

            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                return errorResult.Value;
            }

            IConfiguration configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            String reCaptchResponse = value.ToString();
            String reCaptchaSecret = "6LfrSTsaAAAAAAKkrmuE3GdPYyHncKhUWO8TwreU";


            HttpClient httpClient = new HttpClient();
            var httpResponse = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchResponse}").Result;
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return errorResult.Value;
            }

            String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != true.ToString().ToLower())
            {
                return errorResult.Value;
            }

            return ValidationResult.Success;

        }
    }
}