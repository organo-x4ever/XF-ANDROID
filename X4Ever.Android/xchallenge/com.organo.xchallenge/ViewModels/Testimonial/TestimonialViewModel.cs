using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Testimonial
{
    public class TestimonialViewModel : BaseViewModel
    {
        private ITestimonialService _testimonialService;
        private readonly IHelper _helper;

        public TestimonialViewModel(INavigation navigation = null) : base(navigation)
        {
            _testimonialService = DependencyService.Get<ITestimonialService>();
            _helper = DependencyService.Get<IHelper>();
            Testimonials = new List<Models.Testimonial>();
            TestimonialView = null;
        }

        public async Task OnLoad()
        {
            await GetTestimonialAsync();
        }

        public async Task<List<Models.Testimonial>> GetTestimonialAsync()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.TESTIMONIAL_PERSON_IMAGE);
            if (imageSize == null)
                imageSize = new ImageSize();
            var testimonials = await _testimonialService.GetAsync(true);
            foreach (var testimonial in testimonials)
            {
                testimonial.PersonPhotoSource =
                    _helper.GetFileUri(_helper.GetFilePath(testimonial.PersonPhoto, FileType.TestimonialPhoto),
                        FileType.None);
                testimonial.PersonImageHeight = imageSize.Height;
                testimonial.PersonImageWidth = imageSize.Width;
            }

            this.Testimonials = testimonials.OrderBy(t => t.DisplaySequence).ToList();
            return this.Testimonials;
        }

        private View _testimonialView;
        public const string TestimonialViewPropertyName = "TestimonialView";

        public View TestimonialView
        {
            get { return _testimonialView; }
            set { SetProperty(ref _testimonialView, value, TestimonialViewPropertyName); }
        }

        private List<Models.Testimonial> testimonials;
        public const string TestimonialsPropertyName = "Testimonials";

        public List<Models.Testimonial> Testimonials
        {
            get { return testimonials; }
            set { SetProperty(ref testimonials, value, TestimonialsPropertyName); }
        }

        //private RootPage root;
        //public const string RootPropertyName = "Root";

        //public RootPage Root
        //{
        //    get { return root; }
        //    set { SetProperty(ref root, value, RootPropertyName); }
        //}

        private ICommand readCommand;

        public ICommand ReadCommand
        {
            get { return readCommand ?? (readCommand = new Command((obj) => { Navigation.PopModalAsync(); })); }
        }

        //private ICommand _showSideMenuCommand;

        //public ICommand ShowSideMenuCommand
        //{
        //    get
        //    {
        //        return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
        //        {
        //            Root.IsPresented = Root.IsPresented == false;
        //        }));
        //    }
        //}
    }

    public enum LayoutSide
    {
        Left = 0,
        Right = 1
    }
}