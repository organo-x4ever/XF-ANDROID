using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.ViewModels.Splash
{
    public class SplashItemViewModel : BaseViewModel
    {
        public string Description { get; set; }
        public string Uri { get; set; }

        public bool UriIsPresent
        {
            get { return !String.IsNullOrWhiteSpace(Uri); }
        }
    }
}