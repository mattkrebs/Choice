using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public static class AesEncryption
    {
        private const int BUFFER_SIZE = 8192;

        #region Properties

        public static string Key
        {
            get { return @"bsGV=yB<PSzDd/u|dr#k}}i2Ya) `Qh-]>7Kc3I[Npf^XrF<MF%2&4 [+aq4a5(v')"; }
        }

        public static byte[] Salt
        {
            get
            {
                Console.WriteLine("TODO: get salt unique to the device");
                return StrToByteArray("TODO: get salt unique to the device");
            }
        }
        #endregion


        private static AesManaged GetAesManaged(string aesKey, byte[] salt)
        {
            if (string.IsNullOrEmpty(aesKey))
                throw new ArgumentNullException("aesKey");
            if (salt == null)
                throw new ArgumentNullException("salt");

            var rdb = new Rfc2898DeriveBytes(aesKey, salt);
            byte[] key = rdb.GetBytes(32);
            byte[] iv = rdb.GetBytes(16);

            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            return new AesManaged {Key = key, IV = iv};
        }


        #region String Encryption/Decryption Methods

        public static string EncryptString(string txt)
        {
            return EncryptString(txt, Key, Salt);
        }

        public static string EncryptString(string txt, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(txt))
                return string.Empty;

            byte[] plain = StrToByteArray(txt);
            byte[] encrypted = EncryptBytes(plain, key, salt);

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        public static byte[] StrToByteArray(string str)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string DecryptString(string cipher)
        {
            return DecryptString(cipher, Key, Salt);
        }

        public static string DecryptString(string cipher, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipher))
                return string.Empty;

            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] decrypted = DecryptBytes(cipherBytes, key, salt);

            return ByteArrayToStr(decrypted);
        }

        public static string ByteArrayToStr(byte[] byteData)
        {
            try
            {
                Encoding enc = Encoding.GetEncoding("utf-8");
                return enc.GetString(byteData, 0, byteData.Length);
            }
            catch
            {
                // swallow exception if cannot convert to UTF8 string.
            }

            return null;
        }

        #endregion

        #region Byte Encryption/Decryption Methods

        public static byte[] EncryptBytes(byte[] contents)
        {
            return EncryptBytes(contents, Key, Salt);
        }

        public static byte[] EncryptBytes(byte[] contents, string key, byte[] salt)
        {
            var msInput = new MemoryStream(contents);
            var msOutput = new MemoryStream();

            EncryptStream(msInput, msOutput, key, salt);

            byte[] retArray = msOutput.ToArray();
            return retArray;
        }

        public static byte[] DecryptBytes(byte[] cipher)
        {
            return DecryptBytes(cipher, Key, Salt);
        }

        public static byte[] DecryptBytes(byte[] cipher, string key, byte[] salt)
        {
            var msInput = new MemoryStream(cipher);
            var msOutput = new MemoryStream();

            DecryptStream(msInput, msOutput, key, salt);

            byte[] retArray = msOutput.ToArray();
            return retArray;
        }

        #endregion

        #region Stream Encryption/Decryption Methods

        public static void EncryptStream(Stream inputStream, Stream outputStream)
        {
            EncryptStream(inputStream, outputStream, Key, Salt);
        }

        //public abstract void EncryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt);

        public static void DecryptStream(Stream inputStream, Stream outputStream)
        {
            DecryptStream(inputStream, outputStream, Key, Salt);
        }

        //public abstract void DecryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt);

        #endregion

        #region Stream Encryption/Decryption Methods

        public static void EncryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt)
        {
            //DateTime dtMetric = DateTime.UtcNow;

            AesManaged aesAlg = null;

            // Create the streams used for encryption.
            CryptoStream crypto = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;
            GZipStream gzip = null;
            try
            {
                aesAlg = GetAesManaged(key, salt);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                binaryReader = new BinaryReader(inputStream);

                crypto = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
                gzip = new GZipStream(crypto, CompressionMode.Compress);

                binaryWriter = new BinaryWriter(gzip);

                // process through stream in small chunks to keep peak memory usage down.
                byte[] bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                while (bytes.Length > 0)
                {
                    binaryWriter.Write(bytes);
                    bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                }
            }
            finally
            {
                if (binaryWriter != null)
                    binaryWriter.Close();
                if (gzip != null)
                    gzip.Close();
                if (crypto != null)
                    crypto.Close();

                if (binaryReader != null)
                    binaryReader.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            //MXDevice.Log.Metric( string.Format( "AesEncryption.EncryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );
        }

        public static void DecryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt)
        {
            //DateTime dtMetric = DateTime.UtcNow;

            AesManaged aesAlg = null;
            CryptoStream crypto = null;
            GZipStream gzip = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;

            try
            {
                aesAlg = GetAesManaged(key, salt);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                crypto = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
                gzip = new GZipStream(crypto, CompressionMode.Decompress);
                binaryReader = new BinaryReader(gzip);
                binaryWriter = new BinaryWriter(outputStream);

                // process through stream in small chunks to keep peak memory usage down.
                byte[] bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                while (bytes.Length > 0)
                {
                    binaryWriter.Write(bytes);
                    bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                }
            }
            finally
            {
                if (binaryWriter != null)
                    binaryWriter.Close();
                if (gzip != null)
                    gzip.Close();
                if (crypto != null)
                    crypto.Close();

                if (binaryReader != null)
                    binaryReader.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            //MXDevice.Log.Metric( string.Format( "AesEncryption.DecryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );
        }

        #endregion
    }
}
