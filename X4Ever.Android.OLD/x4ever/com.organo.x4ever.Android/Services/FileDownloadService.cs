
using com.organo.x4ever.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using com.organo.x4ever.Droid;
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
            var filePathCombine = await GetFileAsync(fileName);
            _client.DownloadFileAsync(fileUri, filePathCombine);
            return true;
        }

        public async Task<bool> DownloadFileAsync(string fileUri, string fileName)
        {
            var filePathCombine = await GetFileAsync(fileName);
            _client.DownloadFile(fileUri, filePathCombine);
            return true;
        }

        public string GetFile(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }

        public async Task<string> GetFileAsync(string fileName)
        {
            string filePathCombine = "";
            await Task.Run(() => { filePathCombine = GetFile(fileName); });
            //(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path
            return filePathCombine;
        }

        public async Task<bool> RemoveFileAsync(string fileName)
        {
            var filePathCombine = await GetFileAsync(fileName);
            FileInfo manager = new FileInfo(filePathCombine);
            bool removed = false;
            if (manager.Exists)
            {
                manager.Delete();
                removed = true;
            }

            return removed;
        }
    }
}