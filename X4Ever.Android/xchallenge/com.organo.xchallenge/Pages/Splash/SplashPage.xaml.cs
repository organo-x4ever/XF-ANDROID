
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Splash;
using System;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Splash
{
    public partial class SplashPage : SplashPageXaml
    {
        public SplashPage()
        {
            try
            {
                InitializeComponent();

                App.Configuration.Initial(this);
                NavigationPage.SetHasNavigationBar(this, false);
                //this._model = new SplashViewModel();
                //BindingContext = this._model;
                //_AuthenticationService = DependencyService.Get<IAuthenticationService>();
                //animation = DependencyService.Get<IAnimation>();
                //var notification = DependencyService.Get<INotificationServices>();
                //notification.Send("Sample Notification", "Hello World! This is my first notification!");
                AddComponent();
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }

        void AddComponent()
        {
            //await animation.Clear();
            //await animation.Add(UserSignUp);
            //await animation.Add(UserMenu);
            //await animation.Add(UserSignIn);
            //var imageMainLogo = TextResources.Logo_AppMain;
            //var _imageMainLogo = Device.RuntimePlatform == Device.iOS ?
            //    ImageSource.FromFile(imageMainLogo) :
            //    (Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imageMainLogo) :
            //    ImageSource.FromFile(imageMainLogo));
            //imageLogo.Source = _imageMainLogo;
            ////await animation.Add(imageLogo);

            //var imageCompanyLogoPath = TextResources.Logo_Company;
            //var _imageCompanyLogoPath = Device.RuntimePlatform == Device.iOS ?
            //    ImageSource.FromFile(imageCompanyLogoPath) :
            //    (Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imageCompanyLogoPath) :
            //    ImageSource.FromFile(imageCompanyLogoPath));
            //imageCompanyLogo.Source = _imageCompanyLogoPath;
            ////await animation.Add(imageCompanyLogo);

            //var imageUserSignUp = TextResources.UserSignUpBlack;
            //var _imageUserSignUp = Device.RuntimePlatform == Device.iOS ?
            //    ImageSource.FromFile(imageUserSignUp) :
            //    (Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imageUserSignUp) :
            //    ImageSource.FromFile(imageUserSignUp));
            //UserSignUp.Source = _imageUserSignUp;
            //var tapGestureSignUp = new TapGestureRecognizer()
            //{
            //    Command = new Command(UserSignUp_Clicked)
            //};
            //UserSignUp.GestureRecognizers.Add(tapGestureSignUp);
            //labelRegister.GestureRecognizers.Add(tapGestureSignUp);

            //var imageUserSignIn = TextResources.UserSignInBlack;
            //var _imageUserSignIn = Device.RuntimePlatform == Device.iOS ?
            //    ImageSource.FromFile(imageUserSignIn) :
            //    (Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imageUserSignIn) :
            //    ImageSource.FromFile(imageUserSignIn));
            //UserSignIn.Source = _imageUserSignIn;
            //var tapGestureSignIn = new TapGestureRecognizer()
            //{
            //    Command = new Command(UserSignIn_Clicked)
            //};
            //UserSignIn.GestureRecognizers.Add(tapGestureSignIn);
            //labelLogin.GestureRecognizers.Add(tapGestureSignIn);

            //var imageUserMenu = TextResources.UserMenuBlack;
            //var _imageUserMenu = Device.RuntimePlatform == Device.iOS ?
            //    ImageSource.FromFile(imageUserMenu) :
            //    (Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imageUserMenu) :
            //    ImageSource.FromFile(imageUserMenu));
            //UserMenu.Source = _imageUserMenu;
            //var tapGestureMenu = new TapGestureRecognizer()
            //{
            //    Command = new Command(UserMenu_Clicked)
            //};
            //UserMenu.GestureRecognizers.Add(tapGestureMenu);
            //labelMenu.GestureRecognizers.Add(tapGestureMenu);

            //await animation.Animate();
        }

        protected void UserSignUp_Clicked()
        {
            //App.CurrentApp.MainPage = new Register();
        }

        protected void UserSignIn_Clicked()
        {
            // The underlying call behind App.Authenticate() calls the ADAL library, which presents the login UI and awaits success.
            //var loginPage = new LoginPage();
            //loginPage.BindingContext = new ViewModels.Login.LoginViewModel();
            //await Navigation.PushModalAsync(loginPage, true);
            //App.CurrentApp.MainPage = new LoginPage();
        }

        protected void UserMenu_Clicked()
        {
            App.CurrentApp.MainPage = new SplashLivePage();
        }
    }

    /// <summary>
    /// This class definition just gives us a way to reference ModelBoundContentPage<T> in the XAML of this Page.
    /// </summary>
    public abstract class SplashPageXaml : ModelBoundContentPage<SplashViewModel>
    {
    }
}