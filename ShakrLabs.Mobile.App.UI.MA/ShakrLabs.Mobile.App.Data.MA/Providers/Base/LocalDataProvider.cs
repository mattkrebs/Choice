using System;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public class LocalDataProvider<TDataObject> where TDataObject : class, new()
    {
        #region Variables & Properties

        private readonly FileStorage<TDataObject> _fileStorage;
        private readonly MemoryStorage<TDataObject> _memoryStorage;
        private readonly string _providerInstanceUri; //uniquely identifies an instance of a data provider of any type.

        #endregion

        public LocalDataProvider(string providerInstanceUri, bool memoryStorageEnabled, bool fileStorageEnabled, bool fileEncrypted = true)
        {
            _providerInstanceUri = providerInstanceUri;
            _memoryStorage = new MemoryStorage<TDataObject>(memoryStorageEnabled);
            _fileStorage = new FileStorage<TDataObject>(providerInstanceUri, fileStorageEnabled, fileEncrypted);
        }

        #region Methods

        #region Set Methods

        public void SetObject(TDataObject objectToSet)
        {
            _memoryStorage.Set(_providerInstanceUri, objectToSet);
            _fileStorage.Set(_providerInstanceUri, objectToSet);
        }

        public void SetObject(string dataKey, TDataObject objectToSet)
        {
            string dataObjectUri = GetDataObjectUri(dataKey);

            _memoryStorage.Set(dataObjectUri, objectToSet);
            _fileStorage.Set(dataObjectUri, objectToSet);
        }

        #endregion

        #region Get Methods

        public virtual TDataObject GetObject()
        {
            return GetObject(null);
        }

        public bool CheckObjectInMemory(string dataKey)
        {
            // Local Key
            string dataObjectUri = GetDataObjectUri(dataKey);

            // Memory and File
            return _memoryStorage.ContainsKey(dataObjectUri);
        }

        public TDataObject GetObject(string dataKey)
        {
            var dataObjectUri = GetDataObjectUri(dataKey);

            // Memory
            TDataObject deserializedObject;
            if (_memoryStorage.Get(dataObjectUri, out deserializedObject))
            {
                return deserializedObject;
            }

            // File
            string serializedObjectString;

            if (_fileStorage.Get(dataObjectUri, out serializedObjectString))
            {
                deserializedObject = Deserialize(serializedObjectString);
                if (deserializedObject == null)
                {
                    Console.WriteLine("GetObject: " + _providerInstanceUri + " *WARNING deserialization failed");
                }
                else
                {
                    _memoryStorage.Set(dataObjectUri, deserializedObject);
                    return deserializedObject;
                }
            }

            return null;
        }

        // uniquely identifies a provider instance and object within it
        private string GetDataObjectUri(string dataKey)
        {
            string dataObjectUri = _providerInstanceUri + dataKey;
            if (string.IsNullOrEmpty(dataObjectUri))
            {
                throw new ApplicationException("Attempt to access null provider URI");
            }

            return dataObjectUri;
        }

        #endregion

        public void Clear()
        {
            // Memory
            _memoryStorage.Clear();

            // File
            _fileStorage.DeleteAllFiles();
        }

        private static TDataObject Deserialize(string serializedObjectString)
        {
            return Serialization.DeserializeObject<TDataObject>(serializedObjectString);
        }

        #endregion
    }
}