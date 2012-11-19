using System;
using System.Collections.Concurrent;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public class MemoryStorage<T>
    {
        private readonly ConcurrentDictionary<string, T> _memoryStorageDictionary;

        public MemoryStorage(bool enabled)
        {
            if (enabled)
            {
                _memoryStorageDictionary = new ConcurrentDictionary<string, T>();
            }
        }

        public bool Enabled
        {
            get { return _memoryStorageDictionary != null; }
        }

        internal bool ContainsKey(string uri)
        {
            if (Enabled)
            {
                return _memoryStorageDictionary.ContainsKey(uri);
            }
            return false;
        }

        internal void Set(string uri, T deserializedObject)
        {
            if (Enabled)
            {
                if (_memoryStorageDictionary.ContainsKey(uri))
                {
                    T objectToRemove;
                    _memoryStorageDictionary.TryRemove(uri, out objectToRemove);
                    if(objectToRemove == null)
                    {
                        throw new ApplicationException("could not remove memory object from dictionary");
                    }
                }
                Console.WriteLine("Set Object in Memory: " + uri);
                _memoryStorageDictionary.TryAdd(uri, deserializedObject); // ignore if this fails
            }
        }

        internal bool Get(string uri, out T objectFound)
        {
            if (Enabled && _memoryStorageDictionary.ContainsKey(uri))
            {
                //Console.WriteLine("Get Object from Memory: " + uri);
                objectFound = _memoryStorageDictionary[uri];
                return true;
            }
            objectFound = default(T);
            return false;
        }

        internal void Clear()
        {
            if (!Enabled)
            {
                throw new ApplicationException("Attempt to clear a disabled MemoryStorage object");
            }

            if (_memoryStorageDictionary.Count == 0)
            {
                return;
            }

            Console.WriteLine("Clearing " + _memoryStorageDictionary.Count + " objects from memory");
            _memoryStorageDictionary.Clear();
        }
    }
}