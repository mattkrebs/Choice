using ShakrLabs.Mobile.App.Data.Providers.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ShakrLabs.Mobile.App.Data
{
    public class AppConfig : IAppConfig
    {
        /// <summary>
        /// The _current singleton instance
        /// </summary>
        private static AppConfig _current;

        /// <summary>
        /// Prevents a default instance of the <see cref="AppConfig" /> class from being created.
        /// </summary>
        private AppConfig()
        {
        } 

        /// <summary>
        /// Gets the singleton instance of the AppConfig class..
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static IAppConfig Current
        {
            get { return _current ?? (_current = new AppConfig()); }
        }

        private const string AppConfigLocalSourceFileName = "Config/AppConfig.xml";

        public string RootFilePath
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); }
        }

        public string StoragePath
        {
            get { return Path.Combine(RootFilePath, "FileStorage"); }
        }

        //public SerializationFormat ProviderSerializationFormat
        //{
        //    get { return SerializationFormat.JSON; }
        //}

        private static Dictionary<string, string> _appConfigDictionary;

#if MONOTOUCH
        static readonly string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif ANDROID
        private static readonly string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
        public static readonly string AppPath = Environment.CurrentDirectory; // @"C:\Work\BlueCrossBlueShield-LA\MobileApp\BCBS\WindowsTest\WindowsTest";
#endif

        private Func<string, string> _appAssetStringDelegate;

        public void SetAppAssetStringDelegate(Func<string, string> appAssetStringDelegate)
        {
            _appAssetStringDelegate = appAssetStringDelegate;
        }

        public string GetAsset(string uri)
        {
            Console.WriteLine("Get Asset: " + uri);
            return _appAssetStringDelegate(uri);
        }

        public string PublicUriBase
        {
            get { return GetValue("PublicUriBase"); }
        }

        public string MemberUriBase
        {
            get { return GetValue("MemberUriBase"); }
        }

        public string LoginUri
        {
            get { return GetValue("LogInUri"); }
        }

        public static string GetFileAsString(string relativePath)
        {
            string filePath = Path.Combine(AppPath, relativePath);
            string fileText = null;
            if (File.Exists(filePath))
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    fileText = streamReader.ReadToEnd();
                }
            }
            return fileText;
        }

        //private static string AppConfigLocalSourceFilePath
        //{
        //    get
        //    {
        //        return Path.Combine(AppPath, Path.Combine(ConfigFolderName, AppConfigLocalSourceFileName));
        //    }
        //}

        public string GetValue(string key)
        {
            if (_appConfigDictionary == null)
            {
                _appConfigDictionary = LoadAppConfigData();
            }

            if (_appConfigDictionary.ContainsKey(key))
            {
                return _appConfigDictionary[key];
            }
            return null;
        }

        private Dictionary<string, string> LoadAppConfigData()
        {
            var dictionary = new Dictionary<string, string>();
            //var appConfigLocalSourceFilePath = AppConfigLocalSourceFilePath;
            //if (File.Exists(appConfigLocalSourceFilePath))
            //{
            string configXml = GetAsset(AppConfigLocalSourceFileName);
            var parameterList = DeserializeFromXmlString<ParameterList>(configXml);
            foreach (Parameter parameter in parameterList)
            {
                dictionary.Add(parameter.Name, parameter.Value);
            }
            //}
            return dictionary;
        }

        private T DeserializeFromXmlFile<T>(string dataFilePath) where T : class
        {
            if (!File.Exists(dataFilePath))
            {
                return null;
            }

            XDocument loadedData = XDocument.Load(dataFilePath);
            using (XmlReader reader = loadedData.Root.CreateReader())
            {
                var targetObject = (T) new XmlSerializer(typeof (T)).Deserialize(reader);
                return targetObject;
            }
        }

        private static T DeserializeFromXmlString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof (T));
            T resultObject;

            using (TextReader reader = new StringReader(objectData))
            {
                resultObject = (T) serializer.Deserialize(reader);
            }

            return resultObject;
        }


        public void DeleteAllDataFiles()
        {
            string path = StoragePath;
            if (Directory.Exists(path))
            {
                Console.WriteLine("Clear All File Storage: " + path);
                Directory.Delete(path, true);
            }
        }
    }
}