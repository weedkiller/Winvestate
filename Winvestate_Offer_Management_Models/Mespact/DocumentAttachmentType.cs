using System;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentAttachmentType
    {
        public int id { get; set; }

        public string document_type_guid { get; set; }

        public string attachment_name { get; set; }

        public int is_active { get; set; }

        public int is_deleted { get; set; }

        public string row_guid { get; set; }

        public DateTime? row_create_date { get; set; }

        public DateTime? row_update_date { get; set; }

        public int? row_update_user { get; set; }

        public int? row_create_user { get; set; }

        public int version { get; set; }

        public string document_attachment_content { get; set; }
    }
}