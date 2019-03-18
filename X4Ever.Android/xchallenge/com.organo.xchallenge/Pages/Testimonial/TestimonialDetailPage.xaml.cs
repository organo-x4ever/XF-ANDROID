using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Testimonial;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Testimonial
{
    public partial class TestimonialDetailPage : TestimonialDetailPageXaml
    {
        private TestimonialDetailViewModel _model;

        public TestimonialDetailPage(Models.Testimonial testimonial)
        {
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this._model = new TestimonialDetailViewModel(App.CurrentApp.MainPage.Navigation)
            {
                Testimonial = testimonial
            };
            BindingContext = _model;
            _model.OnLoad();
        }

        protected override bool OnBackButtonPressed()
        {
            _model.CloseWindow().GetAwaiter();
            return true;
        }
    }

    public abstract class TestimonialDetailPageXaml : ModelBoundContentPage<TestimonialDetailViewModel>
    {
    }
}