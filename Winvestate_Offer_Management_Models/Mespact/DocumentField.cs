using System;
using System.Collections.Generic;

namespace Winvestate_Offer_Management_Models.Mespact
{
    public class DocumentField
    {
        public int id { get; set; }

        public string document_type_row_guid { get; set; }

        public string document_field_name { get; set; }

        public int document_field_data_type_system_type_id { get; set; }

        public int document_type_version { get; set; }

        public int document_type_deleted_version { get; set; }

        public DateTime? row_create_date { get; set; }

        public DateTime? row_update_date { get; set; }

        public int? row_update_user { get; set; }

        public int? row_create_user { get; set; }

        public int version { get; set; }

        public bool document_field_is_required { get; set; }

        public string row_guid { get; set; }
        public string document_field_text_size { get; set; }
        public string document_field_font_type { get; set; }

        public DocumentFieldLocation location { get; set; }

        public List<DocumentFieldLocation> clones { get; set; }

        //public static DocumentField GetModel(DocumentFieldTemp v)
        //{
        //    return new DocumentField
        //    {
        //        id = v.id,
        //        document_type_row_guid = v.document_type_row_guid,
        //        document_field_name = v.document_field_name,
        //        document_field_data_type_system_type_id = v.document_field_data_type_system_type_id,
        //        document_type_version = v.document_type_version,
        //        document_type_deleted_version = v.document_type_deleted_version,
        //        row_guid = v.row_guid,
        //        version = v.version,
        //        row_create_user = v.row_create_user,
        //        row_create_date = v.row_create_date,
        //        row_update_user = v.row_update_user,
        //        row_update_date = v.row_update_date
        //    };
        //}
    }
}