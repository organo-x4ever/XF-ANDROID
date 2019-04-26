using com.organo.xchallenge.Models.Media;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Media;
using System;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Notification;
using com.organo.xchallenge.Services;
using Plugin.MediaManager.Abstractions.Implementations;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Media
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
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init(object obj)
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
            if (e.SelectedItem != null)
            {
                var selectedContent = (MediaFile) e.SelectedItem;
                _model.CurrentMediaFile = selectedContent;
                int index = this._model.MediaFiles.FindIndex(m => m == selectedContent && m == selectedContent);
                await _model.PlayCurrent(index);
            }

            ListViewPlayer.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            _model?.StopAsync();
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