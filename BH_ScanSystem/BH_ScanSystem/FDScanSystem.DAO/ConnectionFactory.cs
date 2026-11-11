using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO
{
    public class ConnectionFactory
    {

        private static readonly string Default_Connection_Name = "DataConnection";

        private static Dictionary<string, string> connection;

        static ConnectionFactory()
        {
            connection = new Dictionary<string, string>(5);

            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                connection[item.Name] = item.ConnectionString;
            }
        }

        public static IDbConnection Create()
        {
            return new SqlConnection(GetConnection<DBDataContext>());
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
