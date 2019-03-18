using Android.Content;
using Android.OS;
using Android.Widget;
using com.organo.x4ever.Droid.Services;
using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Java.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using com.organo.x4ever.Extensions;
using Java.Net;
using Xamarin.Forms;
using File = Java.IO.File;


[assembly: Dependency(typeof(LocalFile))]

namespace com.organo.xchallenge.Droid.Services
{
    public class LocalFile : ILocalFile
    {
        private List<string> songs = new List<string>();
        public List<FileDetail> Files { get; set; }
        //private Context Context => (Forms.Context as MainActivity).Window.Context;
        public List<string> Messages { get; set; }
        private string[] extensions, exludeDirs;
        private File[] files;

        public LocalFile()
        {
            Files = new List<FileDetail>();
            extensions = new string[]
            {
                "WAV",
                "MP3"
            };
            exludeDirs = new[]
            {
                "android",
                "android/data",
                "android/framework",
                "dcim"
            };
            files = new File[2];
            files[0] = new File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic));
            files[1] = Environment.GetExternalStoragePublicDirectory("");
        }

        public async Task<List<FileDetail>> UpdatePlayListAsync()
        {
            Files = new List<FileDetail>();
            Messages = new List<string>();
            Messages.Add("###Playlist");
            foreach (var home in files)
            {
                Messages.Add("#MAIN " + home.AbsolutePath);
                var tempFiles = (await home.ListFilesAsync());
                Messages.Add("#DIR-COUNT " + tempFiles?.Length);
                if (tempFiles != null && tempFiles?.Length > 0)
                {
                    foreach (var file in tempFiles)
                    {
                        if (file.AbsolutePath == null || IsExists(file.AbsolutePath))
                        {
                        }
                        else if (file.IsDirectory)
                            await SubDirectories(file);
                        else if (file.Name.IsExtension(extensions))
                            AddFile(file);
                        else
                            Messages.Add("#FILE " + file.AbsolutePath);
                    }
                }
            }

            return Files;
        }

        private bool IsExists(string filePath)
        {
            foreach (var dir in exludeDirs)
            {
                if (filePath.ToLower().Trim().Contains(dir.ToLower().Trim()))
                    return true;
            }

            return false;
        }

        private async Task SubDirectories(File home)
        {
            Messages.Add("#DIR " + home?.AbsolutePath);
            try
            {
                var tempFiles = (await home.ListFilesAsync());
                Messages.Add("#DIR-COUNT " + tempFiles?.Length);
                if (tempFiles != null && tempFiles?.Length > 0)
                {
                    foreach (var file in tempFiles)
                    {
                        if (file.AbsolutePath == null || IsExists(file.AbsolutePath))
                        {
                        }
                        else if (file.IsDirectory)
                            await SubDirectories(file);
                        else if (file.Name.IsExtension(extensions))
                            AddFile(file);
                        else
                            Messages.Add("#FILE " + file.AbsolutePath);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messages.Add("E. " + ex.Message);
            }
        }

        private void AddFile(File file)
        {
            Messages.Add("#FILE " + file?.AbsolutePath);
            if (file != null)
                Files.Add(new FileDetail()
                {
                    Name = file.Name,
                    Path = file.AbsolutePath,
                    IsDirectory = file.IsDirectory,
                    IsFile = file.IsFile,
                    Parent = file.Parent
                });
        }
    }
}