using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Controls
{
    public class CheckBox : ContentView
    {
        private Label lbl = new Label() { Text = "\u2610" }; // \u2610 Uni code will show empty box
        private Label lbl1 = new Label() { Text = "" };

        public CheckBox()
        {
            //Padding = Device.OnPlatform(new Thickness(0, 20, 0, 0),
            //         new Thickness(0),
            //         new Thickness(0));

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.Tapped += OnTapped;
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };
            stackLayout.Children.Add(lbl);
            stackLayout.Children.Add(lbl1);
            stackLayout.GestureRecognizers.Add(t);
            Content = stackLayout;
        }

        public string Text { get { return lbl1.Text; } set { lbl1.Text = value; } }
        public Color TextColor { get { return lbl1.TextColor; } set { lbl.TextColor = lbl1.TextColor = value; } }
        public TextAlignment HorizontalTextAlignment { get { return lbl1.HorizontalTextAlignment; } set { lbl.HorizontalTextAlignment = lbl1.HorizontalTextAlignment = value; } }
        public string FontFamily { get { return lbl1.FontFamily; } set { lbl1.FontFamily = value; } }
        public double FontSize { get { return lbl1.FontSize; } set { lbl1.FontSize = value; lbl.FontSize = lbl1.FontSize + 2; } }
        public bool Checked { get { return lbl.Text == "\u2611"; } set { if (value) lbl.Text = "\u2611"; else lbl.Text = "\u2610"; } }
        public ColumnDefinition ColumnDefinition { get; set; }

        private void OnTapped(object sender, EventArgs e)
        {
            //lbl.Text = lbl.Text == "\u2611" ? "\u2610" : "\u2611"; // \u2611 Uni code will show checked Box
            Checked = Checked == false;
        }
    }
}