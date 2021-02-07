using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;
using Winvestate_Offer_Management_MVC.Api;
using Winvestate_Offer_Management_MVC.Classes;
using Winvestate_Offer_Management_MVC.Models;
using Winvestate_Offer_Management_MVC.Session;

namespace Winvestate_Offer_Management_MVC.Controllers
{
    public class AssetController : Controller
    {
        private IWebHostEnvironment _environment;

        public AssetController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [SessionTimeout]
        [CheckAuthorize]
        public IActionResult Info(string pId)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            loUser.session_id = Guid.NewGuid().ToString();
            HttpContext.Session.SetObject("User", loUser);

            if (string.IsNullOrEmpty(pId))
            {
                return View(new HomeViewModel { User = loUser, AssetPhotos = new List<AssetPhoto>() });
            }

            var loAsset = RestCalls.GetAssetById(pId, loUser.token);
            var loModel = new HomeViewModel()
            {
                User = loUser,
                Asset = loAsset
            };

            loModel.AssetExplanation = loModel.Asset.explanation;
            loModel.AssetPhotos = loModel.Asset.asset_photos ?? new List<AssetPhoto>();
            loModel.Asset.explanation = "";
            loModel.Asset.asset_photos = new List<AssetPhoto>();
            return View(loModel);
        }

        [HttpPost]
        [SessionTimeout]
        [OnlyAdmin]
        public JsonResult Save([FromBody] AssetDto pAsset)
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");

            var loAssetFiles = new List<AssetPhoto>();
            var loFileListToSave = new List<FileModel>();

            if (Directory.Exists(_environment.WebRootPath + "\\" + "Uploads\\Temp\\" + loUser.session_id))
            {
                string[] filePaths = Directory.GetFiles(_environment.WebRootPath + "\\" + "Uploads\\Temp\\" + loUser.session_id);
                foreach (var file in filePaths)
                {
                    var loFileModel = new FileModel
                    {
                        FileContent = System.IO.File.ReadAllBytes(file),
                        FileName = Path.GetFileName(file),
                        FilePath = loUser.session_id
                    };

                    var loAssetFile = new AssetPhoto
                    {
                        file_path = loFileModel.FilePath + "\\" + loFileModel.FileName
                    };

                    if (loFileModel.FileName.Contains("thumb_"))
                    {
                        pAsset.thumb_path = loAssetFile.file_path;
                        loAssetFile.is_thumb = true;
                    }

                    loFileListToSave.Add(loFileModel);
                    loAssetFiles.Add(loAssetFile);
                }
            }

            if (!loFileListToSave.Any() || HelperMethods.SaveFilesToFileServer(_environment.WebRootPath + "\\" + "Uploads\\", loFileListToSave))
            {
                if (pAsset.asset_photos != null && pAsset.asset_photos.Any())
                {
                    pAsset.asset_photos.AddRange(loAssetFiles);
                }
                else
                {
                    pAsset.asset_photos = loAssetFiles;
                }

                pAsset.asset_photos.ForEach(x => x.file_path = x.file_path.Replace(",", "\\"));

                HelperMethods.DeleteFiles(pAsset.asset_photos.FindAll(x => x.is_deleted), _environment.WebRootPath + "\\" + "Uploads\\");

                //var loAsset = RestCalls.UpdateAsset(pAsset, loUser.token);

                var loAsset = RestCalls.SaveAsset(pAsset, loUser.token);

                if (!string.IsNullOrEmpty(loAsset.message) || loAsset.id <= 0) return Json(loAsset);

                try
                {
                    if (loFileListToSave.Any()) // yeni kayıt varsa sil templeri!
                        Directory.Delete(Path.Combine(_environment.WebRootPath, "Uploads\\Temp\\" + loUser.session_id), true); // temp resimleri sil
                }
                catch (Exception)
                {
                    //ignored;
                }


                return Json(loAsset);

            }

            return Json(new AssetDto());
        }


        public IActionResult AssetForOffer()
        {
            HttpContext.Session.Remove("User");
            var loToken = RestCalls.GetToken();
            return View(new HomeViewModel { Token = loToken, Cities = RestCalls.GetCities(loToken) });
        }

        [SessionTimeout]
        [OnlyAdmin]
        public IActionResult List()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }

        [OnlyWinvestate]
        public IActionResult Callback()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }

        [OnlyAdmin]
        public IActionResult Sold()
        {
            var loUser = HttpContext.Session.GetObject<UserDto>("User");
            return View(new HomeViewModel { User = loUser });
        }

        public IActionResult AssetDetail(string pId)
        {
            var loToken = RestCalls.GetToken();
            var loAsset = RestCalls.GetAssetById(pId, loToken);

            if (loAsset != null)
            {
                var loModel = new HomeViewModel { Asset = loAsset, Token = loToken };
                loModel.AssetExplanation = loModel.Asset.explanation;
                loModel.Asset.explanation = "";
                loModel.User = HttpContext.Session.GetObject<UserDto>("User");
                return View(loModel);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
