using com.organo.x4ever.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Testimonial
{
    public class TestimonialPhotoViewModel : BaseViewModel
    {
        public TestimonialPhotoViewModel(INavigation navigation = null) : base(navigation)
        {
        }

        private Models.Testimonial _testimonial;
        public const string TestimonialPropertyName = "Testimonial";

        public Models.Testimonial Testimonial
        {
            get { return _testimonial; }
            set { SetProperty(ref _testimonial, value, TestimonialPropertyName); }
        }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(
                           async (obj) => { await this.PopModalAsync(); }));
            }
        }
    }
}