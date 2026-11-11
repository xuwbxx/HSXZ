using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Interactive.Utility;

namespace DataEntity
{
    public class DataContextFactory
    {
        private static readonly string Default_Connection_Name = "TH_Config";

        private static Dictionary<string, string> connection;

        static DataContextFactory()
        {
            connection = new Dictionary<string, string>(5);

            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                connection[item.Name] = item.ConnectionString;
            }
        }

        public static DBDataContext Create()
        {

            DBDataContext ctx = new DBDataContext(GetConnection<DBDataContext>());
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
