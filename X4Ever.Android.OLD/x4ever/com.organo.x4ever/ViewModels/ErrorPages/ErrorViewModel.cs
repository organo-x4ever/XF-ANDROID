using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Localization;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.ErrorPages
{
    public class ErrorViewModel : BaseViewModel
    {
        public ErrorViewModel(INavigation navigation = null) : base(navigation)
        {
            ErrorText = TextResources.GotError;
        }

        private string _errorText;
        public const string ErrorTextPropertyName = "ErrorText";

        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value, ErrorTextPropertyName); }
        }
    }
}