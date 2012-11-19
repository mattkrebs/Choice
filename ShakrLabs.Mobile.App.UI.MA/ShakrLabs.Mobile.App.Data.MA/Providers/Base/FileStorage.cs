using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public class FileStorage<T>
    {
        private static readonly object _locker = new object();
        private readonly bool _enabled;
        private readonly bool _encrypted;
        private readonly string _providerStoragePath;

        public bool Enabled
        {
            get { return _enabled; }
        }

        public FileStorage(string providerStorageKey, bool enabled, bool encrypted)
        {
            _enabled = enabled;
			_encrypted = encrypted;
            _providerStoragePath = Path.Combine(AppConfig.Current.StoragePath, providerStorageKey);
        }

        private string GetFilePath(string fileName)
        {
            var path = Path.Combine(_providerStoragePath, fileName) + ".dat";
            return path;
        }

        internal void Set(string fileName, T objectToSet)
        {
            if (!Enabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

            fileName = GetHashKey(fileName);
            var filePath = GetFilePath(fileName);

            if (_encrypted)
            {
                SerializeObjectToFileEncrypted(objectToSet, filePath);
            }
            else
            {
                SerializeObjectToFileUnencrypted(objectToSet, filePath);
            }
        }

        public bool Get(string fileName, out string serializedObjectString)
        {
            if (!Enabled)
            {
                serializedObjectString = null;
                return false;
            }

            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

            fileName = GetHashKey(fileName);

            var filePath = GetFilePath(fileName);
            serializedObjectString = _encrypted ? ReadFromEncryptedFile(filePath) : ReadFromUnencryptedFile(filePath);

            if (String.IsNullOrEmpty(serializedObjectString))
            {
                serializedObjectString = null;
                return false;
            }
            return true;
        }

        private static string GetHashKey(string key)
        {
            return key.GetHashCode().ToString(CultureInfo.InvariantCulture).Replace("-", "0");
        }

        public void Clear(string key)
        {
            if (!Enabled)
            {
                throw new ApplicationException("Attempt to clear a disabled FileStorage object");
            }

            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            lock (_locker)
            {
                File.Delete(GetFilePath(key));
            }
        }

        public void DeleteAllFiles()
        {
            if (!Enabled)
            {
                throw new ApplicationException("Attempt to clear a disabled FileStorage object");
            }

            var path = _providerStoragePath;

            if (!Directory.Exists(path)) return;

            Console.WriteLine("FileStorage.DeleteAllFiles: " + path);
            lock (_locker)
            {
                Directory.Delete(path, true);
            }
        }

        private static void SerializeObjectToFileUnencrypted(T objectToSet, string filePath)
        {
            var directoryPath = DirectoryName(filePath);
            AssurePathExists(directoryPath);
            lock (_locker)
            {
                using (var fileStream = File.Open(filePath, FileMode.Create))
                using (var streamWriter = new StreamWriter(fileStream))
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
#if DEBUG
                    jsonWriter.Formatting = Formatting.Indented;
#else
					jsonWriter.Formatting = Formatting.None;
#endif
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, objectToSet);
                }
            }
        }

        private static void SerializeObjectToFileEncrypted(T objectToSet, string filePath)
        {
            var directoryPath = DirectoryName(filePath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            var serializedObjectString = JsonConvert.SerializeObject(objectToSet);
            var byteArray = AesEncryption.StrToByteArray(serializedObjectString);

            lock (_locker)
            {
                using (var fileStream = File.Open(filePath, FileMode.Create))
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    AesEncryption.EncryptStream(memoryStream, fileStream);
                }
            }
        }

        private void WriteToFile(string fileContent, string filepath)
        {
            var encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(fileContent);

            string directoryPath = DirectoryName(filepath);

            AssurePathExists(directoryPath);

            Console.WriteLine("WriteToFile: " + filepath);
            lock (_locker)
            {
                File.WriteAllBytes(filepath, bytes);
            }
        }

        private static void AssurePathExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath)) return;

            lock (_locker)
            {
                if (!Directory.Exists(directoryPath)) // second check done to avoid threading issue where a second thread recreates the directory after a first thread locked and created it
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        private static string ReadFromEncryptedFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return string.Empty;
            }

            Console.WriteLine("ReadFromEncryptedFile: " + filepath);

            byte[] fileContentsByteArray;
            byte[] decryptedByteArray;
            lock (_locker)
            {
                fileContentsByteArray = File.ReadAllBytes(filepath);
            }
#if DEBUG
            try
            {
#endif
                decryptedByteArray = AesEncryption.DecryptBytes(fileContentsByteArray);
#if DEBUG
            }
            catch (Exception)
            {
				// This failure may be caused by an unencrypted file that was saved that way for testing. Just pass through the bytes unencrypted.
                decryptedByteArray = fileContentsByteArray;
				lock (_locker) // resave the file encrypted
				{
					using (var fileStream = File.Open(filepath, FileMode.Create))
					using (var memoryStream = new MemoryStream(decryptedByteArray))
					{
						AesEncryption.EncryptStream(memoryStream, fileStream);
					}
				}
            }
#endif

            var fileContents = AesEncryption.ByteArrayToStr(decryptedByteArray);
            if (string.IsNullOrEmpty(fileContents))
            {
                Console.WriteLine("ReadFromEncryptedFile: " + filepath + " *WARNING empty file");
                return string.Empty;
            }
            return fileContents;
        }

        private static string ReadFromUnencryptedFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return string.Empty;
            }

            Console.WriteLine("ReadFromUnencryptedFile: " + filepath);
            string fileContents;
            lock (_locker)
            {
                fileContents = File.ReadAllText(filepath);
            }
            if (string.IsNullOrEmpty(fileContents))
            {
                Console.WriteLine("ReadFromUnencryptedFile: " + filepath + " *WARNING empty file");
                return string.Empty;
            }
            return fileContents;
        }

        private static string DirectoryName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            var idx = filename.LastIndexOf(Path.DirectorySeparatorChar);
            return idx > 0 ? filename.Remove(idx) : string.Empty;
        }
    }
}