using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Animation_Test;
using com.organo.xchallenge.Effects.Control;

namespace com.organo.xchallenge.Pages.Animation_Test
{
    public partial class LabelEffects : LabelEffectsXaml
    {
        private IAnimation animation;
        Button buttonBack;
        public LabelEffects()
        {
            try
            {
                InitializeComponent();
                App.Configuration.Initial(this);
                BindingContext = new LabelEffectsViewModel();
                animation = DependencyService.Get<IAnimation>();
                AddComponent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void AddComponent()
        {
            StartAnimation();
            buttonBack = new Button()
            {
                Text = "Go back",
                Style = (Style)App.CurrentApp.Resources["buttonStyle"]
            };
            buttonBack.Clicked += (sender, e) =>
            {
                App.CurrentApp.MainPage = new Animation_TestMainPage();
            };
            await animation.Add(buttonBack);

            stackLayoutContent.Children.Add(buttonBack);
            await animation.Animate();
        }

        private async void StartAnimation()
        {
            int count = 0;
            bool animationStart = true;
            while (animationStart)
            {
                await Task.Delay(2000);
                WhiteLabel.FadeTo(0, 2000);
                await Task.Delay(2000);
                WhiteLabel.FadeTo(1, 2000);
                animationStart = count < 3;
                count++;
            }
        }
    }

    public abstract class LabelEffectsXaml : ModelBoundContentPage<LabelEffectsViewModel> { }
}