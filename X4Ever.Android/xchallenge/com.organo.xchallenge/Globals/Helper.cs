using System;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Helper))]

namespace com.organo.xchallenge.Globals
{
    public class Helper : IHelper
    {
        private IMessage message;

        public Helper()
        {
            message = DependencyService.Get<IMessage>();
        }

        public string GetResource(string resourceKey)
        {
            return message.GetResource(resourceKey);
        }

        public async Task DisplayMessage(string result)
        {
            string msg = "";
            try
            {
                var model = JsonConvert.DeserializeObject<ReturningMessage>(result);
                if (model != null)
                {
                    string[] messages = model.Message.Split(';');
                    foreach (var m in messages)
                    {
                        msg += message.GetResource(m) + " \n";
                    }
                }
            }
            catch
            {
                var replaced = result.Replace("\"", "");
                msg = replaced;
            }

            if (!string.IsNullOrEmpty(msg))
                await DisplayMessageOnly(msg);
        }

        public async Task DisplayMessage(string result, char splitBy)
        {
            string msg = "";
            string[] messages = result.Split(splitBy);
            foreach (var m in messages)
            {
                try
                {
                    msg += message.GetResource(m) + " \n";
                }
                catch
                {
                    msg = m;
                }
            }

            if (!string.IsNullOrEmpty(msg))
                await DisplayMessageOnly(msg);
        }

        public async Task DisplayMessage(string title, string msg, string action)
        {
            await message.AlertAsync(title, msg, action);
        }

        public async Task DisplayMessageOnly(string msg)
        {
            await message.AlertAsync(message.GetResource("Review"), msg, message.GetResource("Ok"));
        }

        public string ReturnMessage(string result)
        {
            string msg = "";
            try
            {
                var model = JsonConvert.DeserializeObject<ReturningMessage>(result);
                if (model != null)
                {
                    string[] messages = model.Message.Split(';');
                    foreach (var m in messages)
                    {
                        if (!string.IsNullOrEmpty(m) && !string.IsNullOrWhiteSpace(m))
                        {
                            var ms = message.GetResource(m);
                            if (string.IsNullOrEmpty(ms))
                                msg += m + " \n";
                            else
                                msg += message.GetResource(m) + " \n";
                        }
                        else
                            msg += m + " \n";
                    }
                }
            }
            catch
            {
                var replaced = result.Replace("\"", "");
                var resource = message.GetResource(replaced);
                msg = resource ?? replaced;
            }

            return msg;
        }

        public async Task<string> DatetimeStampAsync()
        {
            return await Task.Factory.StartNew(() => DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        public string GetFilePath(string fileName, FileType fileType)
        {
            // Check file name exists
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            // Check App Configuration object exists
            if (App.Configuration == null || App.Configuration.AppConfig == null) return string.Empty;

            // Domain name checking and getting
            var domain = App.Configuration.AppConfig.BaseUrl;
            if (domain == null) return string.Empty;

            var filePath = "";
            switch (fileType)
            {
                case FileType.Image:
                    filePath = domain + App.Configuration.AppConfig.UserImageUrl;
                    break;
                case FileType.Audio:
                    filePath = domain + App.Configuration.AppConfig.AudioUrl;
                    break;

                case FileType.Upload:
                    filePath = domain + App.Configuration.AppConfig.FileUploadUrl;
                    break;

                case FileType.User:
                    filePath = domain;
                    break;

                case FileType.Document:
                    filePath = domain + App.Configuration.AppConfig.DocumentUrl;
                    break;

                case FileType.TestimonialPhoto:
                    filePath = domain + App.Configuration.AppConfig.TestimonialPhotoUrl;
                    break;

                case FileType.TestimonialVideo:
                    filePath = domain + App.Configuration.AppConfig.TestimonialVideoUrl;
                    break;

                case FileType.Video:
                    filePath = domain + App.Configuration.AppConfig.VideoUrl;
                    break;
                case FileType.Resources:
                    filePath = "Resources/";
                    break;
                case FileType.None:
                    filePath = "";
                    break;
                default:
                    filePath = "";
                    break;
            }

            return fileType == FileType.None
                ? fileName
                : (fileType == FileType.Resources ? filePath + fileName : filePath.Replace("\\", "/") + "/" + fileName);
        }

        public UriImageSource GetFileUri(string fileName, FileType fileType)
        {
            var filePath = "";
            if (fileName != null && fileName.Trim().Length > 0 && !fileName.Trim().Contains("null"))
                filePath = GetFilePath(fileName, fileType);
            return new UriImageSource
            {
                Uri = new Uri(filePath),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(30, 0, 0, 0)
            };
        }

        public ImageSource GetFileSource(string fileName, FileType fileType)
        {
            var filePath = "";
            if (fileName != null && fileName.Trim().Length > 0 && !fileName.Trim().Contains("null"))
                filePath = GetFilePath(fileName, fileType);
            return ImageSource.FromFile(filePath);
        }

        public object GetFileObject(string fileName, FileType fileType)
        {
            var filePath = "";
            if (fileName != null && fileName.Trim().Length > 0 && !fileName.Trim().Contains("null"))
                filePath = GetFilePath(fileName, fileType);
            else
                return ImageSource.FromFile(filePath);
            var timeSpan = new TimeSpan(30, 0, 0, 0);
            switch (fileType)
            {
                case FileType.Image:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };
                case FileType.Upload:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };
                case FileType.Audio:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };
                case FileType.User:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };
                case FileType.Video:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };

                case FileType.Document:
                    return new UriImageSource
                    {
                        Uri = new Uri(filePath),
                        CachingEnabled = true,
                        CacheValidity = timeSpan
                    };

                case FileType.Resources:
                    return ImageSource.FromFile(filePath);
                case FileType.None:
                    return ImageSource.FromFile(filePath);
                default:
                    return ImageSource.FromFile(filePath);
            }
        }

        public string Encrypt(string input, string key = "")
        {
            return input;
        }

        public string Decrypt(string input, string key = "")
        {
            return input;
        }
    }
}