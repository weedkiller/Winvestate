using System;
using System.Collections.Generic;
using System.Text;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_Models
{
    public class AllTypes
    {
        public List<SysType> asset_category { get; set; }
        public List<SysType> portfolio_house { get; set; }
        public List<SysType> portfolio_office { get; set; }
        public List<SysType> portfolio_ground { get; set; }

    }
}
