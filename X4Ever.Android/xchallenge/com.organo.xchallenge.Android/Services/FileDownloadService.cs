
using com.organo.xchallenge.Services;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using com.organo.xchallenge.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileDownloadService))]

namespace com.organo.xchallenge.Droid
{
    public class FileDownloadService : IFileDownloadService
    {
        private WebClient _client;
        private bool retryLoading = false;

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
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await DownloadFileAsync(fileUri, fileName);
                }
            }

            return false;
        }

        public async Task<bool> DownloadFileAsync(string fileUri, string fileName)
        {
            try
            {
                _client.DownloadFile(fileUri,  await GetFileAsync(fileName));
                return true;
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await DownloadFileAsync(fileUri, fileName);
                }
            }

            return false;
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
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await RemoveFileAsync(fileName);
                }
            }

            return removed;
        }
    }
}