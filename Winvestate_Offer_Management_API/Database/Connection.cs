using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Razor.Language;
using Npgsql;

namespace Winvestate_Offer_Management_API.Database
{
    public class Connection
    {
        public static string ConnectionStringForLog = "Server=192.168.200.102;Port=5432;Database=system_log;User Id=sys_log;Password=2020Bikurti;";
        public static string ConnectionStringForMesnet = "Server=192.168.200.102;Port=5432;Database=mesnet;User Id=mesnet;Password='2020Bikurti';";
        public static string ConnectionStringForWinvestate = "Server=192.168.200.102;Port=5432;Database=winvestate;User Id=winvestate;Password='2021!Win-vestaTE';";

        public static Func<DbConnection> ConnectionWinvestate;
        public static Func<DbConnection> ConnectionLog;
        public static Func<DbConnection> ConnectionMesnet;



        public static void PrepareDatabase()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            ConnectionWinvestate = () => new NpgsqlConnection(ConnectionStringForWinvestate);
            ConnectionLog = () => new NpgsqlConnection(ConnectionStringForLog);
            ConnectionMesnet = () => new NpgsqlConnection(ConnectionStringForMesnet);
        }

        public static bool OpenConnection(IDbConnection pDbConnection)
        {
            try
            {
                pDbConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    pDbConnection.Open();
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }

        }
    }
}
