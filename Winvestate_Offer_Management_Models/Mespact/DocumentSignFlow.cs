using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentSignFlow
    {
        public int id { get; set; }
        public string row_guid { get; set; }

        public string participant_ip_address { get; set; }
        //public string participant_guid { get; set; }

        public string participant_hash { get; set; }


        public string participant_name { get; set; }


        public string participant_full_name { get; set; }


        public string participant_guid { get; set; }


        public string company_tax_office { get; set; }


        public string company_name { get; set; }


        public string participant_surname { get; set; }


        public string identity_number { get; set; }


        public string mail { get; set; }


        public string document_template_key { get; set; }


        public string sign_image { get; set; }


        public string assigned_participant_hash { get; set; }


        public string phone_number { get; set; }

        public string document_to_sign_guid { get; set; }

        public int sign_status_type_system_type_id { get; set; }

        public DateTime? row_create_date { get; set; }

        public DateTime? row_update_date { get; set; }

        public int? row_update_user { get; set; }

        public int? row_create_user { get; set; }

        public int review_count { get; set; }

        public int version { get; set; }

        public byte is_deleted { get; set; }

        public int order { get; set; }


        public int user_type_system_type_id { get; set; }


        public string sign_link { get; set; }
    }
}
