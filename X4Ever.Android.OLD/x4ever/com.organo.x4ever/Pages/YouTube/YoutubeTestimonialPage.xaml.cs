using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Models.Youtube;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.YouTube;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.x4ever.Pages.YouTube
{
    public partial class YoutubeTestimonialPage : YoutubeTestimonialPageXaml
    {
        private YoutubeViewModel _model;

        public YoutubeTestimonialPage(RootPage root)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new YoutubeViewModel()
            {
                Root = root,
            };
            BindingContext = _model;
        }

        private void ListViewOnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            var youtubeItem = itemTappedEventArgs.Item as YoutubeItem;
            Device.OpenUri(new Uri(string.Format(_model.YoutubeConfiguration.VideoWatchApiUrl, youtubeItem?.VideoId)));
        }
    }

    public abstract class YoutubeTestimonialPageXaml : ModelBoundContentPage<YoutubeViewModel>
    {
    }
}