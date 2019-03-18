using System.Windows.Input;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Content
{
    public class ContentViewModel : BaseViewModel
    {
        public ContentViewModel(INavigation navigation = null) : base(navigation)
        {
        }

        //private RootPage root;
        //public const string RootPropertyName = "Root";

        //public RootPage Root
        //{
        //    get { return root; }
        //    set { SetProperty(ref root, value, RootPropertyName); }
        //}

        //private ICommand _showSideMenuCommand;

        //public ICommand ShowSideMenuCommand
        //{
        //    get
        //    {
        //        return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
        //        {
        //            if (App.Configuration.IsMenuLoaded)
        //                this.Root.IsPresented = this.Root.IsPresented == false;
        //        }));
        //    }
        //}
    }
}