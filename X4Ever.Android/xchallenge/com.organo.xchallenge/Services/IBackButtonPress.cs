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
        Task<bool> ExitAsync();
        void ExitWarning();
        Task ExitWarningAsync();
        void Action(Action action);
        Task ActionAsync(Action action);
        bool Redirect(Page page);
        Task<bool> RedirectAsync(Page page);
        bool Redirect(RootPage root, MenuType? menuType);
        Task<bool> RedirectAsync(RootPage root, MenuType? menuType);
        bool Redirect(RootPage root);
        Task<bool> RedirectAsync(RootPage root);
    }
}