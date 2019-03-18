using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.ErrorPages
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