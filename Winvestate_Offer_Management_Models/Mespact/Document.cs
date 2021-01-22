using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentType
    {
        public int id { get; set; }

        public string document_name { get; set; }
        public int document_page_count { get; set; }

        public string row_guid { get; set; }

     
        public List<DocumentField> document_type_fields { get; set; }

        public List<DocumentAttachmentType> document_attachments { get; set; }

        public string document_content { get; set; }

        public int is_shared { get; set; }

        public bool? from_api { get; set; }

        public string document_extension { get; set; }

        public string message { get; set; }
    }
}
