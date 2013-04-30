using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace Choice.Core
{
    public abstract class ImageDownloader
    {
        readonly IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

        readonly ThrottledHttp http;

        readonly TimeSpan cacheDuration;

        public ImageDownloader(int maxConcurrentDownloads = 2)
            : this(TimeSpan.FromDays(1))
        {
            http = new ThrottledHttp(maxConcurrentDownloads);
        }

        public ImageDownloader(TimeSpan cacheDuration)
        {
            this.cacheDuration = cacheDuration;

            if (!store.DirectoryExists("ImageCache"))
            {
                store.CreateDirectory("ImageCache");
            }
        }

        public bool HasLocallyCachedCopy(Uri uri)
        {
            var now = DateTime.UtcNow;

            var filename = Uri.EscapeDataString(uri.AbsoluteUri);

            var lastWriteTime = GetLastWriteTimeUtc(filename);

            return lastWriteTime.HasValue &&
                (now - lastWriteTime.Value) < cacheDuration;
        }

        public Task<object> GetImageAsync(Uri uri)
        {
            return Task.Factory.StartNew(() =>
            {
                return GetImage(uri);
            });
        }

        public object GetImage(Uri uri)
        {
            var filename = Uri.EscapeDataString(uri.AbsoluteUri);

            if (HasLocallyCachedCopy(uri))
            {
                using (var o = OpenStorage(filename, FileMode.Open))
                {
                    return LoadImage(o);
                }
            }
            else
            {
                using (var d = http.Get(uri))
                {
                    using (var o = OpenStorage(filename, FileMode.Create))
                    {
                        d.CopyTo(o);
                    }
                }
                using (var o = OpenStorage(filename, FileMode.Open))
                {
                    return LoadImage(o);
                }
            }
        }

        protected virtual DateTime? GetLastWriteTimeUtc(string fileName)
        {
            var path = Path.Combine("ImageCache", fileName);
            if (store.FileExists(path))
            {
                return store.GetLastWriteTime(path).UtcDateTime;
            }
            else
            {
                return null;
            }
        }

        protected virtual Stream OpenStorage(string fileName, FileMode mode)
        {
            return store.OpenFile(Path.Combine("ImageCache", fileName), mode);
        }

        protected abstract object LoadImage(Stream stream);
    }




    /// <summary>
    /// This class only allows a specific number of WebRequests to execute
    /// simultaneously.
    /// </summary>
    public class ThrottledHttp
    {
        Semaphore throttle;

        public ThrottledHttp(int maxConcurrent)
        {
            throttle = new Semaphore(maxConcurrent, maxConcurrent);
        }

        /// <summary>
        /// Get the specified resource. Blocks the thread until
        /// the throttling logic allows it to execute.
        /// </summary>
        /// <param name='uri'>
        /// The URI of the resource to get.
        /// </param>
        public Stream Get(Uri uri)
        {
            throttle.WaitOne();

            var req = WebRequest.Create(uri);

            var getTask = Task.Factory.FromAsync<WebResponse>(
                req.BeginGetResponse, req.EndGetResponse, null);

            return getTask.ContinueWith(task =>
            {
                throttle.Release();
                var res = task.Result;
                return res.GetResponseStream();
            }).Result;
        }
    }

}