using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_MVC.Models
{
    public class ViewModelBase
    {
        public UserDto User { get; set; }
        public AssetDto Asset{ get; set; }
        public List<AssetPhoto> AssetPhotos{ get; set; }
        public List<OfferDto> Offers{ get; set; }
        public string Token { get; set; }
        public string AssetExplanation { get; set; }
    }
}
