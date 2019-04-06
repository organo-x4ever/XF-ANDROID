using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class MenuUser : Grid
    {
        //private static string _applicationVersion;

        //public string ApplicationVersion
        //{
        //    get
        //    {
        //        _applicationVersion = (string) GetValue(ApplicationVersionProperty);
        //        return _applicationVersion;
        //    }
        //    set
        //    {
        //        _applicationVersion = value;
        //        SetValue(ApplicationVersionProperty, _applicationVersion);
        //    }
        //}

        //public static readonly BindableProperty ApplicationVersionProperty = BindableProperty.Create(
        //    nameof(ApplicationVersion),
        //    typeof(string), typeof(GridMenu), null, BindingMode.OneWay, null);

        //public List<HomeMenuItem> Source
        //{
        //    get => (List<HomeMenuItem>) GetValue(SourceProperty);
        //    set => SetValue(SourceProperty, value);
        //}

        //public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
        //    typeof(List<HomeMenuItem>), typeof(GridMenu), null, BindingMode.TwoWay, null, OnGridMenuChanged);

        //private static void OnGridMenuChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (GridMenu) bindable;
        //    if (control != null)
        //    {
        //        if (newValue is List<HomeMenuItem> menuItems)
        //        {
        //            var rowNumber = -1;
        //            menuItems.Select(m =>
        //            {
        //                rowNumber++;
        //                control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
        //                control.Children.Add(new StackLayout()
        //                {
        //                    Padding = new Thickness(15, 5, 0, 5),
        //                    Orientation = StackOrientation.Horizontal,
        //                    VerticalOptions = LayoutOptions.StartAndExpand,
        //                    BackgroundColor = Palette._Transparent,
        //                    Children =
        //                    {
        //                        new Image()
        //                        {
        //                            Source = m.IconSource,
        //                            Style = (Style) App.CurrentApp.Resources["imageIconMenuItem"],
        //                            VerticalOptions = LayoutOptions.Center,
        //                            IsVisible = m.IsIconVisible,
        //                            BackgroundColor = Palette._Transparent
        //                        },
        //                        new Label()
        //                        {
        //                            Text = m.MenuTitle,
        //                            Style = (Style) App.CurrentApp.Resources["labelStyleMenuItem"],
        //                            VerticalOptions = LayoutOptions.Center,
        //                            BackgroundColor = Palette._Transparent
        //                        }
        //                    }
        //                }, 0, rowNumber);

        //                return m;
        //            });
        //        }
        //    }

        //    //<Grid HorizontalOptions="FillAndExpand" VerticalOptions="CenterAndExpand" Margin="0,0,0,6" BackgroundColor="{x:Static statics:Palette._Transparent}">
        //    //    <Grid.ColumnDefinitions>
        //    //    <ColumnDefinition Width="*" />
        //    //    </Grid.ColumnDefinitions>
        //    //    <Grid.RowDefinitions>
        //    //    <RowDefinition Height="Auto" />
        //    //    <RowDefinition Height="Auto" />
        //    //    <RowDefinition Height="Auto" />
        //    //    <RowDefinition Height="1" />
        //    //    </Grid.RowDefinitions>
        //    //    <StackLayout Grid.Row="0" HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
        //    //    <controls:CircleImage BorderThickness="2" HorizontalOptions="Center" VerticalOptions="Center" Source="{Binding ProfileImageSource}" Style="{DynamicResource imageMenuUser}" Margin="0,8,0,0" BorderColor="{x:Static statics:Palette._MainAccent}"
        //    //HeightRequest="{Binding ProfileImageHeight}" WidthRequest="{Binding ProfileImageWidth}">
        //    //    <Image.GestureRecognizers>
        //    //    <TapGestureRecognizer Tapped="ChangeProfilePhoto" NumberOfTapsRequired="1" />
        //    //    </Image.GestureRecognizers>
        //    //    </controls:CircleImage>
        //    //    </StackLayout>
        //    //    <Label Grid.Row="1" Text="{translate:Translate ChangeSmall}" Style="{DynamicResource labelStyleLinkHighlight}" HorizontalOptions="Center" Margin="0,-5,0,0">
        //    //    <Label.GestureRecognizers>
        //    //    <TapGestureRecognizer Tapped="ChangeProfilePhoto" NumberOfTapsRequired="1" />
        //    //    </Label.GestureRecognizers>
        //    //    </Label>
        //    //    <Label Grid.Row="2" Text="{Binding User.FullName}" Style="{DynamicResource labelStyleMenuHeader}" HorizontalOptions="Center" VerticalOptions="Center" Margin="0,-5,0,5" />
        //    //    <StackLayout Grid.Row="3" BackgroundColor="{x:Static statics:Palette._White}"></StackLayout>
        //    //    </Grid>
        //}
    }
}