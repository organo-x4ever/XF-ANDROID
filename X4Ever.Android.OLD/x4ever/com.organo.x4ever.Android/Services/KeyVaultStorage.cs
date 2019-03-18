using com.organo.x4ever.Droid.Services;
using com.organo.x4ever.Services;
using Java.Lang;
using Java.Security;
using Javax.Crypto;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using com.organo.x4ever.Statics;
using Exception = System.Exception;

[assembly: Xamarin.Forms.Dependency(typeof(KeyVaultStorage))]

namespace com.organo.xchallenge.Droid.Services
{
    /// <summary>
    /// Implementation of <see cref="ISecureStorage" /> using Android KeyStore.
    /// </summary>
    public class KeyVaultStorage : ISecureStorage
    {
        private static IsolatedStorageFile File
        {
            get { return IsolatedStorageFile.GetUserStoreForApplication(); }
        }

        private static readonly object SaveLock = new object();

        private const string StorageFile = "com.organo.x4ever.Droid.Services.KeyVaultStorage";

        private readonly KeyStore _keyStore;
        private readonly KeyStore.PasswordProtection _protection;

        private static char[] Password => new char[] {'^', '&', '$', '@', '^', '^'};
        private byte[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultStorage" /> class.
        /// </summary>
        public KeyVaultStorage()
        {
            try
            {
                this._keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
                this._protection = new KeyStore.PasswordProtection(Password);

                if (File.FileExists(StorageFile))
                {
                    using (var stream =
                        new IsolatedStorageFileStream(StorageFile, FileMode.Open, FileAccess.Read, File))
                    {
                        this._keyStore.Load(stream, Password);
                    }
                }
                else
                {
                    this._keyStore.Load(null, Password);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Remote(ex.ToString());
            }
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="KeyVaultStorage"/> class.
        ///// </summary>
        ///// <param name="password">Password to use for encryption.</param>
        //public KeyVaultStorage(char[] password)
        //{
        //    this._keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
        //    this._protection = new KeyStore.PasswordProtection(password);

        //    if (File.FileExists(StorageFile))
        //    {
        //        using (var stream = new IsolatedStorageFileStream(StorageFile, FileMode.Open, FileAccess.Read, File))
        //        {
        //            this._keyStore.Load(stream, password);
        //        }
        //    }
        //    else
        //    {
        //        this._keyStore.Load(null, password);
        //    }
        //}

        #region ISecureStorage Members

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">
        /// Key for the data.
        /// </param>
        /// <param name="dataBytes">
        /// Data bytes to store.
        /// </param>
        public void Store(string key, byte[] dataBytes)
        {
            try
            {
                this._keyStore.SetEntry(key, new KeyStore.SecretKeyEntry(new SecureData(dataBytes)), this._protection);
                Save();
            }
            catch (Exception ex)
            {
                WriteLog.Remote(ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">
        /// Key for the data.
        /// </param>
        /// <returns>
        /// Byte array of stored data.
        /// </returns>
        public byte[] Retrieve(string key)
        {
            try
            {
                var entry = this._keyStore.GetEntry(key, this._protection) as KeyStore.SecretKeyEntry;

                if (entry != null)
                    return entry.SecretKey.GetEncoded();
            }
            catch (Exception ex)
            {
                WriteLog.Remote(ex.ToString());
            }

            return null;
            //throw new Exception(string.Format("No entry found for key {0}.", key));
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">
        /// Key for the data to be deleted.
        /// </param>
        public void Delete(string key)
        {
            try
            {
                this._keyStore.DeleteEntry(key);
                Save();
            }
            catch (Exception ex)
            {
                WriteLog.Remote(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">
        /// The key to search.
        /// </param>
        /// <returns>
        /// True if the storage has the key, otherwise false.
        /// </returns>
        public bool Contains(string key)
        {
            try
            {
                return this._keyStore.ContainsAlias(key);
            }
            catch (Exception ex)
            {
                WriteLog.Remote(ex.ToString());
            }

            return false;
        }

        #endregion ISecureStorage Members

        #region private methods

        private void Save()
        {
            lock (SaveLock)
            {
                using (var stream =
                    new IsolatedStorageFileStream(StorageFile, FileMode.OpenOrCreate, FileAccess.Write, File))
                {
                    this._keyStore.Store(stream, this._protection.GetPassword());
                }
            }
        }

        public async Task<byte[]> RetrieveAsync(string key)
        {
            await Task.Run(() => { data = Retrieve(key); });
            return data;
        }

        public async Task StoreAsync(string key, byte[] dataBytes)
        {
            await Task.Run(() => { Store(key, dataBytes); });
        }

        #endregion private methods

        #region Nested Types

        private class SecureData : Object, ISecretKey
        {
            private const string Raw = "RAW";

            private readonly byte[] data;

            public SecureData(byte[] dataBytes)
            {
                this.data = dataBytes;
            }

            #region IKey Members

            public string Algorithm
            {
                get { return Raw; }
            }

            public string Format
            {
                get { return Raw; }
            }

            public byte[] GetEncoded()
            {
                return this.data;
            }

            #endregion IKey Members
        }

        #endregion Nested Types
    }
}