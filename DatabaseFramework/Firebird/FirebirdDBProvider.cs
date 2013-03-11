using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.IO;
using System.Drawing;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.Threading;
using System.Net;
using System.Drawing.Imaging;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Firebird DB Provider.
    /// </summary>
    public class FirebirdDBProvider : IDBProvider
    {
        #region Fields and Constants

        private FbConnection connection;
        private FbCommand command;
        private FirebirdDataReader dataReader;
        private bool isDisposed;

        private FbDataAdapter dataAdapter;
        private FbTransaction fbTransaction;

        private static int numberOfCurrentlyOpenConnections;

        #endregion

        #region Constructors / Finalizer


        /// <summary>
        /// Constructor
        /// </summary>
        private FirebirdDBProvider()
        {

        }

        /// <summary>
        /// Required since we want to make sure connections are cleaned 
        /// up correctly if the caller does not call Dispose.
        /// </summary>
        ~FirebirdDBProvider()
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
        /// incremented each time a new FirebirdDBProvider is created and decremented when
        /// the FirebirdDBProvider is disposed.  Should only be used as a rough guide.  
        /// </summary>
        public static int NumberOfCurrentlyOpenConnections
        {
            get
            {
                return FirebirdDBProvider.numberOfCurrentlyOpenConnections;
            }
        }

        /// <summary>
        /// Database Connection Object
        /// </summary>
        private FbConnection Connection
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
        private FbCommand Command
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
        private FbDataAdapter DataAdapater
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
        private FirebirdDataReader DataReader
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

        #region Parameter Get/Set Methods

        /// <summary>
        /// Adds a new boolean parameter to the command's parameter list.
        /// </summary>
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
                FbParameter param = new FbParameter(storedProcedureParameterName, FbDbType.SmallInt);
                param.Value = (bool)parameterValue ? 1 : 0;
                this.Command.Parameters.Add(param);
            }
            else
            {
                this.Command.Parameters.AddWithValue(storedProcedureParameterName, DBNull.Value);
            }
        }

        /// <summary>
        /// Adds a new byte parameter to the command's parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, byte parameterValue)
        {
            this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, (short)parameterValue));
        }

        /// <summary>
        /// Adds a new int32 parameter to the command's parameters list.
        /// </summary>
        public void Add(string storedProcedureParameterName, Int32? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, parameterValue));
            }
            else
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
            }
        }

        /// <summary>
        /// Adds a new int16 parameter to the command's parameters list.
        /// </summary>
        public void Add(string storedProcedureParameterName, Int16? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, (Int32)parameterValue));
            }
            else
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
            }
        }

        /// <summary>
        /// Adds a new Int64 parameter to the command's parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, Int64? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, parameterValue));
            }
            else
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
            }
        }

        /// <summary>
        /// Adds a new string parameter to the command's parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, string parameterValue)
        {
            FbParameter param = new FbParameter(storedProcedureParameterName, FbDbType.VarChar);
            param.Value = (object)parameterValue;
            this.Command.Parameters.Add(param);
        }

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, DateTime parameterValue)
        {
            if (parameterValue > DateTime.MinValue)
            {
                if (parameterValue > (DateTime)SqlDateTime.MinValue)
                {
                    this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, parameterValue));
                }
                else
                {
                    this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, SqlDateTime.MinValue));
                }
            }
            else
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
            }
        }

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, DateTime? parameterValue)
        {
            if (parameterValue.HasValue)
            {
                if (parameterValue > DateTime.MinValue)
                {
                    if (parameterValue > (DateTime)SqlDateTime.MinValue)
                    {
                        this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, parameterValue));
                    }
                    else
                    {
                        this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, SqlDateTime.MinValue));
                    }
                }
                else
                {
                    this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
                }
            }
            else
            {
                this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, DBNull.Value));
            }
        }

        /// <summary>
        /// Adds a parameter to the relevant command(s)' parameter list.
        /// </summary>
        public void Add(DBCommandTypes commandTypeToAddTo, string storedProcedureParameterName, object paramValue)
        {
            if (this.dataAdapter != null)
            {
                if ((commandTypeToAddTo & DBCommandTypes.Select) != 0)
                {
                    this.dataAdapter.SelectCommand.Parameters.Add(new FbParameter(storedProcedureParameterName, paramValue));
                }
                if ((commandTypeToAddTo & DBCommandTypes.Delete) != 0)
                {
                    this.dataAdapter.DeleteCommand.Parameters.Add(new FbParameter(storedProcedureParameterName, paramValue));
                }
                if ((commandTypeToAddTo & DBCommandTypes.Insert) != 0)
                {
                    this.dataAdapter.InsertCommand.Parameters.Add(new FbParameter(storedProcedureParameterName, paramValue));
                }
                if ((commandTypeToAddTo & DBCommandTypes.Update) != 0)
                {
                    this.dataAdapter.UpdateCommand.Parameters.Add(new FbParameter(storedProcedureParameterName, paramValue));
                }
            }
        }

        /// <summary>
        /// Pseudo-Overload of Add for double Type
        /// elsewise clashes with Int32?
        /// Adds a new double parameter to the command's parameter list.
        /// </summary>
        public void AddDouble(string storedProcedureParameterName, double parameterValue)
        {
            this.Command.Parameters.Add(new FbParameter(storedProcedureParameterName, parameterValue));
        }


        /// <summary>
        /// Adds an image parameter to the parameter list.
        /// </summary>
        public void Add(string storedProcedureParameterName, Image parameterValue)
        {
            FbParameter parameter = new FbParameter(storedProcedureParameterName, FbDbType.Binary);
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
        public void AddOutInt(string storedProcedureParameterName)
        {
            FbParameter parameter = this.Command.Parameters.Add(storedProcedureParameterName, FbDbType.Integer);
            parameter.Direction = ParameterDirection.Output;
        }

        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
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
        public void AddColumnToDataSetCommands(DataColumn dataColumn,
                                               string storedProcedureParameterName,
                                               DBCommandTypes dataSetCommandTypesToBeAddedTo)
        {
            if (dataColumn == null)
            {
                // The data column must be specified
                throw new Exception("dataColumn");
            }

            if (string.IsNullOrEmpty(storedProcedureParameterName))
            {
                // The stored procedure paramater name must be specified
                throw new Exception("storedProcedureParameterName");
            }

            if (dataSetCommandTypesToBeAddedTo == DBCommandTypes.None)
            {
                // There are no command types specified to add to so there is no
                // work to be done, just return
                return;
            }

            FbDbType dbType = FbDbType.Integer;
            int dbSize = 0;
            if (dataColumn.DataType == typeof(bool))
            {
                dbType = FbDbType.SmallInt;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(UInt32))
            {
                dbType = FbDbType.Integer;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(decimal))
            {
                dbType = FbDbType.Decimal;
                dbSize = 0;
            }
            else if (dataColumn.DataType == typeof(string))
            {
                dbType = FbDbType.VarChar;
                dbSize = dataColumn.MaxLength;
            }

            foreach (DBCommandTypes dataSetCommandType in Enum.GetValues(typeof(DBCommandTypes)))
            {
                if ((dataSetCommandType & dataSetCommandTypesToBeAddedTo) > 0)
                {
                    FbParameter dbParamater;
                    if (dbSize == 0)
                    {
                        dbParamater = new FbParameter(storedProcedureParameterName, dbType);
                    }
                    else
                    {
                        dbParamater = new FbParameter(storedProcedureParameterName, dbType, dbSize);
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
        /// Get parameter value for given name
        /// </summary>
        public object GetParameterValue(string storedProcedureParameterName)
        {
            return this.Command.Parameters[storedProcedureParameterName].Value;
        }

        /// <summary>
        /// Clear all of the parameters from this FirebirdDBProvider's Command object.  Useful for re-using same provider
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
        /// Clear all of the parameters from this FirebirdDBProvider's Command objects in the data-adapter.  Useful for re-using same provider
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
        /// AddWithValue
        /// </summary>
        public void AddWithValue(string parameterName, object value)
        {
            this.Command.Parameters.AddWithValue(parameterName, value);
        }

        #endregion

        #region New Provider Methods

        /// <summary>
        /// Creates a new FirebirdDBProvider that is connected to the Gulliver Project
        /// database.
        /// </summary>
        internal static FirebirdDBProvider NewProjectProvider(string storedProcedure)
        {
            return NewProvider(storedProcedure, RWhizzConfiguration.DatabaseConnectionString);
        }

        /// <summary>
        /// Creates a new FirebirdDBProvider that is connected to the Gulliver Project and set up for a dataset
        /// database.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <returns></returns>
        internal static FirebirdDBProvider NewDatabaseDataSetProvider(string deleteStoredProcedureName,
                                                           string insertStoredProcedureName,
                                                           string selectStoredProcedureName,
                                                           string updateStoredProcedureName)
        {
            return NewDataSetProvider(deleteStoredProcedureName,
                                      insertStoredProcedureName,
                                      selectStoredProcedureName,
                                      updateStoredProcedureName,
                                      RWhizzConfiguration.DatabaseConnectionString,
                                      false);
        }

        /// <summary>
        /// Creates a new FirebirdDBProvider that is connected to the Gulliver Project and set up for a dataset
        /// database.
        /// Mehtod is adding to improve the performacne in Post transfer steps. Its anly supported for SaveDataTable Method for now.
        /// </summary>
        /// <param name="deleteStoredProcedureName">The name of the delete stored procedure to execute.</param>
        /// <param name="insertStoredProcedureName">The name of the insert stored procedure to execute.</param>
        /// <param name="selectStoredProcedureName">The name of the select stored procedure to execute.</param>
        /// <param name="updateStoredProcedureName">The name of the update stored procedure to execute.</param>
        /// <returns></returns>
        internal static FirebirdDBProvider NewDatabaseDataSetProviderWithTransaction(string deleteStoredProcedureName,
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
        /// the conectionString parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static FirebirdDBProvider NewProvider(string sqlText, string connectionString, CommandType commandType)
        {
            // Try to create the db provider... if it fails, we will try again one time to
            // overcome the .NET connection pool bug that sometimes happens.
            FirebirdDBProvider provider = null;
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
                provider = NewProvider_Internal(sqlText, connectionString, commandType);
            }

            // Return what we have (if there was an error even the second time, we wouldn't even get here).
            return provider;
        }

        /// <summary>
        /// Creates a new DbProvider which talks to the database specified in
        /// the conectionString parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static FirebirdDBProvider NewProvider(string storedProcedureName, string connectionString)
        {
            return NewProvider(storedProcedureName, connectionString, CommandType.StoredProcedure);
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
        internal static FirebirdDBProvider NewDataSetProviderWithTransaction(string deleteStoredProcedureName,
                                                    string insertStoredProcedureName,
                                                    string selectStoredProcedureName,
                                                    string updateStoredProcedureName,
                                                    string connectionString)
        {
            // Try to create the db provider... if it fails, we will try again one time to
            // overcome the .NET connection pool bug that sometimes happens.
            FirebirdDBProvider provider = null;
            bool tryAgain = false;
            try
            {
                // 1st try...
                provider = NewDataSetProvider(
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
                provider = NewDataSetProvider(
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        internal static FirebirdDBProvider NewDataSetProvider(string deleteStoredProcedureName,
                                                    string insertStoredProcedureName,
                                                    string selectStoredProcedureName,
                                                    string updateStoredProcedureName,
                                                    string connectionString,
                                                    bool withTransaction)
        {
            FirebirdDBProvider provider = new FirebirdDBProvider();

            try
            {

                IDbTransaction transaction = null;
                if (GlobalTransaction.IsGlobalTransactionInProgress())
                {
                    transaction = GlobalTransaction.CurrentGlobalTransaction.GetDbTransaction(connectionString, provider);
                    provider.Connection = (FbConnection)transaction.Connection;
                    withTransaction = true;
                }
                else
                {
                    provider.Connection = (FbConnection)provider.CreateConnection(connectionString);
                    provider.Connection.Open();


                    // Increment our counter for the number of open connections
                    FirebirdDBProvider.numberOfCurrentlyOpenConnections++;
                }

                FbCommand command = provider.Connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = selectStoredProcedureName;

                provider.DataAdapater = new FbDataAdapter(command);

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
                    command.Transaction = (FbTransaction)transaction;// provider.fbTransaction = provider.Connection.BeginTransaction();
                    provider.DataAdapater.DeleteCommand.Transaction = command.Transaction;
                    provider.DataAdapater.InsertCommand.Transaction = command.Transaction;
                    provider.DataAdapater.UpdateCommand.Transaction = command.Transaction;
                    provider.DataAdapater.SelectCommand.Transaction = command.Transaction;
                }
            }
            
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            return provider;

        }

        /// <summary>
        /// Creates a new DbProvider which talks to the database specified in
        /// the conectionString parameter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        private static FirebirdDBProvider NewProvider_Internal(string commandText, string connectionString, CommandType commandType)
        {
            FirebirdDBProvider provider = new FirebirdDBProvider();
            try
            {
                IDbTransaction transaction = null;
                if (GlobalTransaction.IsGlobalTransactionInProgress())
                {
                    transaction = GlobalTransaction.CurrentGlobalTransaction.GetDbTransaction(connectionString, provider);
                    provider.Connection = (FbConnection)transaction.Connection;
                }
                else
                {
                    provider.Connection = (FbConnection)provider.CreateConnection(connectionString);
                    provider.Connection.Open();

                    //Increment our counter for the number of open connections
                    FirebirdDBProvider.numberOfCurrentlyOpenConnections++;
                }


                provider.Command = provider.Connection.CreateCommand();
                provider.Command.CommandType = commandType;
                provider.Command.CommandText = commandText;

                if (transaction != null)
                {
                    provider.Command.Transaction = (FbTransaction)transaction;
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            return provider;

        }

        /// <summary>
        /// Creates a new DataAdapter with Select command instantiated.
        /// </summary>
        private void NewDataAdapterWithSelect(string tableName)
        {
            if (this.DataAdapater != null) this.DataAdapater.Dispose();

            this.DataAdapater = new FbDataAdapter();
            FbCommand command;

            command = this.Connection.CreateCommand();
            command.CommandType = CommandType.TableDirect;
            command.CommandText = "SELECT * FROM " + tableName;
            this.DataAdapater.SelectCommand = command;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates new connection with the given connection string.
        /// </summary>
        public IDbConnection CreateConnection(string connectionString)
        {
            return new FbConnection(connectionString);
        }

        /// <summary>
        /// Creates a new FirebirdDBProvider that is connected to the project database.
        /// </summary>
        public static FirebirdDBProvider NewProjectProvider(CommandType cmdType)
        {
            FirebirdDBProvider provider = NewProvider_Internal(null,
                RWhizzConfiguration.DatabaseConnectionString, cmdType);

            return provider;
        }

        /// <summary>
        /// Returns a Dataset populated with the results of executing the specified storedProcedure on the database specified in
        /// the conectionString parameter.
        /// </summary>
        public DataSet GetDataSet(string tableName)
        {
            using (FbDataAdapter dataAdapter = new FbDataAdapter(this.Command))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, tableName);
                return dataSet;
            }
        }

        /// <summary>
        /// Checks if the data reader object of this FirebirdDBProvider is disposed and, if not, closes it and disposes it.
        /// This is needed for code that plans to execute a reader more than once on the same provider object, as
        /// this FirebirdDBProvider only allows one instance of a reader to exist at a time.
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
        public IWhizzDataReader ExecuteReader()
        {
            CheckDisposedState();

            //Make sure if the user tries to open a second DataReader on the same
            //connection that we throw an exception.  This is probably due to a 
            //programming error which needs to be fixed.
            if (this.DataReader != null)
            {
                throw new Exception();
            }

            FbDataReader reader = null;
            try
            {
                // Execute the reader...
                reader = this.Command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Make it safe...
            this.DataReader = new FirebirdDataReader(reader, this.connection.Database);

            // Return...
            return this.DataReader;
        }

        /// <summary>
        /// Executes the current command and returns a SafeDataReader 
        /// so the caller can iterate through the returned data.
        /// Specifies a timeout value, in seconds...
        /// </summary>
        public IWhizzDataReader ExecuteReader(int timeOut)
        {
            CheckDisposedState();

            //Make sure if the user tries to open a second DataReader on the same
            //connection that we throw an exception.  This is probably due to a 
            //programming error which needs to be fixed.
            if (this.DataReader != null)
            {
                throw new Exception();
            }

            //Execute the datareader and store a reference so that we can close 
            //the reader gracefully in the future.

            // Set the timeout...
            this.Command.CommandTimeout = timeOut;

            FbDataReader reader = null;

            try
            {
                // Execute the reader...
                reader = this.Command.ExecuteReader();
            }
            catch (ThreadAbortException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Make it safe...
            this.DataReader = new FirebirdDataReader(reader, this.connection.Database);

            // Return...
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        public DateTime ExecuteDateTimeScalar()
        {
            CheckDisposedState();

            try
            {
                return (DateTime)this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Load up data table with the data returned from stored procdure
        /// </summary>
        public DataTable LoadDataTable()
        {
            using (FbDataAdapter dataAdapter = new FbDataAdapter(this.Command))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
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

            int prevTimeout = this.DataAdapater.SelectCommand.CommandTimeout;

            try
            {
                // Set the timeout...
                this.DataAdapater.SelectCommand.CommandTimeout = timeoutInSeconds;

                // Get the data...
                this.DataAdapater.Fill(dataTable);

            }
            finally
            {
                // Reset the timeout now that we are done...
                this.DataAdapater.SelectCommand.CommandTimeout = prevTimeout;
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
        public void SaveDataTable(DataTable dataTable)
        {
            CheckDisposedState();
            this.DataAdapater.Update(dataTable);
            if (this.fbTransaction != null)
            {
                this.fbTransaction.Commit();
                this.fbTransaction.Dispose();
                this.fbTransaction = null;
            }
        }

        /// <summary>
        /// Saves a dataset's datatable to the database using the values
        /// specified when db provider was created
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataTable"></param>
        public int SaveDataTableSetRowState(string tableName, DataTable dataTable)
        {
            CheckDisposedState();
            int recordsAffected = 0;
            //Create a data adapter to read Firebird schema for the table.
            this.NewDataAdapterWithSelect(tableName);

            this.DataAdapater.FillLoadOption = LoadOption.OverwriteChanges;
            FbCommandBuilder cmdBuilder = new FbCommandBuilder(this.DataAdapater);

            DataTable table = new DataTable();
            this.DataAdapater.Fill(table);

            cmdBuilder.RefreshSchema();
            table.TableName = dataTable.TableName;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Unchanged) row.SetAdded();
                table.ImportRow(row);
            }
            try
            {
                recordsAffected = this.DataAdapater.Update(table);
            }
            catch
            {
                throw new Exception();
            }

            return recordsAffected;
        }

        /// <summary>
        /// Executes the specified embedded resource as a script using batch execution.
        /// </summary>
        public void ExecuteResourceScript(string resourcePath)
        {
            using (TextReader textReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath)))
            {
                this.ExecuteScript(textReader);
            }
        }

        /// <summary>
        /// Exceutes batch of Sql scrips from given file name.
        /// </summary>
        public void ExecuteFileScript(string fileName)
        {
            using (TextReader textReader = new StreamReader(fileName))
            {
                this.ExecuteScript(textReader);
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
                    this.Command.Transaction = transaction as FbTransaction;
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
                //User has called Dispose, not GC.  If GC called
                //Dispose we cannot be sure that the references 
                //this object are holding are still valid.
                if (disposing == true)
                {
                    if (this.DataReader != null)
                    {
                        this.DataReader.Dispose();
                    }

                    if (this.Command != null)
                    {
                        this.Command.Dispose();
                    }

                    if (this.Connection != null && !GlobalTransaction.IsGlobalTransactionInProgress())
                    {
                        this.Connection.Dispose();
                    }

                    if (this.dataAdapter != null)
                    {
                        this.dataAdapter.Dispose();
                    }
                    FirebirdDBProvider.numberOfCurrentlyOpenConnections--;
                }
                this.isDisposed = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ExecuteScript
        /// </summary>
        private void ExecuteScript(TextReader textReader)
        {
            try
            {
                FbScript fbScript = new FbScript(textReader);
                fbScript.Parse();

                FbBatchExecution fbBatchExection = new FbBatchExecution(this.Connection);
                foreach (string cmd in fbScript.Results)
                {
                    fbBatchExection.SqlStatements.Add(cmd);
                }
                fbBatchExection.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                throw new Exception("Object already disposed");
            }
        }

        #endregion

    }
}

