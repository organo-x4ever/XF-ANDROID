using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Registration;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Registration
{
    public partial class TermAndConditionsPage : TermAndConditionsPageXaml
    {
        private TermAndConditionViewModel _model;

        public TermAndConditionsPage()
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            this._model = new TermAndConditionViewModel(App.CurrentApp.MainPage.Navigation);
            BindingContext = this._model;
        }
        
        protected override bool OnBackButtonPressed()
        {
            _model.CloseWindow().GetAwaiter();
            return true;
        }
    }

    public abstract class TermAndConditionsPageXaml : ModelBoundContentPage<TermAndConditionViewModel>
    {
    }
}