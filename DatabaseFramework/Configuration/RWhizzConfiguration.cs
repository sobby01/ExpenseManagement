using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BrainWhizzDatabaseFramework
{
    /// <summary>
    /// This class should be responsible to read all the information of config file
    /// </summary>
    public static class RWhizzConfiguration
    {
        #region Private Fields

        /// <summary>
        /// database connection string
        /// </summary>
        private static string databaseConnectionString = string.Empty;

        /// <summary>
        /// The configurationAgent knows how to load the configuration
        /// file from wherever it is stored.
        /// </summary>
        private static ConfigurationManager configurationManager;

        /// <summary>
        /// database storage type
        /// </summary>
        private static DataStorageType? databaseStorageType;

        /// <summary>
        /// app data path
        /// </summary>
        private static string appDataPath = string.Empty;

        /// <summary>
        /// Stores a connection string without any database selected, will point to [master]
        /// used only for checking availibility SQL Server
        /// </summary>
        private static string nonDatabaseSpecificConnectionString;

        #endregion

        #region Constants

        /// <summary>
        /// Database connection key - this should be mapped with configuration key
        /// Using this key application try to read value from configuration file
        /// </summary>
        private static string DatabaseConnectionKey = "DatabaseConnectionString";

        /// <summary>
        /// Storage type of database.
        /// </summary>
        public static string DatabaseStorageTypeKey = "DatabaseStorageType";

        public static string AppDirectoryPathKey = "AppFolderPath";

        /// <summary>
        /// key for non database specific connection string
        /// </summary>
        public static string nonDatabaseSpecificConnectionStringKey = "NonDatabaseSpecificConnectionString";

        #endregion

        #region Properties

        /// <summary>
        /// Get instance of configuration manager
        /// </summary>
        public static ConfigurationManager ConfigManager
        {
            get
            {
                if (RWhizzConfiguration.configurationManager == null)
                {
                    RWhizzConfiguration.configurationManager = new ConfigurationManager();
                }
                return RWhizzConfiguration.configurationManager;
            }
        }

        /// <summary>
        /// DatabaseStorageType
        /// </summary>
        public static DataStorageType DatabaseStorageType
        {
            get
            {
                if (RWhizzConfiguration.databaseStorageType == null)
                {
                    //Get value of database storage
                    string value = GetAppSettingKeyValue(DatabaseStorageTypeKey);
                    if (string.IsNullOrEmpty(value))
                    {
                        RWhizzConfiguration.databaseStorageType = DataStorageType.Default;
                    }
                    else
                    {
                        RWhizzConfiguration.databaseStorageType = (DataStorageType)Enum.Parse(typeof(DataStorageType), value);
                    }
                }

                return (DataStorageType)RWhizzConfiguration.databaseStorageType;
            }
        }

        /// <summary>
        /// Get app data path
        /// </summary>
        public static string GetAppDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(RWhizzConfiguration.appDataPath))
                {
                    string path = Path.Combine(ConfigManager.CurrentDirectory, @"..\..");
                    DirectoryInfo dir = Directory.GetParent(Directory.GetParent(Directory.GetParent(ConfigManager.CurrentDirectory).ToString()).ToString());
                    appDataPath = Path.Combine(dir.ToString(), GetAppSettingKeyValue(AppDirectoryPathKey));
                }

                return appDataPath;
            }
        }

        /// <summary>
        /// Gets a connection string that has no Initial Catalog specified
        /// </summary>
        /// <exception cref="Lutron.Gulliver.Infrastructure.Configuration.EnvironmentVariableNotFoundException">
        /// If the caller tries to retrieve the connection string information and the environment 
        /// variable is not set the an exception will be thrown.</exception>
        /// <exception cref="Lutron.Gulliver.Infrastructure.Configuration.ConfigurationInformationMissingException">Thrown if 
        /// the configuration file does not contain the correct key/value pairs</exception>
        /// <exception cref="Lutron.Gulliver.Infrastructure.Configuration.InvalidConfigurationLocationException">Thrown if 
        /// the given location of the configuration file is incorrect.</exception>
        public static string NonDatabaseSpecificConnectionString
        {
            get
            {
                // Need to load the connection string if not already done so.
                if (RWhizzConfiguration.nonDatabaseSpecificConnectionString == null)
                {
                    RWhizzConfiguration.nonDatabaseSpecificConnectionString = GetAppSettingKeyValue(nonDatabaseSpecificConnectionStringKey);
                }

                return RWhizzConfiguration.nonDatabaseSpecificConnectionString;
            }
        }

        /// <summary>
        /// Returns the current database connection string as per the config file
        /// </summary>
        public static string DatabaseConnectionString
        {
            get
            {
                // Need to load the connection string if not already done so.
                if (string.IsNullOrEmpty(RWhizzConfiguration.databaseConnectionString))
                {
                    LoadProjectConnectionString();
                }
                return RWhizzConfiguration.databaseConnectionString;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load connection string
        /// </summary>
        private static void LoadProjectConnectionString()
        {
            RWhizzConfiguration.databaseConnectionString = GetAppSettingKeyValue(DatabaseConnectionKey);
            if (String.IsNullOrEmpty(databaseConnectionString))
            {
                //Show error message
            }
            if (RWhizzConfiguration.DatabaseStorageType == DataStorageType.Firebird)
            {
                RWhizzConfiguration.databaseConnectionString = FirebirdHelper.GetConnectionStringWithRootedAppDataPath(databaseConnectionString);
            }
        }

        /// <summary>
        /// Gets the value for a specified key inside the appSettings element
        /// of the configuration file.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingKeyValue(string key)
        {
            string keyXPathLocation = "/configuration/appSettings/add[@key='" + key + "']";

            //Read node from configuration file.
            XmlNode configurationNode;
            configurationNode = ConfigManager.ConfigurationFile.SelectSingleNode(keyXPathLocation);

            if (configurationNode == null)
            {
                // The required information is missing from the configuration file.
                return null;
            }
            else
            {
                return configurationNode.Attributes["value"].Value;
            }
        }

        /// <summary>
        /// Gets file path with respect to application data.
        /// </summary>
        /// <param name="fileName">file name that is to be converted to application data path.</param>
        /// <returns></returns>
        public static string GetFilePathWRTAppData(string fileName)
        {
            return Path.Combine(GetAppDataPath, fileName);
        }

        #endregion
    }
}
