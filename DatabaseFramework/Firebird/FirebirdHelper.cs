using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FirebirdSql.Data.FirebirdClient;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Contains Firbird common helper methods
    /// </summary>
    public static class FirebirdHelper
    {

        #region Public Methods

        /// <summary>
        /// Gets connection string with rooted path if it is not already.
        /// </summary>
        public static string GetConnectionStringWithRootedAppDataPath(string connectionString)
        {
            string finalConnectionString = connectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                string databaseName = FirebirdHelper.GetDatabaseFromConnectionString(connectionString);
                if (!Path.IsPathRooted(databaseName))
                {
                    finalConnectionString = connectionString.Replace(databaseName, RWhizzConfiguration.GetFilePathWRTAppData(databaseName));
                }
            }

            return finalConnectionString;
        }

        /// <summary>
        /// GetDatabaseFromConnectionString
        /// </summary>
        public static string GetDatabaseFromConnectionString(string connectionString)
        {
            return GetKeyValue(connectionString, "Database");
        }

        /// <summary>
        /// Specifies if connection string is for firebird server, if not it means it is for embedded.
        /// </summary>
        public static bool IsFirebirdServerConnectionString(string connectionString)
        {
            return GetKeyValue(connectionString, "ServerType").Equals("0");
        }

        /// <summary>
        /// Clear all the connections of FireBird.
        /// </summary>
        public static void ClearFirebirdConnectionPools()
        {
            FbConnection.ClearAllPools();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get key value pair.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetKeyValue(string connectionString, string key)
        {
            string value = string.Empty;
            foreach (string connectionStringPart in connectionString.Split(";".ToCharArray()))
            {
                string[] currentKeyValue = connectionStringPart.Split("=".ToCharArray());
                if (currentKeyValue[0].Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = currentKeyValue[1];
                }
            }
            return value;
        }

        #endregion
    }
}
