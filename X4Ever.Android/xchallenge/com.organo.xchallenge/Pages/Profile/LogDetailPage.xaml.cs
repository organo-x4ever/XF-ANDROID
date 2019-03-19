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
            _model.SliderTrackerWeight = sliderTrackerWeight;
            BindingContext = _model;
            ListViewTrackers.ItemSelected += async (sender, e) =>
            {
                if (_model.UserDetail.IsDownloadAllowed)
                {
                    if (ListViewTrackers.SelectedItem == null)
                        return;
                    DownloadImages((TrackerPivot) e.SelectedItem);
                }
            };
        }

        private async void DownloadImages(TrackerPivot tracker)
        {
            await Task.Factory.StartNew(() =>
            {
                Device.OpenUri(new Uri(tracker.FrontImageWithUrl));
                Device.OpenUri(new Uri(tracker.SideImageWithUrl));
            });
        }
        
        private void TrackerEditEvent(object sender, EventArgs args)
        {
            _model.TrackerEditCommand.Execute(null);
        }

        private void TrackerUpdateEvent(object sender, EventArgs args)
        {
            _model.TrackerUpdateCommand.Execute(null);
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