using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Plugin.Toasts;
using Xamarin.Forms;

[assembly:Dependency(typeof(BackButtonPress))]

namespace com.organo.x4ever.Services
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

        public async Task ActionAsync(Action action)
        {
            await Task.Run(() => { action?.Invoke(); });
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

        public async Task<bool> ExitAsync()
        {
            if (DateTime.Now.CompareTo(_backPressPeriod.AddMilliseconds(timeDelay)) >= 0)
            {
                App.CurrentApp.Quit();
                return false;
            }
            else
            {
                //https://github.com/EgorBo/Toasts.Forms.Plugin
                var result = await _notificator.Notify(new NotificationOptions()
                {
                    Title = TextResources.MessagePressBackTwiceToExitTitle,
                    Description = TextResources.MessagePressBackTwiceToExit,
                });
            }

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

        public async Task ExitWarningAsync()
        {
            await Task.Run(() =>
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
            });
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

        public async Task<bool> RedirectAsync(Page page)
        {
            if (page != null)
            {
                App.CurrentApp.MainPage = page;
                return true;
            }

            return await ExitAsync();
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

        public async Task<bool> RedirectAsync(RootPage root, MenuType? menuType)
        {

            if (menuType != null)
            {
                await root.NavigateAsync((MenuType) menuType, true);
                return true;
            }

            return await ExitAsync();
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

        public async Task<bool> RedirectAsync(RootPage root)
        {
            var lastPage = root?.VisitedPages.GetPreviousMenuType();
            if (lastPage != null)
            {
                await root.NavigateAsync(lastPage.MenuType, true);
                return true;
            }

            return await ExitAsync();
        }

        public void ExitFinal()
        {
            App.CurrentApp.Quit();
        }
    }
}