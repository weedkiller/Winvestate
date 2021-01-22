using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    public class Entity
    {
        [Key]
        public int id { get; set; }
        public Guid row_guid { get; set; }
        public DateTime? row_create_date { get; set; }
        public DateTime? row_update_date { get; set; }
        public DateTime? row_delete_date { get; set; }
        public Guid? row_create_user { get; set; }
        public Guid? row_update_user { get; set; }
        public Guid? row_delete_user { get; set; }
        public bool? is_deleted { get; set; }
        public bool? is_active { get; set; }
    }
}
