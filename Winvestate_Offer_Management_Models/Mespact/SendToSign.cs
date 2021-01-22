using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class SendToSign
    {
        public DocumentToSign document_to_sign { get; set; }

        public List<DocumentSignFlow> document_sign_flows { get; set; }

        public List<DocumentTemplate> document_templates { get; set; }
    }
}
