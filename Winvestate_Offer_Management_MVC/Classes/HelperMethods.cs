using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_MVC.Classes
{
    public static class HelperMethods
    {
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public static bool SaveFilesToFileServer(string pBasePath,List<FileModel> files)
        {
            if (!files.Any()) return false;

            try
            {
                foreach (var pFileModel in files)
                {
                    pFileModel.FilePath = pBasePath + pFileModel.FilePath;
                    var loAbsolutePath = pFileModel.FilePath + "\\" + pFileModel.FileName;
                    Directory.CreateDirectory(pFileModel.FilePath);
                    File.WriteAllBytes(loAbsolutePath, pFileModel.FileContent);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void DeleteFiles(List<AssetPhoto> files)
        {
            if (!files.Any()) return;

            try
            {
                foreach (var pFileModel in files)
                {
                    File.Delete(pFileModel.file_path);
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
