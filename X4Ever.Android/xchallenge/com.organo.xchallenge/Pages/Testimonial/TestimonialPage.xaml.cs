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
                App.Configuration.InitialAsync(this);
                this._model = new TestimonialViewModel(App.CurrentApp.MainPage.Navigation);
                this._model.Root = rootPage;
                BindingContext = this._model;
                this.Initial();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private async void Initial()
        {
            await this._model.OnLoad();
            this.ListViewTestimonials.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var content = (Models.Testimonial)e.SelectedItem;
                if (content.IsVideoExists)
                    await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TestimonialDetailPage(content));
                else
                    await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TestimonialPhotoPage(content));
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