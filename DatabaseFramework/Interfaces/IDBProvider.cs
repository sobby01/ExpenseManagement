using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace BrainWhizzDatabaseFramework
{
    public interface IDBProvider : IDisposable
    {
        #region Properties

        /// <summary>
        /// Command timeout to be used with this provider.
        /// </summary>
        int CommandTimeout { get; set; }

        #endregion

        #region Add to default command methods

        /// <summary>
        /// Adds a new boolean parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, bool parameterValue);

        /// <summary>
        /// Adds a new boolean parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, bool? parameterValue);

        /// <summary>
        /// Adds a new byte parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, byte parameterValue);        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        void Add(string storedProcedureParameterName, Int32? parameterValue);  

        /// <summary>
        /// Adds a new Int64 parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, Int64? parameterValue);
        

        /// <summary>
        /// Adds a new string parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure i.e. @SpaceID</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, string parameterValue);        

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure.</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, DateTime parameterValue);
        

        /// <summary>
        /// Adds a new datetime parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName">The name of the parameter in the 
        /// stored procedure.</param>
        /// <param name="parameterValue">The value to pass to the stored procedure.</param>
        void Add(string storedProcedureParameterName, DateTime? parameterValue);
        

        /// <summary>
        /// Adds a parameter to the relevant command(s)' parameter list.
        /// </summary>
        /// <param name="commandTypeToAddTo"></param>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="paramValue"></param>
        void Add(DBCommandTypes commandTypeToAddTo, string storedProcedureParameterName, object paramValue);
        

        /// <summary>
        /// Pseudo-Overload of Add for double Type
        /// elsewise clashes with Int32?
        /// Adds a new double parameter to the command's parameter list.
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        void AddDouble(string storedProcedureParameterName, double parameterValue);
        


        /// <summary>
        /// Add
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        void Add(string storedProcedureParameterName, Image parameterValue);
        

        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        void SetParameterValue(string storedProcedureParameterName, DateTime parameterValue);
       
        /// <summary>
        /// Set/Change value for a existing stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <param name="parameterValue"></param>
        void SetParameterValue(string storedProcedureParameterName, long parameterValue);

        /// <summary>
        /// Adds an OUTPUT parameter of type int
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        void AddOutInt(string storedProcedureParameterName);

        /// <summary>
        /// Gets value of the given stored procedure parameter
        /// </summary>
        /// <param name="storedProcedureParameterName"></param>
        /// <returns></returns>
        object GetParameterValue(string storedProcedureParameterName);

        /// <summary>
        /// Add given parameter name for given value. Wrapper for Parameter.AddWithValue.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        void AddWithValue(string parameterName, object value);

        #endregion

        #region Add column to data set method

        /// <summary>
        /// Adds the passed in dataset column as a paramater to the indicated
        /// dataset commands
        /// </summary>
        /// <remarks>If the datatype of the column in string then be sure to set the MaxLength
        /// field of the column so that the SQL statements will work correctly</remarks>
        /// <param name="dataColumn">Dataset column to add the paramater to</param>
        /// <param name="storedProcedureParameterName">Name of the paramater in the stored procedures</param>
        /// <param name="dataSetCommandTypesToBeAddedTo">Which of the possible commands to add the paramater to</param>
        void AddColumnToDataSetCommands(DataColumn dataColumn,
                                               string storedProcedureParameterName,
                                               DBCommandTypes dataSetCommandTypesToBeAddedTo);
        

        #endregion

        #region Execute methods

        /// <summary>
        /// Checks if the data reader object of this DBProvider is disposed and, if not, closes it and disposes it.
        /// This is needed for code that plans to execute a reader more than once on the same provider object, as
        /// this DBProvider only allows one instance of a reader to exist at a time.
        /// </summary>
        void CloseDataReader();
        

        /// <summary>
        /// Executes the current command and returns a SafeDataReader 
        /// so the caller can iterate through the returned data.
        /// </summary>
        IWhizzDataReader ExecuteReader();
        

        /// <summary>
        /// Executes the current command and returns a SafeDataReader 
        /// so the caller can iterate through the returned data.
        /// 
        /// Specifies a timeout value, in seconds...
        /// </summary>
        /// <param name="timeOut">Timeout value to wait in seconds...</param>
        /// <returns></returns>
        IWhizzDataReader ExecuteReader(int timeOut);
        

        /// <summary>
        /// ExecuteNonQuery against the database
        /// </summary>
        void ExecuteNonQuery();
		
        /// <summary>
        /// ExecuteNonQuery against the database with timeout
        /// </summary>
        void ExecuteNonQuery(int timeOutInSeconds);
       

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>Int64</returns>
        Int64 ExecuteInt64Scalar();
        
        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>Int32</returns>
        Int32 ExecuteInt32Scalar();

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        object ExecuteScalar();

        /// <summary>
        /// Calls ExecuteScalar against the database.
        /// </summary>
        /// <returns>DateTime</returns>
        DateTime ExecuteDateTimeScalar();
        
        /// <summary>
        /// GetDataSet
        /// </summary>
        DataSet GetDataSet(string tableName);

        /// <summary>
        /// LoadDataTable
        /// </summary>
        DataTable LoadDataTable();

        #endregion

        #region Dataset specific commands

        /// <summary>
        /// Loads up data table from a dataset with the values specified when the
        /// db provider was created
        /// </summary>
        /// <param name="dataTable"></param>
        void LoadDataTable(DataTable dataTable);
        

        /// <summary>
        /// Loads up data table from a dataset with the values specified when the
        /// db provider was created.
        /// 
        /// Lets the user specify a timeout... in seconds.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="timeoutInSeconds"></param>
        void LoadDataTable(DataTable dataTable, int timeoutInSeconds);
        

        /// <summary>
        /// Saves a dataset's datatable to the database using the values
        /// specified when db provider was created
        /// </summary>
        /// <param name="dataTable"></param>
        void SaveDataTable(DataTable dataTable);

        /// <summary>
        /// LoadDataTable
        /// </summary>
        void LoadDataTable(DataTable dataTable, Dictionary<string, object> parameters);

        /// <summary>
        /// Saves the DataTable to the table in the database 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        int SaveDataTableSetRowState(string tableName, DataTable table);

        /// <summary>
        /// Executes the specified file using batch processor.
        /// </summary>
        /// <param name="resourceName">Name of the embedded resource</param>
        void ExecuteResourceScript(string resourceName);

        /// <summary>
        /// Exceutes batch of Sql scrips from given file name.
        /// </summary>
        void ExecuteFileScript(string fileName);

        #endregion

        #region Clear Parameters methods

        /// <summary>
        /// Clear all of the parameters from this DBProvider's Command object.  Useful for re-using same provider
        /// for multiple SQL calls.
        /// </summary>
        void ClearParameters();

        #endregion

        #region Transaction

        /// <summary>
        /// Begins a DB Transaction.
        /// </summary>
        /// <returns></returns>
        IDbTransaction BeginTransaction();

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets Text for the command.
        /// </summary>
        void SetCommandText(string commandText);

        /// <summary>
        /// Sets command type for this provider.
        /// </summary>
        void SetCommandType(CommandType commandType);

        /// <summary>
        /// Creates new connection with the given connection string.
        /// </summary>
        IDbConnection CreateConnection(string connectionString);

        #endregion
    }
}