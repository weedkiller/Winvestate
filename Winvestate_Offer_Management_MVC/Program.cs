using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Classes;
using SixLabors.ImageSharp;
using Image = System.Drawing.Image;

namespace Winvestate_Offer_Management_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.SetVariablesForDebug();
            Common.SetVariablesForTest();
            Common.SetVariablesForProd();

            //var loDirectories = Directory.GetDirectories(
            //    @"C:\Users\Emre\source\repos\Winvestate_Offer_Management\Winvestate_Offer_Management_MVC\wwwroot\Uploads");

            //foreach (var loDirectory in loDirectories)
            //{
            //    Directory.CreateDirectory(loDirectory.Replace("Uploads", "Uploads2"));
            //    Directory.CreateDirectory(loDirectory.Replace("Uploads", "Uploads3"));

            //    var loFiles = System.IO.Directory.GetFiles(loDirectory);
            //    foreach (var loFile in loFiles)
            //    {
            //        if (loFile.StartsWith("thumb") || loFile.Contains("HEIC")) continue;

            //        var loImage1 = @"C:\Users\Emre\source\repos\Winvestate_Offer_Management\Winvestate_Offer_Management_MVC\wwwroot\white_canvas.png";

            //        using var image = SixLabors.ImageSharp.Image.Load(System.IO.File.ReadAllBytes(loFile));

            //        if (image.Width > image.Height)
            //        {
            //            if (image.Width > 800)
            //            {
            //                image.Mutate(x => x.Resize(800, 800 * image.Height / image.Width));
            //            }
            //            else
            //            {
            //                image.Mutate(x => x.Resize(image.Width, image.Width * image.Height / image.Width));
            //            }
            //        }
            //        else
            //        {
            //            if (image.Height > 450)
            //            {
            //                image.Mutate(x => x.Resize(450 * image.Width / image.Height, 450));
            //            }
            //            else
            //            {
            //                image.Mutate(x => x.Resize(image.Height * image.Width / image.Height, image.Height));
            //            }
            //        }

            //        var loFilePath = loFile.Replace("Uploads", "Uploads3");

            //        image.Save(loFilePath);
            //        System.Drawing.Image loImage2 = System.Drawing.Image.FromFile(loFilePath);

            //        var loPath = loFile.Replace("Uploads", "Uploads2");
            //        HelperMethods.CombineImages(System.Drawing.Image.FromFile(loImage1), loImage2, loPath);



            //    }
            //}


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
