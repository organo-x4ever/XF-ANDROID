﻿
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class TrackerLogPage : TrackerLogPageXaml, IDisposable
    {
        private ProfileEnhancedViewModel _model;

        public TrackerLogPage(ProfileEnhancedViewModel model)
        {
            try
            {
                InitializeComponent();
                _model = model;
                Init();
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private async void Init(object obj = null)
        {
            BindingContext = _model;
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, true);
            SetGridTracker();
        }

        private async void SetGridTracker()
        {
            await Task.Factory.StartNew(async () =>
            {
                gridTracker.ProfileModel = _model;
                gridTracker.Source = _model.UserTrackers;
                gridTracker.CloseAction = async () =>
                {
                    _model.ShowTrackerDetail = false;
                    await Navigation.PopAsync();
                    await gridTracker.ProfileModel.GetUserAsync(
                        gridTracker.ProfileModel.UserDetail.IsTrackerRequiredAfterDelete);
                };
            });
        }

        protected override bool OnBackButtonPressed()
        {
            _model.ShowTrackerDetail = false;
            Device.BeginInvokeOnMainThread(async () => { await Navigation.PopAsync(); });
            return true;
        }

        public void Dispose()
        {
            _model.ShowTrackerDetail = false;
            if (!isDispose)
            {
                isDispose = true;
                gridTracker.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        private bool isDispose = false;
    }

    public abstract class TrackerLogPageXaml : ModelBoundContentPage<ProfileEnhancedViewModel>
    {
    }
}