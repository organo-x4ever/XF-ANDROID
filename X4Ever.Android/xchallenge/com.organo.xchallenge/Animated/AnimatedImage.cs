
using com.organo.xchallenge.Views;
using System;
using System.ComponentModel;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Animated
{
    public class AnimatedImage : ContentView
    {
        private FloatingActionButtonView _floatingImageButton;
        private Label _textLabel;
        private StackLayout _layout;

        /// <summary>
        /// Gets or sets the text color for the text
        /// </summary>
        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(AnimatedImage),
            propertyChanged: OnTextColorChanged,
            defaultValue: Color.White
        );

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AnimatedImage) bindable;
            if (control != null)
            {
                if (newValue is Color textColor)
                {
                    if (control._textLabel != null)
                        control._textLabel.TextColor = textColor;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font size for the text
        /// </summary>
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            propertyName: nameof(FontSize),
            returnType: typeof(double),
            declaringType: typeof(AnimatedImage),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnFontSizeChanged,
            defaultValue: (double)Device.GetNamedSize(NamedSize.Micro, typeof(Label)));

        private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AnimatedImage) bindable;
            if (control != null)
            {
                if (newValue is double fontSize)
                {
                    control._textLabel.FontSize = fontSize;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the animation image
        /// </summary>
        /// <param name="floatingImageButton">FloatingActionButtonView instance</param>
        /// <param name="callback">Action to call when the animation is complete</param>
        public AnimatedImage(FloatingActionButtonView floatingImageButton, Action callback = null)
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
            _floatingImageButton = new FloatingActionButtonView()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            _floatingImageButton = floatingImageButton;
            _layout.Children.Add(_floatingImageButton);

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
        /// Creates a new instance of the animation image
        /// </summary>
        /// <param name="floatingImageButton">FloatingActionButtonView instance</param>
        /// <param name="showLabel">If label exists under image button set true otherwise false</param>
        /// /// <param name="labelText">If label exists the set text for label</param>
        /// /// <param name="colorText">If label exists then set label text color</param>
        /// <param name="callback">Action to call when the animation is complete</param>
        public AnimatedImage(FloatingActionButtonView floatingImageButton, bool showLabel, string labelText, Action callback = null)
        {
            // create the layout
            _layout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = 5,
            };

            // create the label
            _floatingImageButton = new FloatingActionButtonView()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            _floatingImageButton = floatingImageButton;
            _layout.Children.Add(_floatingImageButton);

            if (showLabel)
            {
                // create the label
                _textLabel = new Label
                {
                    FontSize = FontSize,
                    Text = labelText,
                    TextColor = TextColor,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                _layout.Children.Add(_textLabel);
            }

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
        /// Creates a new instance of the animation image
        /// </summary>
        /// <param name="imageName">Image name as simple text</param>
        /// <param name="colorNormal">Normal look Background Color</param>
        /// <param name="colorPressed">Pressed look Background Color</param>
        /// <param name="colorRipple">Ripple look Background Color</param>
        /// <param name="size">Image Button Size either normal or mini</param>
        /// <param name="hasShadow">Does Image Button has shadow</param>
        /// <param name="callback">
        /// Action to call when the animation is complete
        /// </param>
        public AnimatedImage(string imageName, Color colorNormal, Color colorPressed, Color colorRipple,
            FloatingActionButtonSize size, bool hasShadow = false, Action callback = null)
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
            _floatingImageButton = new FloatingActionButtonView()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ImageName = imageName,
                ColorNormal = colorNormal,
                ColorPressed = colorPressed,
                ColorRipple = colorRipple,
                HasShadow = hasShadow,
                Size = size
            };
            _layout.Children.Add(_floatingImageButton);

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
    }
}