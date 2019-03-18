using System.Windows.Input;
using com.organo.x4ever.Pages;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Content
{
    public class ContentViewModel : BaseViewModel
    {
        public ContentViewModel(INavigation navigation = null) : base(navigation)
        {
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    this.Root.IsPresented = this.Root.IsPresented == false;
                }));
            }
        }
    }
}