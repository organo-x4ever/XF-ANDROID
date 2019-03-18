using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Converters;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Statics;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Registration
{
    public class TermAndConditionViewModel : Base.BaseViewModel
    {
        public TermAndConditionViewModel(INavigation navigation = null) : base(navigation)
        {
            TermAndConditionText = TextResources.TermAndConditionsText;
            TermAndConditionHeader = TextResources.TermAndConditionsHeader;
        }

        private string termAndConditionText;
        public const string TermAndConditionTextPropertyName = "TermAndConditionText";

        public string TermAndConditionText
        {
            get { return termAndConditionText; }
            set { SetProperty(ref termAndConditionText, value, TermAndConditionTextPropertyName); }
        }

        private string termAndConditionHeader;
        public const string TermAndConditionHeaderPropertyName = "TermAndConditionHeader";

        public string TermAndConditionHeader
        {
            get { return termAndConditionHeader; }
            set { SetProperty(ref termAndConditionHeader, value, TermAndConditionHeaderPropertyName); }
        }

        private double termAndConditionText_FontSize;
        public const string termAndConditionText_FontSizePropertyName = "termAndConditionText_FontSize";

        public double TermAndConditionText_FontSize
        {
            get { return termAndConditionText_FontSize; }
            set { SetProperty(ref termAndConditionText_FontSize, value, termAndConditionText_FontSizePropertyName); }
        }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new Command(async (obj) => { await CloseWindow(); })); }
        }

        public async Task CloseWindow()
        {
            await App.CurrentApp.MainPage.Navigation.PopModalAsync();
        }
    }
}