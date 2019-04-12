using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages;
using Xamarin.Forms;

namespace com.organo.xchallenge.Services
{
    public interface IBackButtonPress
    {
        void ExitFinal();
        bool Exit();
        void ExitWarning();
        void Action(Action action);
        bool Redirect(Page page);
        bool Redirect(RootPage root, MenuType? menuType);
        bool Redirect(RootPage root);
    }
}