using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Test;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Test
{
    public partial class MenuTopBarTestPage : MenuTopBarTestPageXaml
    {
        private RootPage Root;

        public MenuTopBarTestPage(RootPage root)
        {
            InitializeComponent();
            this.Root = root;
            this.topBarWithMenu.MenuImageAction = MenuVisibleToggle;
            this.topBarWithMenu.Bind();
        }

        public void MenuVisibleToggle()
        {
            this.topBarWithMenu.IsMenuVisible = this.topBarWithMenu.IsMenuVisible == false;
            this.Root.IsPresented = this.topBarWithMenu.IsMenuVisible;
        }
    }

    public abstract class MenuTopBarTestPageXaml : ModelBoundContentPage<MenuTopBarTestViewModel>
    {
    }
}