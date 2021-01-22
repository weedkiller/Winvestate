using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models
{
    public class FileModel
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FileSubfolder { get; set; }
        public string FileFolder { get; set; }
        public string FileServerCompanyLogoFolder { get; set; }
        public string FileServerUiImagesFolder { get; set; }
    }
}
