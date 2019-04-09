using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.Youtube;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.YouTube;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.YouTube
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
            if (itemTappedEventArgs.Item != null)
            {
                var youtubeItem = itemTappedEventArgs.Item as YoutubeItem;
                Device.OpenUri(
                    new Uri(string.Format(_model.YoutubeConfiguration.VideoWatchApiUrl, youtubeItem?.VideoId)));
            }

            ListViewYouTube.SelectedItem = null;
        }
    }

    public abstract class YoutubeTestimonialPageXaml : ModelBoundContentPage<YoutubeViewModel>
    {
    }
}