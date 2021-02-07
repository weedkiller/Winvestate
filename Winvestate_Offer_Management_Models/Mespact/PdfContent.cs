using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class PdfContent
    {
        public string pdf_content { get; set; }
        public List<string> attachment_contents { get; set; }
        public string erp_id { get; set; }
        public string mespact_id { get; set; }
        public string company_logo { get; set; }
        public string company_link { get; set; }
    }
}
