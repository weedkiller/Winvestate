using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Processing;
using Winvestate_Offer_Management_MVC.Classes;
using SixLabors.ImageSharp;

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
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId));
                    filePath = Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + sessionId + "\\") + filePath;

                    if (!file.ContentType.ToLower().Contains("image"))
                    {
                        await using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);
                    }
                    else
                    {
                        using var image = Image.Load(file.OpenReadStream());
                        if (image.Width > 1024)
                        {
                            image.Mutate(x => x.Resize(1024, 1024 * image.Height / image.Width));
                        }

                        image.Save(filePath);

                        if (i == 0)
                        {
                            using var imageThumb = Image.Load(file.OpenReadStream());
                            if (imageThumb.Width > 100)
                            {
                                imageThumb.Mutate(x => x.Resize(100, 100 * imageThumb.Height / imageThumb.Width));
                            }
                            filePath = "thumb_"+HelperMethods.GetTimestamp(DateTime.Now) + Path.GetExtension(file.FileName);
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
                return BadRequest(new { success = false, message = "Error file failed to upload" });
            }
        }
    }
}
