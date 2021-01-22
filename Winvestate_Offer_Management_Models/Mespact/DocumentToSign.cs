using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentToSign
    {
        public int id { get; set; }
        public string participant_guid { get; set; }
        public string erp_id { get; set; }
        public string participant_contractor_row_guid { get; set; }
        public string participant_user_guid { get; set; }
        public int document_to_sign_status_system_type_id { get; set; }
        public DateTime? row_create_date { get; set; }
        public DateTime? row_update_date { get; set; }
        public DateTime? deadline_time { get; set; }
        public int? row_update_user { get; set; }
        public int? row_create_user { get; set; }
        public int version { get; set; }
        public int is_deleted { get; set; }
        public string row_guid { get; set; }
        public string document_type_guid { get; set; }
        public string send_to_sign_message { get; set; }


        public string document_template_key { get; set; }

        public string participant_phone { get; set; }

        public string participant_callback_url { get; set; }


        public string document_content { get; set; }


        public string signer_name { get; set; }


        public string document_type_name { get; set; }


        public string sender_name { get; set; }


        public string sent_date { get; set; }


        public List<DocumentAttachmentType> document_attachments { get; set; }

    }
}
