
using com.organo.xchallenge.Animated;
using com.organo.xchallenge.Effects.Control;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Pages.Splash;
using com.organo.xchallenge.ViewModels.Animation_Test;
using System;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Animation_Test
{
    public partial class Animation_TestMainPage : Animation_TestMainPageXaml
    {
        private IAnimation animation;
        Button buttonAnimatedButton, buttonLabel, buttonBack;
        public Animation_TestMainPage()
        {
            try
            {
                InitializeComponent();
                App.Configuration.Initial(this);
                BindingContext = new Animation_TestMainViewModel();
                animation = DependencyService.Get<IAnimation>();
                AddComponent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async void AddComponent()
        {
            buttonBack = new Button()
            {
                Text = "Go back to Main",
                Style = (Style)App.CurrentApp.Resources["buttonStyle"]
            };
            buttonBack.Clicked += (sender, e) =>
            {
                App.CurrentApp.MainPage = new SplashPage();
            };

            buttonLabel = new Button()
            {
                Text = "Label Animation",
                Style = (Style)App.CurrentApp.Resources["buttonStyleGray"]
            };
            buttonLabel.Clicked += (sender, e) =>
            {
                App.CurrentApp.MainPage = new LabelEffects();
            };

            buttonAnimatedButton = new Button()
            {
                Text = "Animated Button",
                Style = (Style)App.CurrentApp.Resources["buttonStyleGray"]
            };
            buttonAnimatedButton.Clicked += (sender, e) =>
            {
                App.CurrentApp.MainPage = new AnimatedButtonPage();
            };

            stackLayoutContent.Children.Add(buttonAnimatedButton);
            stackLayoutContent.Children.Add(buttonLabel);
            stackLayoutContent.Children.Add(buttonBack);
            await animation.Animate(stackLayoutContent);
        }
    }
    public abstract class Animation_TestMainPageXaml : ModelBoundContentPage<Animation_TestMainViewModel> { }
}