using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Processing;
using Winvestate_Offer_Management_MVC.Classes;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(string sessionId)
        {
            var i = 0;
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length <= 0) continue;
                    string filePath = HelperMethods.GetTimestamp(DateTime.Now) + Path.GetExtension(file.FileName);
                    string filePath2 = HelperMethods.GetTimestamp(DateTime.Now.AddMilliseconds(10)) + Path.GetExtension(file.FileName);
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId));
                    filePath = Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId + "\\") + filePath;
                    filePath2 = Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId + "\\") + filePath2;

                    if (!file.ContentType.ToLower().Contains("image"))
                    {
                        await using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);
                    }
                    else
                    {

                        var loMyImage = System.Drawing.Image.FromStream(file.OpenReadStream());
                        loMyImage.Save(filePath);

                        var loMySecondImage = System.Drawing.Image.FromFile(filePath);
                        loMySecondImage.NormalizeOrientation();
                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(loMySecondImage, typeof(byte[]));
                        loMySecondImage.Dispose();
                        System.IO.File.Delete(filePath);
                        using var image = Image.Load(bytes);

                        if (image.Width > image.Height)
                        {
                            if (image.Width > 800)
                            {
                                image.Mutate(x => x.Resize(800, 800 * image.Height / image.Width));
                            }
                            else
                            {
                                image.Mutate(x => x.Resize(image.Width, image.Width * image.Height / image.Width));
                            }
                        }
                        else
                        {
                            if (image.Height > 450)
                            {
                                image.Mutate(x => x.Resize(450 * image.Width / image.Height, 450));
                            }
                            else
                            {
                                image.Mutate(x => x.Resize(image.Height * image.Width / image.Height, image.Height));
                            }
                        }

                        image.Save(filePath);
                        //System.Drawing.Image loImage = System.Drawing.Image.FromFile(_environment.WebRootPath + "/white_canvas.png");
                        //System.Drawing.Image loImage2 = System.Drawing.Image.FromFile(filePath);

                        //HelperMethods.CombineImages(loImage, loImage2, filePath2);
                        //HelperMethods.MergedBitmaps(loImage, loImage2, "emrenin_denemesi.jpg");

                        //loImage.Dispose();
                        //loImage2.Dispose();
                        //System.IO.File.Delete(filePath);

                        if (i == 0)
                        {
                            using var imageThumb = Image.Load(file.OpenReadStream());
                            if (imageThumb.Width > 100)
                            {
                                imageThumb.Mutate(x => x.Resize(100, 100 * imageThumb.Height / imageThumb.Width));
                            }
                            filePath = "thumb_" + HelperMethods.GetTimestamp(DateTime.Now) + Path.GetExtension(file.FileName);
                            filePath = Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId + "\\") + filePath;
                            imageThumb.Save(filePath);
                        }
                    }

                    i++;
                }

                return Ok(new { success = true, message = "File Uploaded" });
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("file_error.txt", ex.ToString());
                return BadRequest(new { success = false, message = "Error file failed to upload" });
            }
        }
    }
}
