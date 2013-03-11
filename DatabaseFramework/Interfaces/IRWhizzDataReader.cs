using System;
using System.Drawing;
using System.Data;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Interface for all the lutron data readers.
    /// </summary>
    public interface IWhizzDataReader : IDisposable
    {

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// checks if the value for this column is null
        /// </summary>
        /// <param name="columnName"></param>
        bool IsNull(string columnName);

        /// <summary>
        /// Checks to see if given column exists
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        bool ColumnExists(string columnName);

        /// <summary>
        /// Returns the number of records affected by the load
        /// </summary>
        /// <returns></returns>
        int GetRecordCount();

        /// <summary>
        /// Get Int64 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        Int64? GetInt64(string fieldName);

        /// <summary>
        /// Get Image value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        Image GetImage(string fieldName);

        /// <summary>
        /// Get a not-null Int64 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int64 value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        Int64 GetNotNullInt64(string fieldName);

        /// <summary>
        /// Get Int32 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        Int32? GetInt32(string fieldName);

        /// <summary>
        /// Get a not-null Int32 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int32 value that is not null</returns>
        Int32 GetNotNullInt32(string fieldName);

        /// <summary>
        /// Get a not-null DateTime value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>A DateTime value that is not null</returns>
        DateTime GetNotNullDateTime(string fieldName);


        /// <summary>
        /// Get a DateTime value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>A DateTime value that is not null</returns>
        DateTime? GetDateTime(string fieldName);

        /// <summary>
        /// Get Int16 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Int16</returns>
        Int16? GetInt16(string fieldName);

        /// <summary>
        /// Get a not-null Int16 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int16 value that is not null</returns>
        Int16 GetNotNullInt16(string fieldName);

        /// <summary>
        /// Get string value that is not null of specified field name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>string</returns>
        string GetNotNullString(string fieldName);

        /// <summary>
        /// Get string value of specified field name
        /// Returns null if value is DBNull
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>string</returns>
        string GetString(string fieldName);

        /// <summary>
        /// Get Boolean value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Boolean</returns>
        bool GetBool(string fieldName);

        /// <summary>
        /// Get Byte value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Byte</returns>
        Byte GetByte(string fieldName);

        /// <summary>
        /// Get SByte value of specified field name
        /// </summary>
        SByte GetSByte(string fieldName);

        /// <summary>
        /// Get Nullable Byte value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Byte</returns>
        Byte? GetNullableByte(string fieldName);

        /// <summary>
        /// Get string value of specified field name
        /// Returns null if value is DBNull
        /// Returns null if field is not available
        /// </summary>
        string GetStringIfFieldNameExists(string fieldName);

        /// <summary>
        /// Get double value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Double</returns>
        Double GetDouble(string fieldName);

        /// <summary>
        /// Get possibly null floating point value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Float (possibly null)</returns>
        float? GetFloat(string fieldName);

        /// <summary>
        /// Load DataReader into DataTable
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <returns>DataTable</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.ExceptionFramework.LutronException"></exception>
        DataTable LoadDataTable(String tableName);

        /// <summary>
        /// Advance to next record
        /// </summary>
        /// <returns>Boolean</returns>
        bool Read();

        /// <summary>
        /// Advance the DataReader to next result.
        /// </summary>
        /// <returns>Boolean</returns>
        bool NextResult();

        /// <summary>
        /// Gets field name given the index of the field.
        /// </summary>
        string GetFieldName(int inedx);

        /// <summary>
        /// Gets ordinal of the given field in the reader.
        /// </summary>
        int GetFieldOrdinal(string fieldName);

        /// <summary>
        /// Returns actual value as object for the given field name.
        /// </summary>
        object GetActualValue(string fieldName);
    }
}
