using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BrainWhizzDatabaseFramework
{
    public class ConfigurationManager
    {
        #region Private Fields

        /// <summary>
        /// Stores a copy of the configuration file.
        /// </summary>
        private XmlDocument configurationFile;

        /// <summary>
        /// configuration file status
        /// </summary>
        private ConfigFileStatus configFileStatus = ConfigFileStatus.Unknown;

        /// <summary>
        /// configuration file path
        /// </summary>
        private string configFilePath = string.Empty;

        #endregion
        /// <summary>
        /// Returns the name of the config file
        /// </summary>
        public string ConfigurationFileName
        {
            get
            {
                return "RWhizz.Configuration.config";
            }
        }

        /// <summary>
        /// Get xml configuration file
        /// </summary>
        public XmlDocument ConfigurationFile
        {
            get
            {
                if (configurationFile == null)
                {
                    LoadConfigurationFile();
                }

                return configurationFile;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        /// <summary>
        /// Get configuration file path
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public ConfigFileStatus GetConfigurationFilePath(out string configPath)
        {
            string configurationLocation = string.Empty;
            ConfigFileStatus fileStatus = ConfigFileStatus.FileNotFound;

            configurationLocation = Path.Combine(Environment.CurrentDirectory, ConfigurationFileName);

            if (!string.IsNullOrEmpty(configurationLocation) && File.Exists(configurationLocation))
            {
                fileStatus = ConfigFileStatus.FileOk;
            }

            configPath = fileStatus == ConfigFileStatus.FileOk ? configurationLocation : string.Empty;

            return fileStatus;
        }

        /// <summary>
        /// Load an xml document,if it is valid. Otherwise return NULL
        /// </summary>
        protected XmlDocument ValidXML(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
                return xmlDoc;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to load the configuration file from the load location.
        /// </summary>
        public void LoadConfigurationFile()
        {
            this.configFileStatus = GetConfigurationFilePath(out this.configFilePath);

            if (ConfigFileStatus.FileNotFound == configFileStatus)
            {
                throw new Exception(configFilePath);
            }
            this.configurationFile = ValidXML(this.configFilePath);
            string fileName = Path.GetFileName(this.configFilePath);
            //Check config file for valid XML
            if (configurationFile == null)
            {
                throw new Exception(fileName);
            }
        }
    }
}
