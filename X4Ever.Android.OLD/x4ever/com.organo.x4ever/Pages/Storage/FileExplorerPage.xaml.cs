using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.Storage;
using System;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Storage
{
    public partial class FileExplorerPage : FileExplorerPageXaml
    {
        private FileExplorerViewModel _model;

        public FileExplorerPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                this._model = new FileExplorerViewModel();
                this._model.Root = root;
                BindingContext = this._model;
                this._model.GetFiles();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            this._model.Root.IsPresented = this._model.Root.IsPresented == false;
        }
    }

    public abstract class FileExplorerPageXaml : ModelBoundContentPage<FileExplorerViewModel>
    {
    }
}