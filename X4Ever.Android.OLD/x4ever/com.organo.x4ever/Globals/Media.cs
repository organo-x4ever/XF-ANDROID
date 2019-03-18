using com.organo.x4ever.Localization;
using com.organo.x4ever.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(com.organo.x4ever.Globals.Media))]

namespace com.organo.x4ever.Globals
{
    public class Media : IMedia
    {
        private IFileService fileService;

        public Media()
        {
            fileService = DependencyService.Get<IFileService>();
            Refresh();
        }

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
                    var mediaFile = await CrossMedia.Current.PickPhotoAsync();
                    if (mediaFile != null)
                        return mediaFile;
                }
            }
            catch (Exception)
            {
                Message = TextResources.MessageInvalidObject;
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
                        new StoreCameraMediaOptions() {Name = imageFileName, SaveToAlbum = false});
                    if (mediaFile != null)
                        return mediaFile;
                }
            }
            catch (Exception)
            {
                Message = TextResources.MessageInvalidObject;
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
            var response = await fileService.UploadFileAsync(mediaFile);
            if (response != null)
                if (response.Contains("Success#"))
                {
                    var splits = response.Split('#');
                    FileName = splits[1];
                    int lastIndex = 0;
                    lastIndex = FileName.LastIndexOf('"');
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);

                    lastIndex = FileName.LastIndexOf("\"");
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
            return await fileService.UploadFileResponseAsync(mediaFile);
        }

        public async Task<bool> UploadPhotoAsync(MediaFile mediaFile)
        {
            Refresh();
            var response = await fileService.UploadFileResponseAsync(mediaFile);
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    FileName = await response.Content.ReadAsStringAsync();
                    int lastIndex = 0;
                    lastIndex = FileName.LastIndexOf('"');
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);

                    lastIndex = FileName.LastIndexOf("\"");
                    if (lastIndex != -1)
                        FileName = FileName.Remove(lastIndex, 1);
                    return true;
                }

                Message = await response.Content.ReadAsStringAsync();
            }
            else
                Message = TextResources.MessageFileUploadFailed;

            return false;
        }
    }
}