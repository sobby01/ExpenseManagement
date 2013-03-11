using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainWhizzDatabaseFramework
{
    public enum DataStorageType
    {
        Default,
        Firebird,
        SQLServer
    }

    public enum DBCommandTypes
    {
        None,
        Select,
        Delete,
        Insert,
        Update
    }

    public enum ConfigFileStatus
    {
        Unknown,
        FileNotFound,
        FileOk
    }
}
