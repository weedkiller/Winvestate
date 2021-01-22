using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Winvestate_Offer_Management_API.Database;

namespace Winvestate_Offer_Management_API.Classes
{
    public class Common
    {
        public static readonly string SmsHeader = "MESPACT";
        public static readonly string SmsUsername = "mesnetbilisim";
        public static readonly string SmsPassword = "c74faf2e578639a06e74b4c41f3f8e29";
        public static readonly string InfoUrl = "https://information.mesnetbilisim.com.tr/api/";
        public static readonly string CallbackUrl = "https://winvestate-api.mesnetbilisim.com.tr/api/Agreement/Signed";
        public static readonly string MespactLinkUrl = "https://panel.esozlesme.com.tr";
        public static System.Security.Principal.IIdentity MyIdentity;

        public static readonly string MespactWinvestateUser = "54df5cfc-e986-4e1d-896b-a159e4ed5068";


        [Conditional("PROD")]
        public static void InitVariablesForProd()
        {
            Connection.ConnectionStringForLog = "Server=192.168.200.200;Port=5432;Database=sys_log;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForMesnet = "Server=192.168.200.200;Port=5432;Database=sys_log;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForWinvestate = "Server=192.168.200.200;Port=5432;Database=sys_log;User Id=mesnet;Password=2019MsNt!!;";
        }

        [Conditional("TEST")]
        public static void InitVariablesForTest()
        {
            Connection.ConnectionStringForLog = "Server=192.168.200.102;Port=5432;Database=system_log;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForMesnet = "Server=192.168.200.102;Port=5432;Database=mesnet;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForWinvestate = "Server=192.168.200.102;Port=5432;Database=winvestate;User Id=mesnet;Password=2019MsNt!!;";
        }

        public static void InitVariablesForDebug()
        {
            Connection.ConnectionStringForLog = "Server=192.168.200.102;Port=5432;Database=system_log;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForMesnet = "Server=192.168.200.102;Port=5432;Database=mesnet;User Id=mesnet;Password=2019MsNt!!;";
            Connection.ConnectionStringForWinvestate = "Server=192.168.200.102;Port=5432;Database=winvestate;User Id=mesnet;Password=2019MsNt!!;";
        }
    }
}
