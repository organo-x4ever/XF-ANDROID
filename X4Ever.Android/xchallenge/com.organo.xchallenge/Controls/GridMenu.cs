using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class GridMenu : Grid
    {
        public List<HomeMenuItem> Source
        {
            get => (List<HomeMenuItem>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
            typeof(List<HomeMenuItem>), typeof(GridMenu), null, BindingMode.TwoWay, null, OnGridMenuChanged);

        public HomeMenuItem SelectedItem { get; set; }

        public event EventHandler ItemSelectedHandler;

        protected virtual void OnItemSelected(EventArgs e)
        {
            EventHandler handler = ItemSelectedHandler;
            handler?.Invoke(this, e);
        }

        public void Rebind(object sender, List<HomeMenuItem> menuItems) =>
            OnGridMenuChanged((BindableObject) sender, null, menuItems);

        private static async void OnGridMenuChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (GridMenu) bindable;
            if (control != null)
            {
                control.Children.Clear();
                control.SelectedItem = null;
                if (newValue is List<HomeMenuItem> menuItems)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                    var rowNumber = -1;
                    foreach (var menuItem in menuItems)
                    {
                        var image = new Image()
                        {
                            Source = menuItem.IconSource,
                            Style = menuItem.IconStyle,
                            VerticalOptions = LayoutOptions.Center,
                            IsVisible = menuItem.IsIconVisible,
                            BackgroundColor = Palette._Transparent
                        };
                        var label = new Label()
                        {
                            Text = menuItem.MenuTitle,
                            Style = menuItem.TextStyle,
                            VerticalOptions = LayoutOptions.Center,
                            BackgroundColor = Palette._Transparent
                        };

                        var stackLayout = new StackLayout()
                        {
                            Padding = menuItem.ItemPadding,
                            Orientation = StackOrientation.Horizontal,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            BackgroundColor = Palette._Transparent,
                            Children =
                            {
                                image, label
                            }
                        };

                        var gestureRecognizer = new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                control.SelectedItem = menuItem;
                                control.OnItemSelected(EventArgs.Empty);
                            })
                        };
                        stackLayout.GestureRecognizers.Add(gestureRecognizer);

                        rowNumber++;
                        control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                        control.Children.Add(stackLayout, 0, rowNumber);
                    }
                }
            }
        }
    }
}