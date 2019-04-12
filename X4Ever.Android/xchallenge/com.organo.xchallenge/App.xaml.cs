using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Pages.ErrorPages;
using com.organo.xchallenge.Pages.MainPage;
using com.organo.xchallenge.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge
{
    public partial class App : Application
    {
        private readonly string TAG = typeof(App).FullName;
        private static Application _app;
        public static Application CurrentApp => _app;
        public static IAppConfiguration Configuration { get; set; }
        public static AuthenticationResult CurrentUser { get; set; }
        private static Page LastActivePage { get; set; }
        private static string Action { get; set; }

        public App(string action = "")
        {
            try
            {
                InitializeComponent();
                _app = this;
                Action = action;
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            GoToInitialPage(Action);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            LastActivePage = CurrentApp.MainPage;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CurrentApp.MainPage = LastActivePage;
        }

        private async void GoToInitialPage(string action)
        {
            Configuration = new AppConfiguration();
            GoToAccountPage();
            await Configuration.GetConnectionInfoAsync();
            if (!Configuration.IsConnected)
                CurrentApp.MainPage = new InternetConnectionPage();
        }

        public static async Task LogoutAsync() => await DependencyService.Get<IAuthenticationService>().LogoutAsync();

        public static void GoToAccountPage(bool loggedIn = false)
        {
            if (loggedIn)
                CurrentApp.MainPage = new RootPage();
            else
                CurrentApp.MainPage = new MainPage();
        }
    }
}