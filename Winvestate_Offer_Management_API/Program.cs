using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Winvestate_Offer_Management_API.Api;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;

namespace Winvestate_Offer_Management_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.InitVariablesForDebug();
            Common.InitVariablesForTest();
            Common.InitVariablesForProd();
            Connection.PrepareDatabase();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
