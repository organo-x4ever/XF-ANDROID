using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Rewards;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Rewards
{
    public partial class RewardsPage : RewardsXamlPage
    {
        private RewardsViewModel _model;

        public RewardsPage(RootPage root)
        {
            InitializeComponent();
            _model = new RewardsViewModel()
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

    public abstract class RewardsXamlPage : ModelBoundContentPage<RewardsViewModel>
    {
    }
}