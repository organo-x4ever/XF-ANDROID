using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.HowItWorks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.HowItWorks
{
    public partial class HowItWorksPage : HowItWorksXamlPage
    {
        private HowItWorksViewModel _model;

        public HowItWorksPage(RootPage root)
        {
            InitializeComponent();
            _model = new HowItWorksViewModel()
            {
                Root = root
            };
            Init();
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _model;
            await _model.LoadAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class HowItWorksXamlPage : ModelBoundContentPage<HowItWorksViewModel>
    {
    }
}