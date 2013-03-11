using System;
using System.Data;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Top level data access class. All the data access goes through this class.
    /// </summary>
    public static class DatabaseAccessManager
    {
        #region Properties

        /// <summary>
        /// Returns the number of currently open database connections.  This is
        /// incremented each time a new SQLServerDBProvider is created and decremented when
        /// the DBProvider is disposed.  Should only be used as a rough guide.  
        /// </summary>
        public static int NumberOfCurrentlyOpenConnections
        {
            get
            {
                int count = 0;
                switch (RWhizzConfiguration.DatabaseStorageType)
                {
                    case DataStorageType.Firebird:
                        count = FirebirdDBProvider.NumberOfCurrentlyOpenConnections;
                        break;
                        //FOr Sql Server
                    case DataStorageType.SQLServer:
                         count = SQLServerDBProvider.NumberOfCurrentlyOpenConnections;
                         break;
                }
                return count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// NewProjectProvider
        /// </summary>
        public static IDBProvider NewDatabaseProvider(string storedProcedure)
        {
            IDBProvider provider = null;
            switch (RWhizzConfiguration.DatabaseStorageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewProjectProvider(storedProcedure);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewProjectProvider(storedProcedure);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// Returns Database Provider for given connectionString and StoredProcedure
        /// </summary>
        public static IDBProvider NewDatabaseProvider(string storedProcedure, string connectionString)
        {
            return NewProvider(storedProcedure, connectionString, RWhizzConfiguration.DatabaseStorageType);
        }

        /// <summary>
        /// NewRuntimeProjectProvider
        /// </summary>
        public static IDBProvider NewProvider(string storedProcedure, string connectionString, DataStorageType storageType)
        {
            IDBProvider provider = null;
            switch (storageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewProvider(storedProcedure, connectionString);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewProvider(storedProcedure, connectionString);
                    break;
            }
            return provider;
        }


        /// <summary>
        /// NewSQLTextProvider
        /// </summary>
        public static IDBProvider NewSQLTextProvider(string sqlText, string connectionString, DataStorageType storageType)
        {
            IDBProvider provider = null;
            switch (storageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewProvider(sqlText, connectionString, CommandType.Text);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewProvider(sqlText, connectionString, CommandType.Text);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// NewDataReader
        /// </summary>
        public static IWhizzDataReader NewDataReader(IDataReader reader, string name, DataStorageType storageType)
        {
            IWhizzDataReader dataReader = null;
            switch (storageType)
            {
                case DataStorageType.Firebird:
                    dataReader = new FirebirdDataReader(reader, name);
                    break;
                case DataStorageType.SQLServer:
                    dataReader = new SQLServerDataReader(reader, name);
                    break;
            }
            return dataReader;
        }


        /// <summary>
        /// NewDatabaseDataSetProvider
        /// </summary>
        public static IDBProvider NewDatabaseDataSetProvider(string deleteStoredProcedureName, string insertStoredProcedureName, string selectStoredProcedureName, string updateStoredProcedureName)
        {
            IDBProvider provider = null;
            switch (RWhizzConfiguration.DatabaseStorageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewDatabaseDataSetProvider(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewDatabaseDataSetProvider(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// NewDatabaseDataSetProviderWithTransaction
        /// </summary>
        public static IDBProvider NewDatabaseDataSetProviderWithTransaction(string deleteStoredProcedureName, string insertStoredProcedureName, string selectStoredProcedureName, string updateStoredProcedureName)
        {
            IDBProvider provider = null;
            switch (RWhizzConfiguration.DatabaseStorageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewDatabaseDataSetProviderWithTransaction(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewDatabaseDataSetProviderWithTransaction(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// NewDataSetProvider
        /// </summary>
        public static IDBProvider NewDataSetProvider(string deleteStoredProcedureName, string insertStoredProcedureName, string selectStoredProcedureName, string updateStoredProcedureName, string connectionString, DataStorageType storageType)
        {
            IDBProvider provider = null;
            switch (storageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewDataSetProvider(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName, connectionString, false);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewDataSetProvider(deleteStoredProcedureName, insertStoredProcedureName, selectStoredProcedureName, updateStoredProcedureName, connectionString);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// NewProjectProvider
        /// </summary>
        public static IDBProvider NewDatabaseProvider(DataStorageType dataStorageType, CommandType cmdType)
        {
            IDBProvider provider = null;
            switch (dataStorageType)
            {
                case DataStorageType.Firebird:
                    provider = FirebirdDBProvider.NewProjectProvider(cmdType);
                    break;
                case DataStorageType.SQLServer:
                    provider = SQLServerDBProvider.NewProjectProvider(cmdType);
                    break;
            }
            return provider;
        }

        /// <summary>
        /// Starts a global transaction and returns its reference. THIS SHOULD ALWAYS BE USED IN USING CLAUSE.
        /// </summary>
        public static GlobalTransaction StartGlobalTransaction()
        {
            return GlobalTransaction.BeginGlobalTransaction();
        }

        #endregion
    }
}
