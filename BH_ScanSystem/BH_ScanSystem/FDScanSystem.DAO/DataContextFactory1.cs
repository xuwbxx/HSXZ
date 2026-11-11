using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO
{
    public class DataContextFactory1
    {
        private static readonly string Default_Connection_Name = "DataConnection1";

        private static Dictionary<string, string> connection;

        static DataContextFactory1()
        {
            connection = new Dictionary<string, string>(5);

            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                connection[item.Name] = item.ConnectionString;
            }
        }

        public static DB1DataContext Create()
        {

            DB1DataContext ctx = new DB1DataContext(GetConnection<DB1DataContext>());
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
