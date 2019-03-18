using com.organo.x4ever.Globals;
using com.organo.x4ever.Models.Authentication;
using com.organo.x4ever.Pages.MainPage;
using com.organo.x4ever.Pages.Welcome;
using com.organo.x4ever.Services;
using System;
using System.Threading.Tasks;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Pages.ErrorPages;
using Xamarin.Forms;

namespace com.organo.x4ever
{
    public partial class App : Application
    {
        private static Application _app;
        public static Application CurrentApp => _app;
        public static IAppConfiguration Configuration { get; set; }
        public static AuthenticationResult CurrentUser { get; set; }
        private static short timerSeconds = 30;
        private readonly string TAG = typeof(App).FullName;
        public App(string action = "")
        {
            try
            {
                InitializeComponent();
                _app = this;
                GoToInitialPage(action);
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private async void GoToInitialPage(string action)
        {
            Configuration = new AppConfiguration();
            Configuration.Init();
            Configuration.GetConnectionInfo();
            if (!Configuration.IsConnected)
            {
                CurrentApp.MainPage = new InternetConnectionPage();
                return;
            }
            var token = Configuration.GetToken();
            if (token != null && token.Trim().Length > 0)
                GoToAccountPage();
            else
                GoToWelcomePage();

            await Configuration.GetActivity(action);
        }

        public static void GoToWelcomePage() => CurrentApp.MainPage = new WelcomePage();
        public static void GoToErrorPage(Exception ex) => CurrentApp.MainPage = new ErrorPage(ex);

        public static void GoToAccountPage(bool loggedIn = false, string message = "")
        {
            if (loggedIn)
                CurrentApp.MainPage = new RootPage();
            else
                CurrentApp.MainPage = new MainPage(message);
            var seconds = TimeSpan.FromSeconds(timerSeconds);
            Device.StartTimer(seconds, () =>
            {
                CheckConnectionAsync().GetAwaiter();
                return Configuration.IsConnected;
            });
        }

        public static async Task LogoutAsync() => await DependencyService.Get<IAuthenticationService>().LogoutAsync();

        public static int AnimationSpeed = 300;

        private static async Task<bool> CheckConnectionAsync()
        {
            return await Task.Run<bool>(() =>
            {
                Configuration.GetConnectionInfo();
                if (!Configuration.IsConnected)
                    CurrentApp.MainPage = new InternetConnectionPage();
                return Configuration.IsConnected;
            });
        }

        protected override void OnStart()
        {
            //////CrossPushNotification.Current.Register();
            //if (Device.RuntimePlatform == Device.Android && CurrentApp.MainPage != null)
            //    // Handle when your app starts
            //    await CurrentApp.MainPage.Navigation.PushModalAsync(new SplashView());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}