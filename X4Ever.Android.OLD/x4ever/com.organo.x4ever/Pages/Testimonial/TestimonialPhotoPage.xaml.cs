using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.Testimonial;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Testimonial
{
    public partial class TestimonialPhotoPage : TestimonialPhotoPageXaml
    {
        private TestimonialPhotoViewModel _model;

        public TestimonialPhotoPage(Models.Testimonial testimonial)
        {
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this._model = new TestimonialPhotoViewModel(App.CurrentApp.MainPage.Navigation)
            {
                Testimonial = testimonial
            };
            BindingContext = this._model;
        }

        protected override bool OnBackButtonPressed()
        {
            _model.PopModalAsync().GetAwaiter();
            return true;
        }
    }

    public abstract class TestimonialPhotoPageXaml : ModelBoundContentPage<TestimonialPhotoViewModel>
    {
    }
}