using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IFileDownloadService
    {
        Task<bool> DownloadFileAsync(string fileUri, string fileName);
        Task<bool> DownloadFileAsync(Uri fileUri, string fileName);
        string GetFile(string fileName);
        Task<string> GetFileAsync(string fileName);
        Task<bool> RemoveFileAsync(string fileName);
    }
}