//using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.organo.xchallenge.Droid;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statistic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly:Dependency(typeof(StatisticServices))]

namespace com.organo.xchallenge.Droid
{
    public class StatisticServices : IStatisticServices
    {
        private const string TAG = nameof(StatisticServices);
        private const string FILE_NAME = "statistics.log";
        private readonly IDevicePermissionServices _devicePermissionServices;
        private readonly IHelper _helper;

        public StatisticServices()
        {
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            _helper = DependencyService.Get<IHelper>();
            Allow();
            IsPermitted = true;
        }

        private bool _isAllowed;
        public bool IsAllowed
        {
            get => _isAllowed;
            set => _isAllowed = value;
        }

        private List<string> _messages;
        public List<string> Messages
        {
            get => _messages;
            set => _messages = value;
        }

        private bool _isPermitted;
        public bool IsPermitted
        {
            get => _isPermitted;
            set => _isPermitted = value;
        }

        public async void Allow()
        {
            IsAllowed = true;
            if (!await _devicePermissionServices.RequestReadStoragePermission())
            {
                IsAllowed = false;
                Messages.Add(_helper.GetResource("MessagePermissionReadStorageRequired"));
            }

            if (!await _devicePermissionServices.RequestWriteStoragePermission())
            {
                IsAllowed = false;
                Messages.Add(_helper.GetResource("MessagePermissionCameraRequired"));
            }
        }

        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public async Task Save(string page, string text)
        {
            if (!IsAllowed && !IsPermitted) return;
            var statisticsList = await ReadAsync(page) ?? new List<StatisticModel>();
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            statisticsList.Add(new StatisticModel()
            {
                StatisticDate = dateString,
                StatisticPage = page,
                StatisticMessage = text
            });
            await WriteAsync(statisticsList, page);
        }

        private async Task WriteAsync(List<StatisticModel> statisticsList, string page)
        {
            try
            {
                using (var stream = this.FileInfo().OpenWrite())
                {
                    if (stream != null)
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            var result = JsonConvert.SerializeObject(statisticsList);
                            await writer.WriteAsync(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Handler.ExceptionHandler(TAG + $".WriteAsync({page})", ClientService.GetExceptionDetail(ex));
            }
        }

        public async Task Send()
        {
            if (!IsAllowed && !IsPermitted) return;
        }

        private async Task<List<StatisticModel>> ReadAsync(string page)
        {
            try
            {
                using (var stream = this.FileInfo().OpenRead())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = await reader.ReadToEndAsync();
                            return JsonConvert.DeserializeObject<List<StatisticModel>>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Handler.ExceptionHandler(TAG + $".ReadAsync({page})", ClientService.GetExceptionDetail(ex));
            }

            return new List<StatisticModel>();
        }

        private FileInfo FileInfo() => new FileInfo(FilePath());

        private string FilePath() =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FILE_NAME);
    }
}