using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Drawing;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// Firebird data reader wrapper class.
    /// </summary>
    internal class FirebirdDataReader : IWhizzDataReader
    {
        #region Private Field

        private IDataReader dataReader;
        private String name;
        private bool isDisposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FirebirdDataReader(IDataReader dataReader, String name)
        {
            this.dataReader = dataReader;
            this.name = name;
        }

        #endregion

        #region Public Properties


        /// <summary>
        /// Returns name of this data reader.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// DataReader
        /// </summary>
        protected IDataReader DataReader
        {
            get
            {
                return this.dataReader;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetBool
        /// </summary>
        public bool GetBool(string fieldName)
        {
            object fieldValue = System.DBNull.Value;
            try
            {
                fieldValue = this.DataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    throw (new Exception());
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return Convert.ToInt32(fieldValue) != 0;
        }

        /// <summary>
        /// Returns actual value as object for the given field name.
        /// </summary>
        public object GetActualValue(string fieldName)
        {
            object fieldValue = null;
            try
            {
                fieldValue = this.dataReader[fieldName];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fieldValue;
        }

        /// <summary>
        /// Returns the number of records affected by the load
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount()
        {
            return dataReader.RecordsAffected;
        }
        /// <summary>
        /// Get Int64 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        public Int64? GetInt64(string fieldName)
        {
            Int64? returnValue = null;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue != System.DBNull.Value)
                {
                    returnValue = (Int64?)fieldValue;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;

        }

        /// <summary>
        /// Get Image value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        public Image GetImage(string fieldName)
        {
            Image returnValue = null;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue != DBNull.Value)
                {
                    byte[] image = (byte[])fieldValue;
                    MemoryStream imageStream = new MemoryStream(image);
                    Bitmap imageBitmap = new Bitmap(imageStream);
                    imageStream.Close();
                    returnValue = imageBitmap;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// Get a not-null Int64 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int64 value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        public Int64 GetNotNullInt64(string fieldName)
        {
            Int64? returnValue = this.GetInt64(fieldName);
            if (returnValue.HasValue)
            {
                return returnValue.Value;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get Int32 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        public Int32? GetInt32(string fieldName)
        {
            Int32? returnValue = null;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue != System.DBNull.Value)
                {
                    returnValue = (Int32?)fieldValue;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;

        }

        /// <summary>
        /// Get a not-null Int32 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int32 value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        public Int32 GetNotNullInt32(string fieldName)
        {
            Int32? returnValue = this.GetInt32(fieldName);
            if (returnValue.HasValue)
            {
                return returnValue.Value;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a not-null DateTime value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>A DateTime value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        public DateTime GetNotNullDateTime(string fieldName)
        {
            DateTime? returnValue = this.GetDateTime(fieldName);
            if (returnValue.HasValue)
            {
                return returnValue.Value;
            }
            else
            {
                throw new Exception();
            }
        }


        /// <summary>
        /// Get a DateTime value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>A DateTime value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        public DateTime? GetDateTime(string fieldName)
        {
            try
            {
                DateTime? returnValue = null;
                object fieldValue = this.DataReader[fieldName];
                if (fieldValue != System.DBNull.Value)
                {
                    returnValue = (DateTime?)fieldValue;
                    if (returnValue <= (DateTime)SqlDateTime.MinValue)
                    {
                        returnValue = (DateTime)SqlDateTime.MinValue;
                    }
                }
                if (returnValue == null)
                    returnValue = DateTime.MinValue;
                return returnValue;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get Int16 value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Int16</returns>
        public Int16? GetInt16(string fieldName)
        {
            Int16? returnValue = null;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue != System.DBNull.Value)
                {
                    returnValue = (Int16?)fieldValue;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;

        }

        /// <summary>
        /// Get a not-null Int16 value of the specified field name, 
        /// if the value in the field is null then an exception is thrown
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>An Int16 value that is not null</returns>
        /// <exception cref="LutronDataReaderNullValueException">Thrown when value is null</exception>
        public Int16 GetNotNullInt16(string fieldName)
        {
            Int16? returnValue = this.GetInt16(fieldName);
            if (returnValue.HasValue)
            {
                return returnValue.Value;
            }
            else
            {
               throw new Exception();
            }
        }

        /// <summary>
        /// Get string value that is not null of specified field name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>string</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.DatabaseFramework.DatabaseException"></exception>
        public string GetNotNullString(string fieldName)
        {
            string fieldValue = this.GetString(fieldName);

            if (fieldValue == null)
            {
                throw new Exception();
            }
            return fieldValue;
        }

        /// <summary>
        /// Get string value of specified field name
        /// Returns null if value is DBNull
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>string</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.DatabaseFramework.DatabaseException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual string GetString(string fieldName)
        {
            string returnValue;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    returnValue = null;
                }
                else
                {
                    returnValue = this.dataReader.GetString(this.dataReader.GetOrdinal(fieldName));
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// Get string value of specified field name
        /// Returns null if value is DBNull
        /// Returns null if field is not available
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>string</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.DatabaseFramework.DatabaseException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public string GetStringIfFieldNameExists(string fieldName)
        {
            string returnValue = null;

            if (HasColumn(this.dataReader, fieldName))
            {
                returnValue = GetString(fieldName);
            }
            return returnValue;
        }

        /// <summary>
        /// Check if the specified column exsists in the current DataReader.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool ColumnExists(string columnName)
        {
            return HasColumn(this.dataReader, columnName);
        }

        /// <summary>
        /// Check if the specified collumn exsists in the given DataReader.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private bool HasColumn(IDataReader dr, string columnName)
        {
            for (int fieldCount = 0; fieldCount < dr.FieldCount; fieldCount++)
            {
                if (dr.GetName(fieldCount).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get Byte value of specified field name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Byte GetByte(string fieldName)
        {
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    throw new Exception();
                }
               return Convert.ToByte(fieldValue);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get SByte value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>SByte</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.DatabaseFramework.DatabaseException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public SByte GetSByte(string fieldName)
        {
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    throw new Exception();
                }
                return Convert.ToSByte(fieldValue);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get Nullable Byte value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Byte</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.DatabaseFramework.DatabaseException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public Byte? GetNullableByte(string fieldName)
        {
            Byte? returnValue = null;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue != System.DBNull.Value)
                {
                    returnValue = Convert.ToByte(fieldValue);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }        

        /// <summary>
        /// Get double value of a specified field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Double GetDouble(string fieldName)
        {
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    throw new Exception(fieldName);
                }
                // If the type in database is float, we will have decimal point discrepencies when
                // we convert float to double. .NET API for SQL Server returns Double even for Float fields
                // .NET API for Firebird returns Float for Float fields.
                if (fieldValue.GetType() == typeof(float))
                {
                    return double.Parse(fieldValue.ToString());
                }
                return Convert.ToDouble(fieldValue);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get possibly null floating point value of specified field name
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns>Float (possibly null)</returns>
        public float? GetFloat(string fieldName)
        {
            float? returnValue;
            try
            {
                object fieldValue = this.dataReader[fieldName];
                if (fieldValue == System.DBNull.Value)
                {
                    returnValue = null;
                }
                else
                {
                    returnValue = Convert.ToSingle(fieldValue);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets field name given the index of the field.
        /// </summary>
        public string GetFieldName(int inedx)
        {
            return this.dataReader.GetName(inedx);
        }

        /// <summary>
        /// Gets ordinal of the given field in the reader.
        /// </summary>
        public int GetFieldOrdinal(string fieldName)
        {
            return this.dataReader.GetOrdinal(fieldName);
        }

        /// <summary>
        /// Load DataReader into DataTable
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <returns>DataTable</returns>
        /// <exception cref="Lutron.Gulliver.Infrastructure.ExceptionFramework.LutronException"></exception>
        public DataTable LoadDataTable(String tableName)
        {
            if (dataReader == null)
            {
                throw new Exception("DataReader is null");
            }
            DataTable table = new DataTable(tableName);
            table.Load(this.dataReader, LoadOption.OverwriteChanges);
            return table;
        }

        /// <summary>
        /// Advance to next record
        /// </summary>
        /// <returns>Boolean</returns>
        public bool Read()
        {
            return this.dataReader.Read();
        }

        /// <summary>
        /// Advance the DataReader to next result.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool NextResult()
        {
            return this.dataReader.NextResult();
        }

        /// <summary>
        /// checks if the value for this column is null
        /// </summary>
        /// <param name="columnName"></param>
        public bool IsNull(string columnName)
        {
            return (this.dataReader[columnName] == DBNull.Value);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.isDisposed = true;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (this.dataReader != null)
            {
                this.dataReader.Dispose();
                this.dataReader = null;
            }
        }

        #endregion
    }
}
