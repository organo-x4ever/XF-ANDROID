using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Community;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Community
{
    public partial class CommunityPage : CommunityPageXaml
    {
        private CommunityViewModel _model;

        public CommunityPage(RootPage root)
        {
            InitializeComponent();
            _model = new CommunityViewModel()
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
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class CommunityPageXaml : ModelBoundContentPage<CommunityViewModel>
    {
    }
}