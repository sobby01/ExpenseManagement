using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Container of SQL Server specific helper methods
    /// </summary>
    internal static class SQLServerHelper
    {
        /// <summary>
        /// Gets ConnectionString without DB Name.
        /// </summary>
        /// <returns></returns>
        public static string GetDBConnectionStringWithoutDBName(string connectionString)
        {
            StringBuilder strConnStringWithoutDBName = new StringBuilder();
            string strConnParameter = string.Empty;

            // Store the individual connection parameters in the array
            string[] strConnParameters = connectionString.Split(';');
            for (int iPosition = 0; iPosition < strConnParameters.Length; iPosition++)
            {
                strConnParameter = strConnParameters[iPosition];
                // Exclude the database name parameter from the database connection string
                if (!strConnParameter.Trim().ToLower().Contains("database"))
                {
                    strConnStringWithoutDBName.Append(strConnParameter + ";");
                }
            }
            // Return the connection string without database name
            return strConnStringWithoutDBName.ToString().Substring(0, strConnStringWithoutDBName.Length - 1);
        }

        /// <summary>
        /// Applies given timeout to the connection string
        /// </summary>
        public static string GetDBConnectionStringWithTimeOut(string connectionString, int connectionTimeOut)
        {
            StringBuilder resultConnectionString = new StringBuilder();
            string strConnParameter = string.Empty;

            foreach (string keyValue in connectionString.Split(';'))
            {
                if (!keyValue.Trim().ToLower().Contains("connection timeout"))
                {
                    resultConnectionString.Append(keyValue + ";");
                }
            }

            resultConnectionString.Append(string.Format("Connection Timeout={0}", connectionTimeOut));

            return resultConnectionString.ToString();
        }
    }
}
