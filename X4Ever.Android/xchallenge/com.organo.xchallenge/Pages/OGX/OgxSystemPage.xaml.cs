using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.OGX;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.OGX
{
    public partial class OgxSystemPage : OgxSystemXaml
    {
        private readonly OGXViewModel _model;
        private IHelper _helper;
        private IFileDownloadService _fileDownloadService;

        public OgxSystemPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                _model = new OGXViewModel()
                {
                    Root = root
                };
                Init();
            }
            catch (Exception)
            {

            }
        }

        private async void Init()
        {
            try
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
            catch (Exception)
            {

            }
        }

        async Task RemoveFile()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
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