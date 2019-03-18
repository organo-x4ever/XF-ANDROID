using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.OGX
{
    public class OGXViewModel : BaseViewModel
    {
        public OGXViewModel(INavigation navigation = null) : base(navigation)
        {
        }

        public async Task<bool> LoadPageAsync()
        {
            var response = await DependencyService.Get<IFileService>().GetFileAsync(TextResources.OGX_Content_FilePath);
            if (response != null)
                FileUri = response;
            return FileUri != null && FileUri.Trim().Length > 0;
        }

        private string _fileUri;
        public const string FileUriPropertyName = "FileUri";

        public string FileUri
        {
            get { return _fileUri; }
            set { SetProperty(ref _fileUri, value, FileUriPropertyName); }
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
        //            this.Root.IsPresented = this.Root.IsPresented == false;
        //        }));
        //    }
        //}
    }
}
