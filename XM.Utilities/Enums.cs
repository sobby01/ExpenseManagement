using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XM.Utilities
{
    public static class Enums
    {
        /// <summary>
        /// Understands the status of the Configuration File
        /// </summary>
        public enum ConfigFileStatus
        {
            /// <summary>
            /// Unknown value
            /// </summary>
            unknown = 0,

            /// <summary>
            /// Config File exist
            /// </summary>
            fileOK,

            /// <summary>
            /// Config File doesn't exist
            /// </summary>
            fileNotFound
        }

        /// <summary>
        /// Available data storage types.
        /// </summary>
        public enum DataStorageType
        {
            /// <summary>
            /// SQL Server data storage.
            /// </summary>
            Firebird = 1,

            /// <summary>
            /// Firebird data storage type
            /// </summary>
            SQLServer = 2,

            /// <summary>
            /// Default data storage type.
            /// </summary>
            Default = Firebird

        }

        /// <summary>
        /// DataSetCommandTypes
        /// </summary>
        [Flags]
        public enum DBCommandTypes
        {
            /// <summary>
            /// The type is unknown
            /// </summary>
            None = 0,

            /// <summary>
            /// The delete command
            /// </summary>
            Delete = 0x01,

            /// <summary>
            /// The insert command
            /// </summary>
            Insert = 0x02,

            /// <summary>
            /// The select command
            /// </summary>
            Select = 0x04,

            /// <summary>
            /// The update command
            /// </summary>
            Update = 0x08
        }
    }
}