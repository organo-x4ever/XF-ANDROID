using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class TrackerLogPage : TrackerLogPageXaml
    {
        private MyProfileViewModel _model;

        public TrackerLogPage(MyProfileViewModel model)
        {
            InitializeComponent();
            _model = model;
            Init();
        }

        private async void Init(object obj = null)
        {
            BindingContext = _model;
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model.Navigation = App.CurrentApp.MainPage.Navigation;
            SetGridTracker();
        }

        private async void SetGridTracker()
        {
            await Task.Factory.StartNew(() =>
            {
                gridTracker.ProfileModel = _model;
                gridTracker.Source = _model.UserTrackers;
                gridTracker.CloseAction = async () =>
                {
                    _model.ShowTrackerDetail = false;
                    await _model.PopModalAsync();
                    await gridTracker.ProfileModel.GetUserAsync(
                        gridTracker.ProfileModel.UserDetail.IsTrackerRequiredAfterDelete);
                };
            });
        }

        private void ClosePopup()
        {
            Device.BeginInvokeOnMainThread(async () => { await _model.PopModalAsync(); });
        }

        protected override bool OnBackButtonPressed()
        {
            _model.ShowTrackerDetail = false;
            ClosePopup();
            return true;
        }
    }

    public abstract class TrackerLogPageXaml : ModelBoundContentPage<MyProfileViewModel>
    {
    }
}