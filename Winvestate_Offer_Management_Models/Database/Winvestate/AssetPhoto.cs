using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Winvestate_Offer_Management_Models.Database.Winvestate
{
    [Table("winvestate_asset_photos")]
    public class AssetPhoto
    {
        [Key]
        public int id { get; set; }
        public Guid asset_uuid { get; set; }
        public string file_path { get; set; }
        public int item_order { get; set; }
        public bool is_thumb { get; set; }

        [Write(false)]
        public bool is_deleted { get; set; }
        
    }
}
