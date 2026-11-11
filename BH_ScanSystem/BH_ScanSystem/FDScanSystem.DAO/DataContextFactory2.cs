using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
namespace FDScanSystem.DAO
{
    public class DataContextFactory2
    {
        private static readonly string Default_Connection_Name = "DataConnection2";

        private static Dictionary<string, string> connection;

        static DataContextFactory2()
        {
            connection = new Dictionary<string, string>(5);

            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                connection[item.Name] = item.ConnectionString;
            }
        }

        public static DB2DataContext Create()
        {

            DB2DataContext ctx = new DB2DataContext(GetConnection<DB2DataContext>());
            ctx.ExecuteCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");

            return ctx;
        }


        protected static string GetConnection<T>()
        {
            string value = string.Empty;
            if (!connection.TryGetValue(typeof(T).ToString(), out value))
            {
                return connection[Default_Connection_Name];
            }
            return value;
        }
    }
}
