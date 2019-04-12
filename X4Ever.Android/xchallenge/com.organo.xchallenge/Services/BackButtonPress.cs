using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Plugin.Toasts;
using Xamarin.Forms;

[assembly:Dependency(typeof(BackButtonPress))]

namespace com.organo.xchallenge.Services
{
    public class BackButtonPress : IBackButtonPress
    {
        private short timeDelay => 2000;
        private DateTime _backPressPeriod;
        private readonly IToastNotificator _notificator;
        private short clickAttempts;

        public BackButtonPress()
        {
            _notificator = DependencyService.Get<IToastNotificator>();
            _backPressPeriod = DateTime.Now.AddDays(-1);
            clickAttempts = 0;
        }

        public void Action(Action action)
        {
            action?.Invoke();
        }

        public bool Exit()
        {
            var timeDifference = DateTime.Now.CompareTo(_backPressPeriod.AddMilliseconds(timeDelay));
            if (timeDifference <= 0)
            {
                App.CurrentApp.Quit();
                clickAttempts = 0;
                return false;
            }
            else
            {
                if (clickAttempts >= 1)
                {
                    //https://github.com/EgorBo/Toasts.Forms.Plugin
                    var result = _notificator.Notify(new NotificationOptions()
                    {
                        Title = TextResources.MessagePressBackTwiceToExitTitle,
                        Description = TextResources.MessagePressBackTwiceToExit,
                    });
                    clickAttempts = 0;
                }
            }

            clickAttempts++;
            _backPressPeriod = DateTime.Now;
            return true;
        }

        public void ExitWarning()
        {
            //https://github.com/EgorBo/Toasts.Forms.Plugin
            ShowNotification(new NotificationOptions()
            {
                Title = TextResources.MessagePressBackTwiceToExitTitle,
                Description = TextResources.MessageTapHereToExit,
                IsClickable = true,
                WindowsOptions = new WindowsOptions() {LogoUri = "logo.png"},
                ClearFromHistory = false,
                AllowTapInNotificationCenter = false,
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = "#" + Palette._MainAccent.ToString(),
                    ForceOpenAppOnNotificationTap = true
                }
            });
            // "#F99D1C",
        }

        private void ShowNotification(NotificationOptions options)
        {
            _notificator.Notify(
                (INotificationResult result) =>
                {
                    Debug.WriteLine("Notification [" + result.Id + "] Result Action: " +
                                    result.Action);
                }, options);
        }

        public bool Redirect(Page page)
        {
            if (page != null)
            {
                App.CurrentApp.MainPage = page;
                return true;
            }

            return Exit();
        }

        public bool Redirect(RootPage root, MenuType? menuType)
        {
            if (menuType != null)
            {
                root.NavigateAsync((MenuType) menuType, true).GetAwaiter();
                return true;
            }

            return Exit();
        }

        public bool Redirect(RootPage root)
        {
            var lastPage = root?.VisitedPages.GetPreviousMenuType();
            if (lastPage != null)
            {
                root.NavigateAsync(lastPage.MenuType, true).GetAwaiter();
                return true;
            }

            return Exit();
        }

        public void ExitFinal()
        {
            App.CurrentApp.Quit();
        }
    }
}