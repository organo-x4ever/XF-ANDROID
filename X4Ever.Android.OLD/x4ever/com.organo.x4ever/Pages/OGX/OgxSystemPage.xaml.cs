using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Controls;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.OGX;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.x4ever.Pages.OGX
{
    public partial class OgxSystemPage : OgxSystemXaml
    {
        private readonly OGXViewModel _model;
        private IHelper _helper;
        private IFileDownloadService _fileDownloadService;

        public OgxSystemPage(RootPage root)
        {
                InitializeComponent();
                _model = new OGXViewModel()
                {
                    Root = root
                };
                Init();
        }

        private async void Init()
        {
            _helper = DependencyService.Get<IHelper>();
            _fileDownloadService = DependencyService.Get<IFileDownloadService>();
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _model;
            if (await _model.LoadPageAsync())
            {
                var fullPath = _helper.GetFilePath(_model.FileUri, FileType.Document);
                if (await _fileDownloadService.DownloadFileAsync(fullPath, _model.FileUri))
                {
                    stackLayoutContent.Children.Add(new CustomWebView()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Uri = _model.FileUri
                    });

                    await RemoveFile();
                }
            }
        }

        async Task RemoveFile()
        {
            await Task.Delay(3000);
            await _fileDownloadService.RemoveFileAsync(_model.FileUri);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class OgxSystemXaml : ModelBoundContentPage<OGXViewModel>
    {
    }
}