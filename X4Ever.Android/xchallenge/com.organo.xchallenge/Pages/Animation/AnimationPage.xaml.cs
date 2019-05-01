using com.organo.xchallenge.Effects.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Animation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimationPage : ContentPage
    {
        double scale = 0;
        string control = "";
        Easing easing = Easing.Linear;
        Easing easingBack = Easing.Linear;
        IAnimation animation;
        private readonly Style EntryStyle = (Style) App.CurrentApp.Resources["entryStyle"];
        public AnimationPage()
        {
            InitializeComponent();
            animation = DependencyService.Get<IAnimation>();
            //animation = new com.organo.xchallenge.Effects.Control.Animation(Easing.CubicIn, Easing.CubicOut, 2, 100);
            AnimationSetup();
            AddContent_2();
        }
        protected async Task AddContent_2()
        {
            try
            {
                List<ControlEntity> controlList = new List<ControlEntity>();
                var entry1 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                controlList.Add(new ControlEntity() { ControlView = entry1 });
                //await entry1.ScaleTo(1.2, (uint)1, eas);

                var entry2 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                controlList.Add(new ControlEntity() { ControlView = entry2 });
                //await entry2.ScaleTo(1.2, (uint)1, eas);

                var entry3 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                controlList.Add(new ControlEntity() { ControlView = entry3 });
                //await entry3.ScaleTo(1.2, (uint)1, eas);
                //stackLayoutContent1.IsVisible = stackLayoutContent2.IsVisible = stackLayoutContent3.IsVisible = false;

                stackLayoutContent1.Children.Add(entry1);
                stackLayoutContent2.Children.Add(entry2);
                stackLayoutContent3.Children.Add(entry3);
                await animation.Animate(controlList);
                //stackLayoutContent1.IsVisible = stackLayoutContent2.IsVisible = stackLayoutContent3.IsVisible = true;
                //await Task.Delay(100);
                //await entry1.ScaleTo(1, speed, easBack);
                //await Task.Delay(delay);
                //await entry2.ScaleTo(1, speed, easBack);
                //await Task.Delay(delay);
                //await entry3.ScaleTo(1, speed, easBack);
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }

        protected async Task AddContent()
        {
            try
            {
                int delay = 2;
                uint speed = 150;
                //<Entry x:Name="textTest" Text="The opacity value has no effect unless IsVisible is true" Style="{DynamicResource entryPrimary}"></Entry>
                Easing eas = Easing.CubicIn;
                Easing easBack = Easing.CubicOut;

                var entry1 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                await entry1.ScaleTo(1.2, (uint)1, eas);

                var entry2 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                await entry2.ScaleTo(1.2, (uint)1, eas);

                var entry3 = new Entry()
                {
                    Style = EntryStyle,
                    Text = "The opacity value has no effect unless IsVisible is true"
                };
                await entry3.ScaleTo(1.2, (uint)1, eas);

                stackLayoutContent1.IsVisible = stackLayoutContent2.IsVisible = stackLayoutContent3.IsVisible = false;
                stackLayoutContent1.Children.Add(entry1);
                stackLayoutContent2.Children.Add(entry2);
                stackLayoutContent3.Children.Add(entry3);
                stackLayoutContent1.IsVisible = stackLayoutContent2.IsVisible = stackLayoutContent3.IsVisible = true;
                await Task.Delay(100);
                await entry1.ScaleTo(1, speed, easBack);
                await Task.Delay(delay);
                await entry2.ScaleTo(1, speed, easBack);
                await Task.Delay(delay);
                await entry3.ScaleTo(1, speed, easBack);
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }

        protected void AnimationSetup()
        {
            //Scale Setup
            string[] scales = new string[] { "1", "1.1", "1.2", "1.3", "1.5", "1.7", "2", "3", "4", "5" };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                // handle the tap
                var scale1 = await DisplayActionSheet("Scale", "Cancel", "Ok", scales);
                if (scale1 != null)
                {
                    Double.TryParse(scale1, out scale);
                    labelScale.Text = scale1;
                }
            };
            labelScale.GestureRecognizers.Add(tapGestureRecognizer);


            // Easing Setup
            string[] easings = new string[] { "BounceIn", "BounceOut", "CubicIn", "CubicInOut", "CubicOut", "Linear", "SinIn", "SinInOut", "SinOut", "SpringIn", "SpringOut" };
            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += async (s, e) =>
            {
                // handle the tap
                var easing1 = await DisplayActionSheet("Easing", "Cancel", "Ok", easings);
                if (easing1 != null)
                {
                    labelEasing.Text = easing1;
                    switch (easing1)
                    {
                        case "BounceIn":
                            easing = Easing.BounceIn;
                            easingBack = Easing.BounceOut;
                            break;
                        case "BounceOut":
                            easing = Easing.BounceOut;
                            easingBack = Easing.BounceIn;
                            break;
                        case "CubicIn":
                            easing = Easing.CubicIn;
                            easingBack = Easing.CubicOut;
                            break;
                        case "CubicInOut":
                            easing = Easing.CubicInOut;
                            easingBack = Easing.CubicInOut;
                            break;
                        case "CubicOut":
                            easing = Easing.CubicOut;
                            easingBack = Easing.CubicIn;
                            break;
                        case "Linear":
                            easing = Easing.Linear;
                            easingBack = Easing.Linear;
                            break;
                        case "SinIn":
                            easing = Easing.SinIn;
                            easingBack = Easing.SinOut;
                            break;
                        case "SinInOut":
                            easing = Easing.SinInOut;
                            easingBack = Easing.SinInOut;
                            break;
                        case "SinOut":
                            easing = Easing.SinOut;
                            easingBack = Easing.SinIn;
                            break;
                        case "SpringIn":
                            easing = Easing.SpringIn;
                            easingBack = Easing.SpringOut;
                            break;
                        case "SpringOut":
                            easing = Easing.SpringOut;
                            easingBack = Easing.SpringIn;
                            break;
                    }
                }
            };
            labelEasing.GestureRecognizers.Add(tapGestureRecognizer1);


            // Control Setup
            string[] controls = new string[] { "Label", "Entry", "Button" };
            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) =>
            {
                // handle the tap
                var control1 = await DisplayActionSheet("Easing", "Cancel", "Ok", controls);
                if (control1 != null)
                {
                    control = control1;
                    labelControl.Text = control1;
                }
            };
            labelControl.GestureRecognizers.Add(tapGestureRecognizer2);


            buttonAnimate.Clicked += async (sender, e) =>
             {
                 await Task.Run(async () =>
                 {
                     await Animate();
                 });
             };
        }

        async Task Animate()
        {
            uint.TryParse(textSpeed.Text, out uint speed);
            int.TryParse(textDelay.Text, out int delay);
            //switch (control)
            //{
            //    case "Label":
            //await Task.Run(() =>
            //    labelTest.ScaleTo(scale, (uint)speed, easing)
            //);
            await Task.Delay(delay);
            //    await Task.Run(() =>
            //        labelTest.ScaleTo(1, (uint)speed, easingBack)
            //    );
            //    break;
            //case "Entry":
            //    await Task.Run(() =>
            //        textTest.ScaleTo(scale, (uint)speed, easing)
            //    );
            //    await Task.Delay(delay);
            //    await Task.Run(() =>
            //        textTest.ScaleTo(1, (uint)speed, easingBack)
            //    );
            //    break;
            //case "Button":
            //    await Task.Run(() =>
            //        buttonTest.ScaleTo(scale, (uint)speed, easing)
            //    );
            //    await Task.Delay(delay);
            //    await Task.Run(() =>
            //        buttonTest.ScaleTo(1, (uint)speed, easingBack)
            //    );
            //    break;
            //}
        }
    }
}