using System;
using System.Collections.Generic;
using System.Text;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentTemplate
    {
        public int id { get; set; }
        public string document_to_sign_guid { get; set; }
        public string document_template_key { get; set; }
        public string document_template_value { get; set; }
        public DateTime? row_create_date { get; set; }
        public DateTime? row_update_date { get; set; }
        public int? row_update_user { get; set; }
        public int? row_create_user { get; set; }
        public int version { get; set; }
        public string row_guid { get; set; }
        public string assigned_participant_hash { get; set; }
        public bool saved { get; set; }
        public string participant_name { get; set; }
    }
}
