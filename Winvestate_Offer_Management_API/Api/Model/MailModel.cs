using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winvestate_Offer_Management_API.Api.Model
{
    public class MailModel
    {
        public string BodyMessage { get; set; }
        public string BodyHtmlMessage { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public List<KeyValuePair<string, byte[]>> Attachments { get; set; }
        public string SenderMail { get; set; }
        public string SenderPassword { get; set; }
        public string SenderHost { get; set; }
        public int SenderPort { get; set; }
    }
}
