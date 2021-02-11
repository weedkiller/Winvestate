using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

        public static void NormalizeOrientation(this Image image)
        {
            if (Array.IndexOf(image.PropertyIdList, Common.ExifOrientationTagId) > -1)
            {
                int orientation;

                orientation = image.GetPropertyItem(Common.ExifOrientationTagId).Value[0];

                if (orientation >= 1 && orientation <= 8)
                {
                    switch (orientation)
                    {
                        case 2:
                            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            image.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case 5:
                            image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case 6:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            image.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case 8:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }

                    image.RemovePropertyItem(Common.ExifOrientationTagId);
                }
            }
        }

        public static void DeleteFiles(List<AssetPhoto> files,string pBasePath)
        {
            if (!files.Any()) return;

            try
            {
                foreach (var pFileModel in files)
                {
                    File.Delete(pBasePath+pFileModel.file_path);
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void MergedBitmaps(Image bmp1, Image bmp2,string pImageName)
        {
            Bitmap result = new Bitmap(Math.Max(bmp1.Width, bmp2.Width),
                Math.Max(bmp1.Height, bmp2.Height));
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp2, Point.Empty);
                g.DrawImage(bmp1, Point.Empty);
            }
            result.Save(pImageName);
        }


        public static void CombineImages(Image pImage1, Image pImage2,string pImageName)
        {
            using (pImage1)
            {
                using (var bitmap = new Bitmap(pImage1.Width, pImage1.Height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(pImage1,
                            new Rectangle(0,
                                0,
                                pImage1.Width,
                                pImage1.Height),
                            new Rectangle(0,
                                0,
                                pImage1.Width,
                                pImage1.Height),
                            GraphicsUnit.Pixel);
                        canvas.DrawImage(pImage2,
                            (bitmap.Width / 2) - (pImage2.Width / 2),
                            (bitmap.Height / 2) - (pImage2.Height / 2));
                        canvas.Save();
                    }

                    try
                    {
                        bitmap.Save(pImageName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        ;
                    }
                }
            }
        }
    }
}
