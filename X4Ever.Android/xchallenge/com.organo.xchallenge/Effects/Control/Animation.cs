using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;

[assembly: Dependency(typeof(com.organo.xchallenge.Effects.Control.Animation))]

namespace com.organo.xchallenge.Effects.Control
{
    public class Animation : IAnimation
    {
        public int LoadDelay { get; set; }
        public uint AnimationSpeed { get; set; }
        public Easing EasingIn { get; set; }
        public Easing EasingOut { get; set; }
        private List<ControlEntity> ControlList { get; set; }

        /// <summary>
        /// Animation for controls
        /// </summary>
        /// <param name="easingIn">
        /// Default is 'CubicIn'
        /// </param>
        /// <param name="easingOut">
        /// Default is 'CubicOut'
        /// </param>
        /// <param name="loadDelay">
        /// Default is '2'
        /// </param>
        /// <param name="animationSpeed">
        /// Default is '100'
        /// </param>
        public Animation()
        {
            EasingIn = Easing.CubicIn;
            EasingOut = Easing.CubicOut;
            LoadDelay = 2;
            AnimationSpeed = 100;
            ControlList = new List<ControlEntity>();
        }

        public async Task Add(View view)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (App.Configuration.IsAnimationAllowed)
                        view.Scale = 0;
                    ControlList.Add(new ControlEntity() { ControlView = view });
                });
            }
            catch (Exception ex)
            {
                new ExceptionHandler("Animation.cs", ex);
            }
        }

        public async Task Clear()
        {
            await Task.Run(() =>
            {
                ControlList = new List<ControlEntity>();
                ControlList.Clear();
            });
        }

        public async Task Animate(StackLayout viewParent)
        {
            // hide all children
            foreach (var view in viewParent.Children)
            {
                if (App.Configuration.IsAnimationAllowed)
                    view.Scale = 0;
            }

            // scale in the children for the panel
            foreach (var view in viewParent.Children)
            {
                if (App.Configuration.IsAnimationAllowed)
                {
                    await view.ScaleTo(1.2, 50, EasingIn);
                    await view.ScaleTo(1, 50, EasingOut);
                }
            }
        }

        public async Task Animate(List<ControlEntity> controlList)
        {
            // hide all children
            foreach (var control in controlList)
            {
                if (App.Configuration.IsAnimationAllowed)
                    control.ControlView.Scale = 0;
            }

            // scale in the children for the panel
            foreach (var control in controlList)
            {
                if (App.Configuration.IsAnimationAllowed)
                {
                    await control.ControlView.ScaleTo(1.2, 50, EasingIn);
                    await control.ControlView.ScaleTo(1, 50, EasingOut);
                }
            }
        }

        public async Task Animate()
        {
            // hide all children
            foreach (var control in ControlList)
            {
                if (App.Configuration.IsAnimationAllowed)
                    control.ControlView.Scale = 0;
            }

            // scale in the children for the panel
            foreach (var control in ControlList)
            {
                if (App.Configuration.IsAnimationAllowed)
                {
                    await control.ControlView.ScaleTo(1.2, 50, EasingIn);
                    await control.ControlView.ScaleTo(1, 50, EasingOut);
                }
            }
        }
    }
}