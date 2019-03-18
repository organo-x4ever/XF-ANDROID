using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.Rewards;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.x4ever.Pages.Rewards
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