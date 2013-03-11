using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// SQL Server DB Provider
    /// </summary>
    internal class SQLServerDBProvider : IDBProvider 
    {
        #region Fields and Constants

        private SqlConnection connection;
        private SqlCommand command;
        private IWhizzDataReader dataReader;
        private bool isDisposed;
        private SqlDataAdapter dataAdapter;
        private SqlTransaction sqlTransaction;
        private static int numberOfCurrentlyOpenConnections;
        
        #endregion

        #region Constructors / Finalizer


        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerDBProvider()
        {

        }

        /// <summary>
        /// Required since we want to make sure connections are cleaned 
        /// up correctly if the caller does not call Dispose.
        /// </summary>
        ~SQLServerDBProvider()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Command timeout to be used with this provider.
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                CheckDisposedState();

                return this.Command.CommandTimeout;
            }
            set
            {
                CheckDisposedState();

                this.Command.CommandTimeout = value;
            }
        }

        /// <summary>
        /// Returns the number of currently open database connections.  This is
        /// incremented each time a new SQLServerDBProvider is created and decremented when
        /// the SQLServerDBProvider is disposed.  Should only be used as a rough guide.  
        /// </summary>
        public static int NumberOfCurrentlyOpenConnections
        {
            get
            {
                return SQLServerDBProvider.numberOfCurrentlyOpenConnections;
            }
        }

        /// <summary>
        /// Returns whether the system can communicate with SQL
        /// You must handle the exceptions from this function in your code
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Lutron.I18N", "LI18N009:DoNotUseHardcodedStrings")]
        public static bool IsServerAvailable
        {
            get
            {
                bool availibiltyStatus = true;
                SqlConnection connection = new SqlConnection(RWhizzConfiguration.NonDatabaseSpecificConnectionString);
                //close if already open, connection pool status
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                //attempt to open the connection
                try
                {
                    connection.Open();
                    //fire a dummy query
                    //we need to fire this because the SqlConnection.Open succeeds even if one kills the service. courtesy connection pool.
                    SqlCommand command = new SqlCommand("USE master");
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
                catch (SqlException exs)
                {
                    availibiltyStatus = false;
                    if (exs.Number == 212)
                    {
                        throw new Exception(exs.Message);
                    }
                }
                //close the connection now
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                return availibiltyStatus;

            }
        }
        /// <summary>
        /// Database Connection Object
        /// </summary>
        private SqlConnection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }

        /// <summary>
        /// Database Command Object
        /// </summary>
        private SqlCommand Command
        {
            get
            {
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        /// <summary>
        /// DataAdapater Object
        /// </summary>
        private SqlDataAdapter DataAdapater
        {
            get
            {
                return this.dataAdapter;
            }
            set
            {
                this.dataAdapter = value;
            }
        }

        /// <summary>
        /// DataReader Object
        /// </summary>
        private IWhizzDataReader DataReader
        {
            get
            {
                return this.dataReader;
            }
            set
            {
                this.dataReader = value;
            }
        }

        #endregion

        #region Parameter Get/Set methods

        /// <summary>
        /// Adds a new boolean parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, bool parameterValue)
        {
            Add(storedProcedureParameterName, (bool?)parameterValue);
        }

        /// <summary>
        /// Adds a new boolean parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, bool? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }


        /// <summary>
        /// Adds a new byte parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, byte parameterValue)
        {
            this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);

        }

        /// <summary>
        /// Adds a new UInt16 parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, UInt16? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new UInt32 parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, UInt32? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void Add(string storedProcedureParameterName, Int32? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new UInt64 parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, UInt64? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new Int64 parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, Int64? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new string parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, string parameterValue)
        {
            this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
        }

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure.</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, DateTime parameterValue)
        {
            if (parameterValue > DateTime.MinValue)
            {
                if (parameterValue > (DateTime)SqlDateTime.MinValue)
                {
                    this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
                }
                else
                {
                    this.Command.Parameters.AddWithValue(storedProcedureParameterName, SqlDateTime.MinValue);
                }
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure.</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        public void Add(string storedProcedureParameterName, DateTime? parameterValue)
        {

            if (parameterValue.HasValue)
            {
                if (parameterValue > DateTime.MinValue)
                {
                    if (parameterValue > (DateTime)SqlDateTime.MinValue)
                    {
                        this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
                    }
                    else
                    {
                        this.Command.Parameters.AddWithValue(storedProcedureParameterName, SqlDateTime.MinValue);
                    }
                }
                else
                {
                    this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
                }
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a parameter to the relevant command(s)' parameter list.
        /// </summary>
        /// <param name="commandTypeToAddTo"></param>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="paramValue"></param>
        public void Add(DBCommandTypes commandTypeToAddTo, string storedProcedureParameterName, object paramValue)
        {
            if (this.dataAdapter != null)
            {
                if ((commandTypeToAddTo & DBCommandTypes.Select) != 0)
                {
                    this.dataAdapter.SelectCommand.Parameters.AddWithValue(storedProcedureParameterName, paramValue);
                }
                if ((commandTypeToAddTo & DBCommandTypes.Delete) != 0)
                {
                    this.dataAdapter.DeleteCommand.Parameters.AddWithValue(storedProcedureParameterName, paramValue);
                }
                if ((commandTypeToAddTo & DBCommandTypes.Insert) != 0)
                {
                    this.dataAdapter.InsertCommand.Parameters.AddWithValue(storedProcedureParameterName, paramValue);
                }
                if ((commandTypeToAddTo & DBCommandTypes.Update) != 0)
                {
                    this.dataAdapter.UpdateCommand.Parameters.AddWithValue(storedProcedureParameterName, paramValue);
                }
            }
        }

        /// <summary>
        /// Pseudo-Overload of Add for double Type
        /// elsewise clashes with Int32?
        /// Adds a new double parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void AddDouble(string storedProcedureParameterName, double parameterValue)
        {
            this.Command.Parameters.AddWithValue(storedProcedureParameterName, parameterValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void Add(string storedProcedureParameterName, Image parameterValue)
        {

            SqlParameter parameter = new SqlParameter(storedProcedureParameterName, SqlDbType.Image);
            if (parameterValue != null)
            {
                MemoryStream imageStream = new MemoryStream();
                parameterValue.Save(imageStream, ImageFormat.Bmp);
                byte[] imageBytes = imageStream.ToArray();
                imageStream.Close();
                parameter.Value = imageBytes;
                parameter.Size = imageBytes.Length;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
            this.Command.Parameters.Add(parameter);

        }

        /// <summary>
        /// Adds output parameter of type int
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        public void AddOutInt(string storedProcedureParameterName)
        {
            SqlParameter parameter = this.Command.Parameters.Add(storedProcedureParameterName, SqlDbType.Int);
            parameter.Direction = ParameterDirection.Output;
        }

        /// <summary>
        /// Get parameter value for given name
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <returns></returns>
        public object GetParameterValue(string storedProcedureParameterName)
        {
            return this.Command.Parameters[storedProcedureParameterName].Value;
        }

        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void SetParameterValue(string storedProcedureParameterName, DateTime parameterValue)
        {
            if (this.Command.Parameters[storedProcedureParameterName] != null)
            {
                this.Command.Parameters[storedProcedureParameterName].Value = parameterValue;
            }
        }

        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void SetParameterValue(string storedProcedureParameterName, uint parameterValue)
        {
            if (this.Command.Parameters[storedProcedureParameterName] != null)
            {
                this.Command.Parameters[storedProcedureParameterName].Value = parameterValue;
            }
        }

        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        public void SetParameterValue(string storedProcedureParameterName, long parameterValue)
        {
            if (this.Command.Parameters[storedProcedureParameterName] != null)
            {
                this.Command.Parameters[storedProcedureParameterName].Value = parameterValue;
            }
        }

        /// <summary>
        /// Adds the passed in dataset column as a paramater to the indicated
        /// dataset commands
        /// </summary>
        /// <remarks>If the datatype of the column in string then be sure to set the MaxLength
        /// field of the column so that the SQL statements will work correctly</remarks>
        /// <param name="dataColumn">Dataset column to add the paramater to</param>
        /// <param name="storedProcedureParameterName">Name of the paramater in the stored procedures</param>
        /// <param name="Enums.DBCommandTypesToBeAddedTo">Which of the possible commands to add the paramater to</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Lutron.I18N", "LI18N009:DoNotUseHardcodedStrings")]
        public void AddColumnToDataSetCommands(DataColumn dataColumn,
                                               string storedProcedureParameterName,
                                               DBCommandTypes dBCommandTypesToBeAddedTo)
        {
            if (dataColumn == null)
            {
                // The data column must be specified
                throw new Exception("DataColumn can not be null");
            }

            if (string.IsNullOrEmpty(storedProcedureParameterName))
            {
                // The stored procedure paramater name must be specified
                throw new Exception("storedProcedureParameterName must be specified");
            }

            if (dBCommandTypesToBeAddedTo == DBCommandTypes.None)
            {
                // There are no command types specified to add to so there is no
                // work to be done, just return
                return;
            }

            SqlDbType dbType = SqlDbType.Int;
            int dbSize = 0;
            if (dataColumn.DataType == typeof(bool))
            {
                dbType = SqlDbType.Bit;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(UInt32))
            {
                dbType = SqlDbType.Int;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(decimal))
            {
                dbType = SqlDbType.Decimal;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(string))
            {
                dbType = SqlDbType.NVarChar;
                dbSize = dataColumn.MaxLength;
            }

            foreach (DBCommandTypes dataSetCommandType in Enum.GetValues(typeof(DBCommandTypes)))
            {
                if ((dataSetCommandType & dBCommandTypesToBeAddedTo) > 0)
                {
                    SqlParameter dbParamater;
                    if (dbSize == 0)
                    {
                        dbParamater = new SqlParameter(storedProcedureParameterName, dbType);
                    }
                    else
                    {
                        dbParamater = new SqlParameter(storedProcedureParameterName, dbType, dbSize);
                    }
                    dbParamater.SourceColumn = dataColumn.ColumnName;

                    switch (dataSetCommandType)
                    {
                        case DBCommandTypes.Delete:
                            this.DataAdapater.DeleteCommand.Parameters.Add(dbParamater);
                            break;
                        case DBCommandTypes.Insert:
                            this.DataAdapater.InsertCommand.Parameters.Add(dbParamater);
                            break;
                        case DBCommandTypes.Select:
                            this.DataAdapater.SelectCommand.Parameters.Add(dbParamater);
                            break;
                        case DBCommandTypes.Update:
                            this.DataAdapater.UpdateCommand.Parameters.Add(dbParamater);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// AddWithValue
        /// </summary>
        public void AddWithValue(string parameterName, object value)
        {
            this.Command.Parameters.AddWithValue(parameterName, value);
        }

        #endregion

        #region Factory Methods

        #region New provider methods

        /// <summary>
        /// Creates a new SQLServerSQLServerDBProvider that is connected to the Gulliver Project
        /// database.
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure 
        /// to execute.</param>
        /// <returns></returns>
        internal static SQLServerDBProvider NewProjectProvider(string storedProcedure)
        {
            return NewProvider(storedProcedure, RWhizzConfiguration.DatabaseConnectionString);
        }

        /// <summary>
        /// Creates a new SQLServerDBProvider that is connected to the project database.
        /// </summary>
        public static SQLServerDBProvider NewProjectProvider(CommandType cmdType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new SQLServerDBProvider which talks to the database specified in
        /// the conectionString parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static SQLServerDBProvider NewProvider(string sqlText, string connectionString, CommandType commandType)
        {
            SQLServerDBProvider provider = null;
            bool tryAgain = false;
            try
            {
                // 1st try...
                provider = NewProvider_Internal(sqlText, connectionString, commandType);
            }
            catch
            {
                // Oops, some error happened... let's try again before we give up.
                tryAgain = true;
            }
            if (tryAgain)
            {
                // Not sure how far things may have gone... just to be sure, dispose any provider object that
                // may have been created in the 1st unsuccessful attempt.
                if (provider != null)
                {
                    provider.Dispose();
                    provider = null;
                }
                // Try again the second time...
                provider = NewProvider_Internal(sqlText, connectionString, commandType);
            }

            // Return what we have (if there was an error even the second time, we wouldn't even get here).
            return provider;
        }

        /// <summary>
        /// Creates a new SQLServerDBProvider which talks to the database specified in
        /// the conectionString parameter.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="connectionString">The connection string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static SQLServerDBProvider NewProvider(string storedProcedureName, string connectionString)
        {
            return NewProvider(storedProcedureName, connectionString, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Creates a new SQLServerDBProvider which talks to the database specified in
        /// the conectionString parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        private static SQLServerDBProvider NewProvider_Internal(string commandText, string connectionString, CommandType commandType)
        {
            SQLServerDBProvider provider = new SQLServerDBProvider();
            try
            {

                provider.Connection = new SqlConnection(connectionString);

                provider.Connection.Open();

                //Increment our counter for the number of open connections
                SQLServerDBProvider.numberOfCurrentlyOpenConnections++;

                provider.Command = provider.Connection.CreateCommand();
                provider.Command.CommandType = commandType;
                provider.Command.CommandText = commandText;
            }
            catch (ArgumentException e)
            {
                throw (new Exception(e.Message));
            }
            catch (SqlException e)
            {
                throw (new Exception(e.Message));
            }
            return provider;

        }
        

        /// <summary>
        /// Creates a new SQLServerDBProvider which talks to the database whose name is specified in the 
        /// databaseName parameter.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="databaseName">The name of the Database.</param>
        public static SQLServerDBProvider NewProvider(string storedProcedureName)
        {
            SQLServerDBProvider dbProvider = NewProjectProvider(storedProcedureName);
            return dbProvider;
        }

        #endregion

        #region New dataset provider methods

        /// <summary>
        /// Creates a new SQLServerDBProvider that is connected to the Gulliver Project and set up for a dataset
        /// database.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <returns></returns>
        public static SQLServerDBProvider NewDatabaseDataSetProvider(string deleteStoredProcedureName,
                                                           string insertStoredProcedureName,
                                                           string selectStoredProcedureName,
                                                           string updateStoredProcedureName)
        {
            return NewDataSetProvider(deleteStoredProcedureName,
                                      insertStoredProcedureName,
                                      selectStoredProcedureName,
                                      updateStoredProcedureName,
                                      RWhizzConfiguration.DatabaseConnectionString);
        }

        /// <summary>
        /// Creates a new DbProvider which talks to the database specified in
        /// the conectionString parameter and is specified for a dataset.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <param name="connectionString">The connection string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public static SQLServerDBProvider NewDataSetProvider(string deleteStoredProcedureName,
                                                    string insertStoredProcedureName,
                                                    string selectStoredProcedureName,
                                                    string updateStoredProcedureName,
                                                    string connectionString)
        {
            // Try to create the db provider... if it fails, we will try again one time to
            // overcome the .NET connection pool bug that sometimes happens.
            SQLServerDBProvider provider = null;
            bool tryAgain = false;
            try
            {
                // 1st try...
                provider = NewDataSetProvider_Internal(
                    deleteStoredProcedureName,
                    insertStoredProcedureName,
                    selectStoredProcedureName,
                    updateStoredProcedureName,
                    connectionString, false);
            }
            catch
            {
                // Oops, some error happened... let's try again before we give up.
                tryAgain = true;
            }
            // Should we try again?
            if (tryAgain)
            {
                // Not sure how far things may have gone... just to be sure, dispose any provider object that
                // may have been created in the 1st unsuccessful attempt.
                if (provider != null)
                {
                    provider.Dispose();
                    provider = null;
                }
                // Try again the second time...
                provider = NewDataSetProvider_Internal(
                    deleteStoredProcedureName,
                    insertStoredProcedureName,
                    selectStoredProcedureName,
                    updateStoredProcedureName,
                    connectionString, false);
            }

            // Return what we have (if there was an error even the second time, we wouldn't even get here).
            return provider;
        }

        /// <summary>
        /// Creates a new SQLServerDBProvider that is connected to the Gulliver Project and set up for a dataset
        /// database.
        /// Mehtod is adding to improve the performacne in Post transfer steps. Its anly supported for SaveDataTable Method for now.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <returns></returns>
        public static SQLServerDBProvider NewDatabaseDataSetProviderWithTransaction(string deleteStoredProcedureName,
                                                           string insertStoredProcedureName,
                                                           string selectStoredProcedureName,
                                                           string updateStoredProcedureName)
        {
            return NewDataSetProviderWithTransaction(deleteStoredProcedureName,
                                      insertStoredProcedureName,
                                      selectStoredProcedureName,
                                      updateStoredProcedureName,
                                      RWhizzConfiguration.DatabaseConnectionString);
        }

        /// <summary>
        /// Creates a new DbProvider which talks to the database specified in
        /// the conectionString parameter and is specified for a dataset.
        /// Mehtod is adding to improve the performacne in Post transfer steps. Its anly supported for SaveDataTable Method for now.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <param name="connectionString">The connection string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public static SQLServerDBProvider NewDataSetProviderWithTransaction(string deleteStoredProcedureName,
                                                    string insertStoredProcedureName,
                                                    string selectStoredProcedureName,
                                                    string updateStoredProcedureName,
                                                    string connectionString)
        {
            // Try to create the db provider... if it fails, we will try again one time to
            // overcome the .NET connection pool bug that sometimes happens.
            SQLServerDBProvider provider = null;
            bool tryAgain = false;
            try
            {
                // 1st try...
                provider = NewDataSetProvider_Internal(
                    deleteStoredProcedureName,
                    insertStoredProcedureName,
                    selectStoredProcedureName,
                    updateStoredProcedureName,
                    connectionString, true);
            }
            catch
            {
                // Oops, some error happened... let's try again before we give up.
                tryAgain = true;
            }
            // Should we try again?
            if (tryAgain)
            {
                // Not sure how far things may have gone... just to be sure, dispose any provider object that
                // may have been created in the 1st unsuccessful attempt.
                if (provider != null)
                {
                    provider.Dispose();
                    provider = null;
                }
                // Try again the second time...
                provider = NewDataSetProvider_Internal(
                    deleteStoredProcedureName,
                    insertStoredProcedureName,
                    selectStoredProcedureName,
                    updateStoredProcedureName,
                    connectionString, false);
            }

            // Return what we have (if there was an error even the second time, we wouldn't even get here).
            return provider;
        }

        /// <summary>
        /// Creates a new DbProvider which talks to the database specified in
        /// the conectionString parameter and is specified for a dataset.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="withTransaction">Setting this as true will begin the transaction and associate with the command.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        private static SQLServerDBProvider NewDataSetProvider_Internal(string deleteStoredProcedureName,
                                                    string insertStoredProcedureName,
                                                    string selectStoredProcedureName,
                                                    string updateStoredProcedureName,
                                                    string connectionString, bool withTransaction)
        {
            SQLServerDBProvider provider = new SQLServerDBProvider();
            try
            {
                provider.Connection = new SqlConnection(connectionString);
                provider.Connection.Open();

                // Increment our counter for the number of open connections
                SQLServerDBProvider.numberOfCurrentlyOpenConnections++;

                SqlCommand command = provider.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = selectStoredProcedureName;

                provider.DataAdapater = new SqlDataAdapter(command);

                command = provider.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = deleteStoredProcedureName;
                provider.DataAdapater.DeleteCommand = command;

                command = provider.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = insertStoredProcedureName;
                provider.DataAdapater.InsertCommand = command;

                command = provider.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = updateStoredProcedureName;
                provider.DataAdapater.UpdateCommand = command;

                if (withTransaction)
                {
                    command.Transaction = provider.sqlTransaction = provider.Connection.BeginTransaction();
                    provider.DataAdapater.DeleteCommand.Transaction = provider.sqlTransaction;
                    provider.DataAdapater.InsertCommand.Transaction = provider.sqlTransaction;
                    provider.DataAdapater.UpdateCommand.Transaction = provider.sqlTransaction;
                }
            }
            catch (ArgumentException e)
            {
                throw (new Exception(e.Message));
            }
            catch (SqlException e)
            {
                throw (new Exception(e.Message));
            }
            return provider;

        }
        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates new connection with the given connection string.
        /// </summary>
        public IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Returns a Dataset populated with the results of executing the specified storedProcedure on the database specified in
        /// the conectionString parameter.
        /// </summary>
        /// <param name="tableName">Name of the table in which the results are stored</param>
        public DataSet GetDataSet(string tableName)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(this.Command))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, tableName);
                return dataSet;
            }
        }


        /// <summary>
        /// Checks if the data reader object of this SQLServerDBProvider is disposed and, if not, closes it and disposes it.
        /// This is needed for code that plans to execute a reader more than once on the same provider object, as
        /// this SQLServerDBProvider only allows one instance of a reader to exist at a time.
        /// </summary>
        public void CloseDataReader()
        {
            if (this.DataReader != null)
            {
                this.DataReader.Dispose();
                this.DataReader = null;
            }
        }

        /// <summary>
        /// Executes the current command and returns a SafeDataReader 
        /// so the caller can iterate through the returned data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public IWhizzDataReader ExecuteReader()
        {
            CheckDisposedState();

            if (this.DataReader != null)
            {
                throw new Exception("Datareader must be null after disposing");
            }

            SqlDataReader reader;

            try
            {
                // Execute the reader...
                reader = this.Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            this.DataReader = new SQLServerDataReader(reader, this.connection.Database);

            // Return...
            return this.DataReader;
        }

        /// <summary>
        /// Executes the current command and returns a SafeDataReader 
        /// so the caller can iterate through the returned data.
        /// 
        /// Specifies a timeout value, in seconds...
        /// </summary>
        /// <param name="timeOut">Timeout value to wait in seconds...</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public IWhizzDataReader ExecuteReader(int timeOut)
        {
            CheckDisposedState();

            if (this.DataReader != null)
            {
                throw new Exception("Datareader must be null after disposing");
            }

            this.Command.CommandTimeout = timeOut;

            SqlDataReader reader;

            try
            {
                reader = this.Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            this.DataReader = new SQLServerDataReader(reader, this.connection.Database);

            return this.DataReader;
        }

        /// <summary>
        /// ExecuteNonQuery against the database
        /// </summary>
        public void ExecuteNonQuery()
        {
            CheckDisposedState();

            try
            {
                this.Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ExecuteNonQuery against the database with timeout
        /// </summary>
        public void ExecuteNonQuery(int timeOutInSeconds)
        {
            CheckDisposedState();
            int oldTimeOut = this.command.CommandTimeout;
            this.command.CommandTimeout = timeOutInSeconds;
            try
            {
                this.Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            this.command.CommandTimeout = oldTimeOut;
        }


        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>Int64</returns>
        public Int64 ExecuteInt64Scalar()
        {
            CheckDisposedState();
            try
            {
                return (Int64)this.Command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>Int32</returns>
        public Int32 ExecuteInt32Scalar()
        {
            CheckDisposedState();

            try
            {
                return Convert.ToInt32(this.Command.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        public object ExecuteScalar()
        {
            CheckDisposedState();

            try
            {
                return this.Command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>DateTime</returns>
        public DateTime ExecuteDateTimeScalar()
        {
            CheckDisposedState();

            try
            {
                return (DateTime)this.Command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Checks to see if SQL Server is available with the given timeout.
        /// </summary>
        public static bool CheckIsServerAvailable(int timeOut)
        {
            bool isAvalable = true;
            SqlConnection connection = new SqlConnection(SQLServerHelper.GetDBConnectionStringWithTimeOut(RWhizzConfiguration.NonDatabaseSpecificConnectionString,timeOut));
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("USE master");
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            catch
            {
                isAvalable = false;
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            return isAvalable;
        }

        /// <summary>
        /// If the instance has been disposed then an InvalidOperationException
        /// will be thrown.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        private void CheckDisposedState()
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Instance is already disposed");
            }
        }
        /// <summary>
        /// Loads up data table from a dataset with the values specified when the
        /// db provider was created
        /// </summary>
        /// <param name="dataTable"></param>
        public void LoadDataTable(DataTable dataTable)
        {
            CheckDisposedState();

            this.DataAdapater.Fill(dataTable);
        }

        /// <summary>
        /// Loads up data table from a dataset with the values specified when the
        /// db provider was created.
        /// Lets the user specify a timeout... in seconds.
        /// </summary>
        public void LoadDataTable(DataTable dataTable, int timeoutInSeconds)
        {
            CheckDisposedState();

            // Set the timeout...
            this.DataAdapater.SelectCommand.CommandTimeout = timeoutInSeconds;

            // Get the data...
            this.DataAdapater.Fill(dataTable);

            // Reset the timeout now that we are done...
            this.DataAdapater.SelectCommand.ResetCommandTimeout();
        }

        /// <summary>
        /// Load up data table with the data returned from stored procdure
        /// </summary>
        public DataTable LoadDataTable()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(this.Command))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        /// <summary>
        /// Loads up data table from a dataset with the values specified when the
        /// db provider was created
        /// Use this overload if the Select StoredProcedure of the table being loaded
        /// needs parameter(s)
        /// </summary>
        public void LoadDataTable(DataTable dataTable, Dictionary<string, object> parameters)
        {
            CheckDisposedState();

            foreach (var parameter in parameters)
            {
                this.dataAdapter.SelectCommand.Parameters[parameter.Key].Value = parameter.Value;
            }
            this.DataAdapater.Fill(dataTable);
        }

        /// <summary>
        /// Saves a dataset's datatable to the database using the values
        /// specified when db provider was created
        /// </summary>
        /// <param name="dataTable"></param>
        public void SaveDataTable(DataTable dataTable)
        {
            CheckDisposedState();
            this.DataAdapater.Update(dataTable);
            if (this.sqlTransaction != null)
            {
                this.sqlTransaction.Commit();
                this.sqlTransaction.Dispose();
                this.sqlTransaction = null;
            }
        }


        /// <summary>
        /// Clear all of the parameters from this SQLServerDBProvider's Command object.  Useful for re-using same provider
        /// for multiple SQL calls.
        /// </summary>
        public void ClearParameters()
        {
            if ((this.Command != null) && (this.Command.Parameters != null))
            {
                this.Command.Parameters.Clear();
            }
        }

        /// <summary>
        /// Clear all of the parameters from this SQLServerDBProvider's Command objects in the data-adapter.  Useful for re-using same provider
        /// for multiple SQL calls.
        /// </summary>
        /// <param name="commandType"></param>
        public void ClearParameters(DBCommandTypes commandType)
        {
            if (this.dataAdapter != null)
            {
                if ((commandType & DBCommandTypes.Select) != 0)
                {
                    this.dataAdapter.SelectCommand.Parameters.Clear();
                }
                if ((commandType & DBCommandTypes.Delete) != 0)
                {
                    this.dataAdapter.DeleteCommand.Parameters.Clear();
                }
                if ((commandType & DBCommandTypes.Insert) != 0)
                {
                    this.dataAdapter.InsertCommand.Parameters.Clear();
                }
                if ((commandType & DBCommandTypes.Update) != 0)
                {
                    this.dataAdapter.UpdateCommand.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            IDbTransaction transaction = null;
            if (this.Connection != null)
            {
                transaction = this.Connection.BeginTransaction();
                if (this.Command != null)
                {
                    this.Command.Transaction = transaction as SqlTransaction;
                }
            }
            return transaction;
        }

        /// <summary>
        /// Sets text for the command.
        /// </summary>
        public void SetCommandText(string commandText)
        {
            this.Command.CommandText = commandText;
        }

        /// <summary>
        /// Sets command type for this provider.
        /// </summary>
        public void SetCommandType(CommandType commandType)
        {
            this.Command.CommandType = commandType;
        }

        /// <summary>
        /// SaveDataTableSetRowState
        /// </summary>
        public int SaveDataTableSetRowState(string tableName, DataTable table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the specified file using batch processor.
        /// </summary>
        /// <param name="resourceName">Name of the embedded resource</param>
        public void ExecuteResourceScript(string resourceName)
        {
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)))
            {
                this.ExecuteScript(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// Exceutes batch of Sql scrips from given file name.
        /// </summary>
        public void ExecuteFileScript(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                this.ExecuteScript(sr.ReadToEnd());
            }
        }

        #endregion

        #region IDisposable interface + Cleanup

        /// <summary>
        /// Public Dispose method
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Private Dispose method
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing == true)
                {
                    if (this.DataReader != null)
                    {
                        this.DataReader.Dispose();
                        this.dataReader = null;
                    }
                    if (this.Command != null)
                    {
                        this.Command.Dispose();
                        this.command = null;
                    }

                    if (this.Connection != null)
                    {
                        this.Connection.Dispose();
                        this.connection = null;
                    }

                    if (this.dataAdapter != null)
                    {
                        this.dataAdapter.Dispose();
                        this.dataReader = null;
                    }
                    SQLServerDBProvider.numberOfCurrentlyOpenConnections--;
                }
                this.isDisposed = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Exceutes given script
        /// </summary>
        /// <param name="script"></param>
        private void ExecuteScript(string script)
        {
        
        }

        #endregion

    }
}
