﻿using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;
using Android.Widget;
using com.organo.xchallenge;
using com.organo.xchallenge.Controls;
using Switch = Xamarin.Forms.Switch;
using com.organo.xchallenge.ViewModels.UserSettings;

namespace com.organo.xchallenge.Pages.UserSettings
{
    public partial class UserSettingPage : UserSettingPageXaml
    {
        private readonly UserSettingsViewModel _model;

        public UserSettingPage()
        {
            try
            {
                InitializeComponent();
                _model = new UserSettingsViewModel();
                Page_Load();
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(UserSettingPage).FullName, ex);
            }
        }

        private async void Page_Load()
        {
           await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, true);
            BindingContext = _model;
            _model.SetActivityResource();
            await _model.LoadAppLanguages(OnLanguageRetrieve);
            await _model.LoadWeightVolume(BindWeightVolume);
        }

        private void OnLanguageRetrieve()
        {
            languageOption.DataSource = _model.ApplicationLanguages;
            languageOption.ShowSelection = _model.ApplicationLanguages.Count > 1;
            languageOption.OnItemSelectedAction = OnLanguageSelected;
            languageOption.TextStyle = (Style) App.CurrentApp.Resources["labelStyleTableViewItem"];
            languageOption.FlagStyle = (Style) App.CurrentApp.Resources["imageEntryIcon"];
        }

        private void OnLanguageSelected()
        {
            _model.OnLanguageSelected(languageOption.DataSourceSelected);
        }

        private void BindWeightVolume()
        {
            PickerWeightVolume.Title = TextResources.SelectWeightVolumeType;
            PickerWeightVolume.ItemsSource = _model.WeightVolumeData;
            PickerWeightVolume.ItemDisplayBinding = new Binding("DisplayVolume");
            _model.WeightVolumeClick_Action = OnWeightVolumeClicked;
            PickerWeightVolume.SelectedIndexChanged += PickerWeightVolume_SelectedIndexChanged;
        }

        private void OnWeightVolumeClicked()
        {
            if (_model.WeightVolumeData.Count > 1)
            {
                PickerWeightVolume.Focus();
            }
        }

        private void PickerWeightVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            _model.OnWeightVolumeChange((WeightVolume) PickerWeightVolume.SelectedItem);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class UserSettingPageXaml : ModelBoundContentPage<UserSettingsViewModel>
    {
    }
}