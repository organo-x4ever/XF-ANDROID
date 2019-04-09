using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Testimonial;
using System;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Testimonial
{
    public partial class TestimonialPage : TestimonialPageXaml
    {
        private TestimonialViewModel _model;

        public TestimonialPage(RootPage rootPage)
        {
            try
            {
                InitializeComponent();
                _model = new TestimonialViewModel(App.CurrentApp.MainPage.Navigation);
                _model.Root = rootPage;
                BindingContext = _model;
                Init();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);

            await this._model.OnLoad();
            this.ListViewTestimonials.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (e.SelectedItem != null)
                {
                    var content = (Models.Testimonial) e.SelectedItem;
                    if (content.IsVideoExists)
                        await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TestimonialDetailPage(content));
                    else
                        await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TestimonialPhotoPage(content));
                }

                ListViewTestimonials.SelectedItem = null;
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class TestimonialPageXaml : ModelBoundContentPage<TestimonialViewModel>
    {
    }
}