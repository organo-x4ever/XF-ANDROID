using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class LogDetailPage : LogDetailPageXaml
    {
        private MyProfileViewModel _model;

        public LogDetailPage(MyProfileViewModel model)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = model;
            Init();
        }

        private void Init()
        {
            _model.Navigation = App.CurrentApp.MainPage.Navigation;
            BindingContext = _model;
            ListViewTrackers.ItemSelected += (sender, e) =>
            {
                if (_model.UserDetail.IsDownloadAllowed)
                {
                    if (ListViewTrackers.SelectedItem != null)
                        DownloadImages((TrackerPivot) e.SelectedItem);
                }

                ListViewTrackers.SelectedItem = null;
            };
        }

        private async void DownloadImages(TrackerPivot tracker)
        {
            await Task.Factory.StartNew(() => { Device.OpenUri(new Uri(tracker.FrontImageWithUrl)); });
            await Task.Delay(TimeSpan.FromSeconds(2));
            await Task.Factory.StartNew(() => { Device.OpenUri(new Uri(tracker.SideImageWithUrl)); });
        }
        
        protected override bool OnBackButtonPressed()
        {
            _model.ShowTrackerDetail = false;
            return true;
        }
    }

    public abstract class LogDetailPageXaml : ModelBoundContentPage<MyProfileViewModel>
    {
    }
}