using System;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Registration;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Enums;
using Plugin.MediaManager.Abstractions.Implementations;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Welcome
{
    public class WelcomeViewModel : BaseViewModel
    {
        private IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;
        private readonly ISecureStorage _secureStorage;
        public WelcomeViewModel(INavigation navigation = null) : base(navigation)
        {
            this.Opacity = 0;
            this.Source = string.Empty;
            this.SkipText = string.Empty;
            this.SignInText = string.Empty;
            this.SignUpText = string.Empty;
            this.VideoTransparentColor = Palette._VideoTransparent;
            this.IsSkipVisible = false;
            MediaFiles = new List<MediaFile>();
            _secureStorage = DependencyService.Get<ISecureStorage>();
        }

        public async Task OnLoad()
        {
           
                SetActivityResource(true, false, false, false, string.Empty, string.Empty, string.Empty);
                this.IsSkipVisible = await this.Page_Load();
                await this.ShowButtonAsync();
            //}
        }

        private async Task<bool> Page_Load()
        {
            MediaFiles = new List<MediaFile>();
            this.Source = DependencyService.Get<IHelper>().GetFilePath(App.Configuration.AppConfig.WelcomeVideoUrl, FileType.Video);
            MediaFiles.Add(new MediaFile()
            {
                Url = this.Source,
                Type = MediaFileType.Video,
                MetadataExtracted = false,
                Availability = ResourceAvailability.Remote,
            });
            CrossMediaManager.Current.MediaQueue.Repeat = RepeatType.RepeatOne;
            await CrossMediaManager.Current.Play(MediaFiles);
            this.SkipText = TextResources.Skip;
            this.SignInText = TextResources.SignIn;
            this.SignUpText = TextResources.SignUp;
            await Task.Delay(4000);

            return true;
        }

        public async void StopPlayer()
        {
            try
            {
                await PlaybackController.Stop();
            }
            catch (Exception)
            {
                // Commented
            }
        }

        private async Task ShowButtonAsync()
        {
            if (this.IsSkipVisible)
            {
                await Task.Run(async () =>
                {
                    this.Opacity = .1;
                    while (this.Opacity < 1)
                    {
                        await Task.Delay(100);
                        await this.UpdateOpacity();
                    }
                });
            }
        }

        private async Task UpdateOpacity()
        {
            await Task.Run(() => { this.Opacity = this.Opacity + .1; });
        }

        public List<MediaFile> MediaFiles { get; set; }

        private string _source;
        public const string SourcePropertyName = "Source";

        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value, SourcePropertyName); }
        }

        public async Task Play()
        {
            await PlaybackController.Play();
        }

        private string _skipText;
        public const string SkipTextPropertyName = "SkipText";

        public string SkipText
        {
            get { return _skipText; }
            set { SetProperty(ref _skipText, value, SkipTextPropertyName); }
        }

        private string _signInText;
        public const string SignInTextPropertyName = "SignInText";

        public string SignInText
        {
            get { return _signInText; }
            set { SetProperty(ref _signInText, value, SignInTextPropertyName); }
        }

        private string _signUpText;
        public const string SignUpTextPropertyName = "SignUpText";

        public string SignUpText
        {
            get { return _signUpText; }
            set { SetProperty(ref _signUpText, value, SignUpTextPropertyName); }
        }

        private bool _isSkipVisible;
        public const string IsSkipVisiblePropertyName = "IsSkipVisible";

        public bool IsSkipVisible
        {
            get { return _isSkipVisible; }
            set { SetProperty(ref _isSkipVisible, value, IsSkipVisiblePropertyName); }
        }

        private Color _videoTransparentColor;
        public const string VideoTransparentColorPropertyName = "VideoTransparentColor";

        public Color VideoTransparentColor
        {
            get { return _videoTransparentColor; }
            set { SetProperty(ref _videoTransparentColor, value, VideoTransparentColorPropertyName); }
        }

        private ICommand _skipCommand;

        public ICommand SkipCommand
        {
            get
            {
                return _skipCommand ?? (_skipCommand = new Command(
                           (obj) =>
                           {
                               App.Configuration.WelcomeVideoSkipped();
                               App.GoToAccountPage();
                           }));
            }
        }

        private double _opacity;
        public const string OpacityPropertyName = "Opacity";

        public double Opacity
        {
            get { return _opacity; }
            set { SetProperty(ref _opacity, value, OpacityPropertyName); }
        }
    }
}