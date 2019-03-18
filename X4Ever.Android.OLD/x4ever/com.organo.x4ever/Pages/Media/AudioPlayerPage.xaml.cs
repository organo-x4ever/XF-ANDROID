using com.organo.x4ever.Models.Media;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.Media;
using System;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Notification;
using com.organo.x4ever.Services;
using Plugin.MediaManager.Abstractions.Implementations;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Media
{
    public partial class AudioPlayerPage : AudioPlayerPageXaml
    {
        private AudioPlayerViewModel _model;

        public AudioPlayerPage(RootPage rootPage)
        {
            try
            {
                InitializeComponent();
                Init(rootPage);
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        public async void Init(object obj)
        {
            await App.Configuration.InitialAsync(this);
            _model = new AudioPlayerViewModel()
            {
                Root = (RootPage) obj
            };
            BindingContext = _model;
            await _model.GetFilesAsync();
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedContent = (MediaFile) e.SelectedItem;
            _model.CurrentMediaFile = selectedContent;
            int index = this._model.MediaFiles.FindIndex(m => m == selectedContent && m == selectedContent);
            await _model.PlayCurrent(index);
        }

        protected override void OnDisappearing()
        {
            _model.StopAsync();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class AudioPlayerPageXaml : ModelBoundContentPage<AudioPlayerViewModel>
    {
    }
}