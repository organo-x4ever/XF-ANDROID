
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Miscellaneous;
using com.organo.xchallenge.Statics;
using System;
using com.organo.xchallenge.Controls;
using Xamarin.Forms;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Permissions;

namespace com.organo.xchallenge.Pages.Miscellaneous
{
    public partial class MiscContentPage : MiscContentPageXaml
    {
        private readonly MiscContentViewModel _model;
        private readonly IDevicePermissionServices _devicePermissionServices;
        public MiscContentPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                _model = new MiscContentViewModel() { Root = root };
                _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
                Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ClientService.GetExceptionDetail(ex));
            }
        }

        public async void Init(object obj = null)
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _model;

            if (!await _devicePermissionServices.RequestReadStoragePermission())
            {
                _model.SetActivityResource(showError: true,
                    errorMessage: TextResources.MessagePermissionReadStorageRequired);
                return;
            }

            if (!await _devicePermissionServices.RequestCameraPermission())
            {
                _model.SetActivityResource(showError: true,
                    errorMessage: TextResources.MessagePermissionCameraRequired);
                return;
            }

            if (!await _devicePermissionServices.RequestWriteStoragePermission())
            {
                _model.SetActivityResource(showError: true,
                    errorMessage: TextResources.MessagePermissionCameraRequired);
                return;
            }

            _model.WebUri = await _model.GetLink();
            contentView.Content = new HybridChromeWebView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, -6, 0, 0),
                BackgroundColor = Palette._MainBackground,
                Uri = _model.WebUri + $"?token={App.Configuration.UserToken}"
            };
        }
    }

    public abstract class MiscContentPageXaml : ModelBoundContentPage<MiscContentViewModel> { }
}