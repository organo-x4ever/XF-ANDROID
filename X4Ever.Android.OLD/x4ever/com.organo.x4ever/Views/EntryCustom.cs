using Xamarin.Forms;

namespace com.organo.x4ever.Views
{
    public class EntryCustom : Entry
    {
        public static readonly BindableProperty BorderStyleProperty = BindableProperty.Create("BorderStyle", typeof(string), typeof(EntryCustom), "");

        public string BorderStyle
        {
            get { return (string)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        /// <summary>
        /// The PlaceholderTextColor property
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(Entry), Color.White);

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>
        /// The color of the border.
        /// </value>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
    }
}