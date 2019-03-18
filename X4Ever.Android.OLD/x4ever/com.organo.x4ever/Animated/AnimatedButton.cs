using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Animated
{
    public class AnimatedButton : ContentView
    {
        private Label _textLabel;
        private StackLayout _layout;

        /// <summary>
        /// Creates a new instance of the animation button
        /// </summary>
        /// <param name="text">
        /// the text to set
        /// </param>
        /// <param name="callback">
        /// action to call when the animation is complete
        /// </param>
        public AnimatedButton(string text, Action callback = null)
        {
            // create the layout
            _layout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = 5,
            };

            // create the label
            _textLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                Text = text,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            _layout.Children.Add(_textLabel);

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async (o) =>
                 {
                     await this.ScaleTo(0.95, 50, Easing.CubicOut);
                     await this.ScaleTo(1, 50, Easing.CubicIn);
                     if (callback != null)
                         callback.Invoke();
                 })
            });

            // set the content
            Content = _layout;
        }

        /// <summary>
        /// Gets or sets the font size for the text
        /// </summary>
        public virtual double FontSize
        {
            get { return _textLabel.FontSize; }
            set
            {
                _textLabel.FontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the text color for the text
        /// </summary>
        public virtual Color TextColor
        {
            get { return _textLabel.TextColor; }
            set
            {
                _textLabel.TextColor = value;
            }
        }
    }
}