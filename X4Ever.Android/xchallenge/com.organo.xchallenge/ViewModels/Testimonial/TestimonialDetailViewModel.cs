using com.organo.xchallenge.ViewModels.Base;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Enums;
using Plugin.MediaManager.Abstractions.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Globals;
using Xamarin.Forms;
using com.organo.xchallenge.Statics;

namespace com.organo.xchallenge.ViewModels.Testimonial
{
    public class TestimonialDetailViewModel : BaseViewModel
    {
        private IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;
        private readonly IHelper _helper;

        public TestimonialDetailViewModel(INavigation navigation = null) : base(navigation)
        {
            _helper = DependencyService.Get<IHelper>();
            Source = string.Empty;
        }

        public async void OnLoad()
        {
            if (Testimonial.IsVideoExists)
                await this.Page_Load();
        }

        async Task<bool> Page_Load()
        {
            List<MediaFile> mediaFiles = new List<MediaFile>();
            Source = _helper.GetFilePath(Testimonial.VideoUrl, FileType.TestimonialVideo);
            mediaFiles.Add(new MediaFile()
            {
                Url = this.Source,
                Type = MediaFileType.Video,
                MetadataExtracted = false,
                Availability = ResourceAvailability.Remote,
            });
            CrossMediaManager.Current.MediaQueue.Repeat = RepeatType.RepeatOne;
            await CrossMediaManager.Current.Play(mediaFiles);
            return true;
        }

        private Models.Testimonial _testimonial;
        public const string TestimonialPropertyName = "Testimonial";

        public Models.Testimonial Testimonial
        {
            get { return _testimonial; }
            set { SetProperty(ref _testimonial, value, TestimonialPropertyName); }
        }

        private string _source;
        public const string SourcePropertyName = "Source";

        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value, SourcePropertyName); }
        }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(
                           async (obj) => { await CloseWindow(); }));
            }
        }

        public async Task CloseWindow()
        {
            await PlaybackController.Stop();
            await this.PopModalAsync();
        }
    }
}