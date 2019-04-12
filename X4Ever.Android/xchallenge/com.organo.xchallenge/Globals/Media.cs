using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(com.organo.xchallenge.Globals.Media))]

namespace com.organo.xchallenge.Globals
{
    public class Media : IMedia
    {
        private readonly IFileService _fileService;
        private readonly IHelper _helper;

        public Media()
        {
            _fileService = DependencyService.Get<IFileService>();
            _helper = DependencyService.Get<IHelper>();
            Refresh();
        }

        private string AlbumName => DependencyService.Get<IDeviceInfo>().GetAppName;
        public string Message { get; set; }
        public string FileName { get; set; }

        public async Task<MediaFile> PickPhotoAsync()
        {
            try
            {
                Refresh();
                await CrossMedia.Current.Initialize();
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Custom,
                        CustomPhotoSize = 60,
                        CompressionQuality = 80,
                        MaxWidthHeight = 1024
                    });
                    if (mediaFile != null)
                        return mediaFile;
                }
                else
                {
                    var exceptionHandler = new ExceptionHandler(typeof(Media).FullName + ".PickPhotoAsync()", "Pick Photo Not Supported");
                }
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(Media).FullName + ".PickPhotoAsync()", ex);
            }

            Message = TextResources.NoPickPhotoAvailable;
            return null;
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            try
            {
                Refresh();
                await CrossMedia.Current.Initialize();
                if (CrossMedia.Current.IsTakePhotoSupported)
                {
                    var timeStamp = await DependencyService.Get<IHelper>().DatetimeStampAsync();
                    var imageFileName = "android_" + timeStamp + "_" + ".jpg";
                    var mediaFile = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions()
                        {
                            Directory = AlbumName,
                            Name = imageFileName,
                            SaveToAlbum = true,
                            PhotoSize = PhotoSize.Custom,
                            CustomPhotoSize = 60,
                            CompressionQuality = 80,
                            AllowCropping = true,
                            DefaultCamera = CameraDevice.Front,
                            MaxWidthHeight = 1024
                        });
                    if (mediaFile != null)
                        return mediaFile;
                }
                else
                {
                    var exceptionHandler = new ExceptionHandler(typeof(Media).FullName + ".TakePhotoAsync()", "Take Photo Not Supported ");
                }
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(Media).FullName + ".TakePhotoAsync()", ex);
            }

            Message = TextResources.NoPickPhotoAvailable;
            return null;
        }

        public void Refresh()
        {
            Message = string.Empty;
            FileName = string.Empty;
        }

        public async Task<MediaFile> UploadPhotoAsync(MediaFile mediaFile, bool takenPhoto = false)
        {
            Refresh();
            var response = await _fileService.UploadFileAsync(mediaFile);
            if (response != null)
                if (response.Contains("Success#"))
                {
                    var splits = response.Split('#');
                    FileName = splits[1];
                    var lastIndex = FileName.LastIndexOf('"');
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);

                    lastIndex = FileName.LastIndexOf("\"", StringComparison.Ordinal);
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);
                }
                else
                    Message = TextResources.NoPickPhotoAvailable;
            else
                Message = TextResources.MessageInvalidObject;

            return mediaFile;
        }

        public async Task<HttpResponseMessage> UploadPhotoResponseAsync(MediaFile mediaFile)
        {
            Refresh();
            return await _fileService.UploadFileResponseAsync(mediaFile);
        }

        public async Task<bool> UploadPhotoAsync(MediaFile mediaFile)
        {
            Refresh();
            var response = await _fileService.UploadFileResponseAsync(mediaFile);
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FileName = await response.Content.ReadAsStringAsync();
                    var lastIndex = FileName.LastIndexOf('"');
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);

                    lastIndex = FileName.LastIndexOf("\"", StringComparison.Ordinal);
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);
                    return true;
                }

                var returnedMessage = await response.Content.ReadAsStringAsync();
                Message = returnedMessage.Contains("#")
                    ? (returnedMessage.Split('#'))[1]
                    : _helper.GetResource(returnedMessage);
            }
            else
                Message = TextResources.MessageFileUploadFailed;

            return false;
        }
    }
}