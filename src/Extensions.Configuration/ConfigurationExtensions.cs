using System;
using System.Configuration;
using System.IO;

namespace QuickShop.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Loads a file named {filename}.user.config as a configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static System.Configuration.Configuration LoadConfigurationUserFile(string name)
        {
            string filename = $"{name}.user.config";

            if (!File.Exists(filename))
            {
                throw new InvalidOperationException(
                    $"Could not load user-specific config file, make sure to create {filename} and copy it to the output directory"
                );
            }

            var map = new ExeConfigurationFileMap
            {
                ExeConfigFilename = filename,
            };

            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }
    }
}
