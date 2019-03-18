using com.organo.x4ever.Localization;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.TrainingVideo;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Enums;
using System;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.TrainingVideo
{
    public partial class TrainingVideoPage : TrainingVideoPageXaml
    {
        private TrainingVideoViewModel _model;

        public TrainingVideoPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                this._model = new TrainingVideoViewModel();
                this._model.Root = root;
                BindingContext = this._model;
                this._model.OnLoad();
                ButtonPlayStop.Clicked += ButtonPlayStop_Clicked;
                ButtonNext.Clicked += ButtonNext_Clicked;
                ButtonPrevious.Clicked += ButtonPrevious_Clicked;
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private void ButtonPrevious_Clicked(object sender, EventArgs e)
        {
            this._model.ButtonPlayStop = TextResources.Play;
            if (this._model.CurrentMedia != null)
            {
                var currentIndex = this._model.CurrentMedia.ID;
                this._model.SetCurrentMedia((currentIndex - 1));
            }

            this.ButtonPlayStop_Clicked(sender, e);
        }

        private void ButtonNext_Clicked(object sender, EventArgs e)
        {
            this._model.ButtonPlayStop = TextResources.Play;
            if (this._model.CurrentMedia != null)
            {
                var currentIndex = this._model.CurrentMedia.ID;
                this._model.SetCurrentMedia((currentIndex + 1));
            }

            this.ButtonPlayStop_Clicked(sender, e);
        }

        private async void ButtonPlayStop_Clicked(object sender, EventArgs e)
        {
            this._model.ButtonPlayStop = this._model.ButtonPlayStop == TextResources.Stop
                ? TextResources.Play
                : TextResources.Stop;

            if (this._model.ButtonPlayStop == TextResources.Stop && this._model.CurrentMedia != null)
                await CrossMediaManager.Current.Play(this._model.CurrentMedia.MediaUrl,
                    this._model.CurrentMedia.MediaTypeShortTitle.Contains("v")
                        ? MediaFileType.Video
                        : MediaFileType.Audio);
            else
                await CrossMediaManager.Current.Stop();
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (this._model.Root != null)
                this._model.Root.IsPresented = this._model.Root.IsPresented == false;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class TrainingVideoPageXaml : ModelBoundContentPage<TrainingVideoViewModel>
    {
    }
}