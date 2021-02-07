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
        public static readonly string SmsHeader = "WINVESTATE";
        public static readonly string SmsUsername = "winvestate";
        public static readonly string SmsPassword = "dac295e64ea5536c3ae2854a92ce2ad9";
        public static readonly string InfoUrl = "https://information.mesnetbilisim.com.tr/api/";
        public static readonly string CallbackUrl = "https://winvestate-api.mesnetbilisim.com.tr/api/Agreement/Signed";
        public static readonly string MespactLinkUrl = "https://panel.esozlesme.com.tr";
        public static System.Security.Principal.IIdentity MyIdentity;
        public static readonly string SenderMail = "winvestate@mesnetbilisim.com.tr";
        public static readonly string SenderPassword = "81a?4X";
        public static readonly string SenderHost = "mail.mesnetbilisim.com.tr";
        public static readonly int SenderPort = 587;
        public static string InfoMailList = "emre.akarsu@mesnetbilisim.com.tr";
        public static string OfferMail = "emre.akarsu@mesnetbilisim.com.tr";
        public static string CustomerUrl = "https://winvestate.mesnetbilisim.com.tr/";
        public static string BitlyLink = "d4bbed60e7a7786929a2f43da3f264dc8e8a6f46";

        public static readonly string MespactWinvestateUser = "54df5cfc-e986-4e1d-896b-a159e4ed5068";


        [Conditional("PROD")]
        public static void InitVariablesForProd()
        {
            Connection.ConnectionStringForMesnet = "Server=192.168.200.200;Port=5432;Database=mesnet;User Id=api_connector;Password=!Api_connector*!;";
            Connection.ConnectionStringForLog = "Server=192.168.200.200;Port=5432;Database=system_log;User Id=logger;Password=!loggerMbT*_;";
            Connection.ConnectionStringForWinvestate = "Server=192.168.200.200;Port=5432;Database=winvestate;User Id=winvestate;Password=2021Winvestate*!-;";
            InfoMailList =
                "cuneyt.kurekci@winvestate.com;zafer.karahan@winvestate.com;serkan.yilmaz@winvestate.com;metin.uzel@winvestate.com;busra.ozdemir@winvestate.com;esma.kurekci@winvestate.com";
            OfferMail = "esma.kurekci@winvestate.com";
            CustomerUrl = "https://e-teklif.winvestate.com";
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
            Connection.ConnectionStringForMesnet = "Server=192.168.200.200;Port=5432;Database=mesnet;User Id=api_connector;Password=!Api_connector*!;";
            Connection.ConnectionStringForLog = "Server=192.168.200.200;Port=5432;Database=system_log;User Id=logger;Password=!loggerMbT*_;";
            Connection.ConnectionStringForWinvestate = "Server=192.168.200.200;Port=5432;Database=winvestate;User Id=winvestate;Password=2021Winvestate*!-;";
            InfoMailList =
                "cuneyt.kurekci@winvestate.com;zafer.karahan@winvestate.com;serkan.yilmaz@winvestate.com;metin.uzel@winvestate.com;busra.ozdemir@winvestate.com;esma.kurekci@winvestate.com";
            CustomerUrl = "https://e-teklif.winvestate.com";

            //Connection.ConnectionStringForLog = "Server=192.168.200.102;Port=5432;Database=system_log;User Id=mesnet;Password=2019MsNt!!;";
            //Connection.ConnectionStringForMesnet = "Server=192.168.200.102;Port=5432;Database=mesnet;User Id=mesnet;Password=2019MsNt!!;";
            //Connection.ConnectionStringForWinvestate = "Server=192.168.200.102;Port=5432;Database=winvestate;User Id=mesnet;Password=2019MsNt!!;";
        }
    }
}
