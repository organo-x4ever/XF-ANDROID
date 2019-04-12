
using com.organo.xchallenge.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using com.organo.xchallenge.Droid;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileDownloadService))]

namespace com.organo.xchallenge.Droid
{
    public class FileDownloadService : IFileDownloadService
    {
        private WebClient _client;

        public FileDownloadService()
        {
            _client = new WebClient();
        }

        public async Task<bool> DownloadFileAsync(Uri fileUri, string fileName)
        {
            try
            {
                _client.DownloadFileAsync(fileUri, await GetFileAsync(fileName));
                return true;
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(FileDownloadService).FullName, ex);
            }

            return false;
        }

        public async Task<bool> DownloadFileAsync(string fileUri, string fileName)
        {
            try
            {
                _client.DownloadFile(fileUri, await GetFileAsync(fileName));
                return true;
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(FileDownloadService).FullName, ex);
            }

            return false;
        }

        public string GetFile(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }

        public async Task<string> GetFileAsync(string fileName)
        {
            return await Task.Factory.StartNew(() => GetFile(fileName) ?? "");
        }

        public async Task<bool> RemoveFileAsync(string fileName)
        {
            var filePathCombine = await GetFileAsync(fileName);
            bool removed = false;
            try
            {
                FileInfo manager = new FileInfo(filePathCombine);
                if (manager.Exists)
                {
                    manager.Delete();
                    removed = true;
                }
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(FileDownloadService).FullName, ex);
            }

            return removed;
        }
    }
}