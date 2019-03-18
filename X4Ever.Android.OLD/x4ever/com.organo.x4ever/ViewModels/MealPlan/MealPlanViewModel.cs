using com.organo.x4ever.Controls;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Helpers;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.MealPlan
{
    public class MealPlanViewModel : BaseViewModel
    {
        private readonly IMealPlanService _mealPlanService;

        public MealPlanViewModel(INavigation navigation = null) : base(navigation)
        {
            _mealPlanService = DependencyService.Get<IMealPlanService>();
            this.MealPlanDetails = new List<MealPlanDetail>();
            MealOptionSelectedPreviously = MealOptionSelected.None;
            this.SetPageImageSize();
        }

        private async Task GetData(MealOptionSelected mealOptionSelected)
        {
            if (mealOptionSelected == MealOptionSelected.None)
                return;

            var mealPlanDetail = (await this.GetAsync()).FirstOrDefault(l =>
                l.MealTitleCompare.ToLower().Contains(mealOptionSelected.ToString().ToLower()));
            var accordionSources = new List<AccordionMultiViewSource>();
            var mealPlanOptionDetails =
                mealPlanDetail?.MealPlanOptionDetails.OrderBy(m => m.DisplaySequence).ThenBy(m => m.ID).ToList() ??
                new List<MealPlanOptionDetail>();

            await this.SetMealHeaderImage(mealPlanDetail);
            foreach (var mealPlanOptionDetail in mealPlanOptionDetails)
                switch (mealPlanDetail?.ViewType)
                {
                    case "list":
                        accordionSources.Add(await SetupMealPlanOptionList(mealPlanOptionDetail,
                            mealPlanOptionDetail.MealPlanOptionListDetails.OrderBy(m => m.DisplaySequence)
                                .ThenBy(m => m.ID).ToList(), mealPlanDetail.ViewType));
                        break;

                    case "grid":
                        accordionSources.Add(await SetupMealPlanOptionGrid(mealPlanOptionDetail,
                            mealPlanOptionDetail.MealPlanOptionGridDetails.OrderBy(m => m.DisplaySequence)
                                .ThenBy(m => m.ID).ToList(), mealPlanDetail.ViewType));
                        break;
                }

            this.AccordionSources = accordionSources;
        }

        private async Task<AccordionMultiViewSource> SetupMealPlanOptionList(MealPlanOptionDetail mealPlanOptionDetail,
            List<MealPlanOptionListDetail> mealPlanOptionListDetails, string viewType)
        {
            var contentListView = new ListView();
            await Task.Run(() =>
            {
                contentListView = new ListView()
                {
                    // Source of data items.
                    ItemsSource = mealPlanOptionListDetails,
                    Header = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(5, 0, 5, 0),
                        Children =
                        {
                            new Image()
                            {
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                Aspect = Aspect.AspectFill,
                                Source = mealPlanOptionDetail.MealOptionPhoto,
                                IsVisible = mealPlanOptionDetail.MealOptionPhoto != null,
                                Margin = new Thickness(10, 0, 10, 0)
                            },
                            new Label()
                            {
                                LineBreakMode = LineBreakMode.TailTruncation,
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItemTitle"],
                                Text = mealPlanOptionDetail.MealOptionSubtitle,
                                IsVisible = (mealPlanOptionDetail.MealOptionSubtitle != null &&
                                             mealPlanOptionDetail.MealOptionSubtitle.Trim().Length > 0)
                            },
                            new StackLayout()
                            {
                                HeightRequest = 1,
                                BackgroundColor = Palette._TitleTexts,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.End,
                                IsVisible = (mealPlanOptionDetail.MealOptionSubtitle != null &&
                                             mealPlanOptionDetail.MealOptionSubtitle.Trim().Length > 0)
                            }
                        }
                    },

                    // Define template for displaying each item. (Argument of DataTemplate
                    // constructor is called for each item; it must return a Cell derivative.)
                    ItemTemplate = new DataTemplate(() =>
                    {
                        // Create views with bindings for displaying each property.
                        Label titleLabel = new Label()
                        {
                            LineBreakMode = LineBreakMode.WordWrap,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItem"]
                        };
                        titleLabel.SetBinding(Label.TextProperty,
                            new Binding("MealOptionDetail", BindingMode.OneWay, null, null, "{0}"));

                        // Return an assembled ViewCell.
                        return new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Padding = new Thickness(5, 0, 5, 0),
                                Margin = new Thickness(0, 5, 0, 10),
                                Children =
                                {
                                    new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        VerticalOptions = LayoutOptions.Center,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Padding = new Thickness(5, 0, 5, 0),
                                        Children =
                                        {
                                            new StackLayout()
                                            {
                                                VerticalOptions = LayoutOptions.Start,
                                                HorizontalOptions = LayoutOptions.Start,
                                                BackgroundColor = Palette._TitleTexts,
                                                HeightRequest = 4,
                                                WidthRequest = 4,
                                                Margin = new Thickness(0, 8, 0, 0)
                                            },
                                            titleLabel
                                        },
                                    }
                                },
                            },
                        };
                    }),
                    SeparatorVisibility = SeparatorVisibility.None,
                    Margin = new Thickness(0),
                    VerticalOptions = LayoutOptions.Start,
                    RowHeight = 70,
                    HasUnevenRows = true,
                    BackgroundColor = Color.Transparent
                };
                contentListView.HeightRequest = (contentListView.RowHeight * (mealPlanOptionListDetails.Count)) + 65;
            });
            return new AccordionMultiViewSource()
            {
                HeaderText = mealPlanOptionDetail.MealOptionTitle,
                HeaderStyle = (Style) App.CurrentApp.Resources["buttonStyleGray"],
                HeaderSelectedStyle = (Style) App.CurrentApp.Resources["buttonStyle"],
                ViewType = viewType,
                ContentItems = contentListView
            };
        }

        private async Task<AccordionMultiViewSource> SetupMealPlanOptionGrid(MealPlanOptionDetail mealPlanOptionDetail,
            List<MealPlanOptionGridDetail> mealPlanOptionGridDetails, string viewType)
        {
            var contentListView = new ListView();
            await Task.Run(() =>
            {
                contentListView = new ListView()
                {
                    // Source of data items.
                    ItemsSource = mealPlanOptionGridDetails,
                    Header = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(5, 0, 5, 0),
                        Children =
                        {
                            new Image()
                            {
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Aspect = Aspect.AspectFill,
                                Source = mealPlanOptionDetail.MealOptionPhoto,
                                IsVisible = mealPlanOptionDetail.MealOptionPhoto != null
                            },
                            new Label()
                            {
                                LineBreakMode = LineBreakMode.TailTruncation,
                                VerticalOptions = LayoutOptions.End,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItemHeader"],
                                Text = mealPlanOptionDetail.MealOptionSubtitle,
                                IsVisible = (mealPlanOptionDetail.MealOptionSubtitle != null &&
                                             mealPlanOptionDetail.MealOptionSubtitle.Trim().Length > 0)
                            },
                            new StackLayout()
                            {
                                HeightRequest = 1,
                                BackgroundColor = Palette._TitleTexts,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.End,
                                IsVisible = (mealPlanOptionDetail.MealOptionSubtitle != null &&
                                             mealPlanOptionDetail.MealOptionSubtitle.Trim().Length > 0)
                            }
                        }
                    },

                    // Define template for displaying each item. (Argument of DataTemplate
                    // constructor is called for each item; it must return a Cell derivative.)
                    ItemTemplate = new DataTemplate(() =>
                    {
                        // Create views with bindings for displaying each property.
                        Label mealOptionVolumeLabel = new Label()
                        {
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItem"],
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand
                        };
                        mealOptionVolumeLabel.SetBinding(Label.TextProperty,
                            new Binding("MealOptionVolume", BindingMode.OneWay, null, null, "{0}"));

                        Label mealOptionVolumeTypeLabel = new Label()
                        {
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItem"],
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand
                        };
                        mealOptionVolumeTypeLabel.SetBinding(Label.TextProperty,
                            new Binding("MealOptionVolumeType", BindingMode.OneWay, null, null, "{0}"));

                        Label titleLabel = new Label()
                        {
                            LineBreakMode = LineBreakMode.TailTruncation,
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItem"],
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand
                        };
                        titleLabel.SetBinding(Label.TextProperty,
                            new Binding("MealOptionShakeTitle", BindingMode.OneWay, null, null, "{0}"));

                        var grid = new Grid()
                        {
                            RowDefinitions =
                            {
                                new RowDefinition() {Height = GridLength.Auto}
                            },
                            ColumnDefinitions =
                            {
                                new ColumnDefinition() {Width = 25},
                                new ColumnDefinition() {Width = 50},
                                new ColumnDefinition() {Width = GridLength.Star}
                            },
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        };

                        grid.Children.Add(mealOptionVolumeLabel, 0, 0);
                        grid.Children.Add(mealOptionVolumeTypeLabel, 1, 0);
                        grid.Children.Add(titleLabel, 2, 0);

                        // Return an assembled ViewCell.
                        return new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Padding = new Thickness(5, 0, 0, 0),
                                Margin = new Thickness(0, 5, 0, 10),
                                Children =
                                {
                                    grid
                                },
                            },
                        };
                    }),
                    SeparatorVisibility = SeparatorVisibility.None,
                    Margin = new Thickness(10, 0, 10, 0),
                    VerticalOptions = LayoutOptions.Start,
                    RowHeight = 25,
                    HasUnevenRows = false,
                    BackgroundColor = Color.Transparent
                };

                contentListView.HeightRequest =
                    (contentListView.RowHeight * (mealPlanOptionDetail.MealPlanOptionGridDetails.Count)) + 15;

                //contentListView.HeightRequest = Elements.Length * contentListView.RowHeight;
                //listView.HeightRequest = listView.RowHeight * ((Your List that you want to show in ListView).Count + 1);
            });
            return new AccordionMultiViewSource()
            {
                HeaderText = mealPlanOptionDetail.MealOptionTitle,
                HeaderStyle = (Style) App.CurrentApp.Resources["labelStyleInfoHeading"],
                HeaderSelectedStyle = (Style) App.CurrentApp.Resources["labelStyleInfoHighlight"],

                HeaderImage = TextResources.icon_plus_gray,
                HeaderImageStyle = (Style) App.CurrentApp.Resources["imagePopupClose"],
                HeaderImageSelected = TextResources.icon_plus,

                ViewType = viewType,
                ContentItems = contentListView
            };
        }

        private async Task<List<MealPlanDetail>> GetAsync()
        {
            if (this.MealPlanDetails.Count == 0)
                this.MealPlanDetails = await this._mealPlanService.GetDetailAsync();
            return this.MealPlanDetails.OrderBy(m => m.DisplaySequence).ThenBy(m => m.ID).ToList();
        }

        private async Task SetMealHeaderImage(MealPlanDetail mealPlanDetail)
        {
            MealHeaderImage = mealPlanDetail?.MealPlanPhoto;
            MealHeaderImageExists = mealPlanDetail?.MealPlanPhoto != null;
            await Task.Delay(1);
        }

        private List<MealPlanDetail> _mealPlanDetails;
        public const string MealPlanDetailsPropertyName = "MealPlanDetails";

        public List<MealPlanDetail> MealPlanDetails
        {
            get { return _mealPlanDetails; }
            set { SetProperty(ref _mealPlanDetails, value, MealPlanDetailsPropertyName); }
        }

        private List<AccordionMultiViewSource> _accordionSources;
        public const string AccordionSourcesPropertyName = "AccordionSources";

        public List<AccordionMultiViewSource> AccordionSources
        {
            get { return _accordionSources; }
            set { SetProperty(ref _accordionSources, value, AccordionSourcesPropertyName, ExecuteBind); }
        }

        //public Style ButtonStyleDefault = (Style)App.CurrentApp.Resources["buttonAccordion"];
        //public Style ButtonStyleSelected = (Style)App.CurrentApp.Resources["buttonAccordionHighlight"];

        public Style LabelStyleDefault = (Style) App.CurrentApp.Resources["labelStyleDark"];
        public Style LabelStyleSelected = (Style) App.CurrentApp.Resources["labelStyleHighlight"];

        private ICommand _fullMealsCommand;

        public ICommand FullMealsCommand
        {
            get
            {
                return _fullMealsCommand ?? (_fullMealsCommand = new Command(async (obj) =>
                {
                    await UpdateMealOptionSelected(MealOptionSelected.FullMeals);
                }));
            }
        }

        private ICommand _shakesCommand;

        public ICommand ShakesCommand
        {
            get
            {
                return _shakesCommand ?? (_shakesCommand = new Command(async (obj) =>
                {
                    await UpdateMealOptionSelected(MealOptionSelected.Shakes);
                }));
            }
        }

        private ICommand _proMealsCommand;

        public ICommand ProMealsCommand
        {
            get
            {
                return _proMealsCommand ?? (_proMealsCommand = new Command(async (obj) =>
                {
                    await UpdateMealOptionSelected(MealOptionSelected.ProMeals);
                }));
            }
        }

        public async Task UpdateMealOptionSelected(MealOptionSelected MealOptionSelected = MealOptionSelected.None)
        {
            if (MealOptionSelectedPreviously != MealOptionSelected.None &&
                MealOptionSelectedPreviously == MealOptionSelected)
                return;
            this.FullMealsStyle = this.LabelStyleDefault;
            this.ShakesStyle = this.LabelStyleDefault;
            this.ProMealsStyle = this.LabelStyleDefault;

            this.FullMealsImage = TextResources.icon_full_meals_gray;
            this.ShakesImage = TextResources.icon_shakes_gray;
            this.ProMealsImage = TextResources.icon_pro_meals_gray;

            switch (MealOptionSelected)
            {
                case MealOptionSelected.FullMeals:
                    this.FullMealsStyle = this.LabelStyleSelected;
                    this.FullMealsImage = TextResources.icon_full_meals;
                    break;

                case MealOptionSelected.Shakes:
                    this.ShakesStyle = this.LabelStyleSelected;
                    this.ShakesImage = TextResources.icon_shakes;
                    break;

                case MealOptionSelected.ProMeals:
                    this.ProMealsStyle = this.LabelStyleSelected;
                    this.ProMealsImage = TextResources.icon_pro_meals;
                    break;
            }

            MealOptionSelectedPreviously = MealOptionSelected;
            await this.GetData(MealOptionSelected);
        }

        private MealOptionSelected MealOptionSelectedPreviously { get; set; }

        private Style _fullMealsStyle;
        public const string FullMealsStylePropertyName = "FullMealsStyle";

        public Style FullMealsStyle
        {
            get { return _fullMealsStyle; }
            set { SetProperty(ref _fullMealsStyle, value, FullMealsStylePropertyName); }
        }

        private Style _shakesStyle;
        public const string ShakesStylePropertyName = "ShakesStyle";

        public Style ShakesStyle
        {
            get { return _shakesStyle; }
            set { SetProperty(ref _shakesStyle, value, ShakesStylePropertyName); }
        }

        private Style _proMealsStyle;
        public const string ProMealsStylePropertyName = "ProMealsStyle";

        public Style ProMealsStyle
        {
            get { return _proMealsStyle; }
            set { SetProperty(ref _proMealsStyle, value, ProMealsStylePropertyName); }
        }

        private string _fullMealsImage;
        public const string FullMealsImagePropertyName = "FullMealsImage";

        public string FullMealsImage
        {
            get { return _fullMealsImage; }
            set { SetProperty(ref _fullMealsImage, value, FullMealsImagePropertyName, ChangeFullMealsImage); }
        }

        private void ChangeFullMealsImage()
        {
            if (this.FullMealsImage != null && this.FullMealsImage.Trim().Length > 0 &&
                !this.FullMealsImage.Trim().ToLower().Contains("null"))
                this.FullMealsImageSource = ImageResizer.ResizeImage(this.FullMealsImage, this.MealImageSize);
        }

        private ImageSource _fullMealsImageSource;
        public const string FullMealsImageSourcePropertyName = "FullMealsImageSource";

        public ImageSource FullMealsImageSource
        {
            get { return _fullMealsImageSource; }
            set { SetProperty(ref _fullMealsImageSource, value, FullMealsImageSourcePropertyName); }
        }

        private string _shakesImage;
        public const string ShakesImagePropertyName = "ShakesImage";

        public string ShakesImage
        {
            get { return _shakesImage; }
            set { SetProperty(ref _shakesImage, value, ShakesImagePropertyName, ChangeShakesImage); }
        }

        private void ChangeShakesImage()
        {
            if (this.ShakesImage != null && this.ShakesImage.Trim().Length > 0 &&
                !this.ShakesImage.Trim().ToLower().Contains("null"))
                this.ShakesImageSource = ImageResizer.ResizeImage(this.ShakesImage, this.MealImageSize);
        }

        private ImageSource _shakesImageSource;
        public const string ShakesImageSourcePropertyName = "ShakesImageSource";

        public ImageSource ShakesImageSource
        {
            get { return _shakesImageSource; }
            set { SetProperty(ref _shakesImageSource, value, ShakesImageSourcePropertyName); }
        }

        private string _proMealsImage;
        public const string ProMealsImagePropertyName = "ProMealsImage";

        public string ProMealsImage
        {
            get { return _proMealsImage; }
            set { SetProperty(ref _proMealsImage, value, ProMealsImagePropertyName, ChangeProMealsImage); }
        }

        private void ChangeProMealsImage()
        {
            if (this.ProMealsImage != null && this.ProMealsImage.Trim().Length > 0 &&
                !this.ProMealsImage.Trim().ToLower().Contains("null"))
                this.ProMealsImageSource = ImageResizer.ResizeImage(this.ProMealsImage, this.MealImageSize);
        }

        private ImageSource _proMealsImageSource;
        public const string ProMealsImageSourcePropertyName = "ProMealsImageSource";

        public ImageSource ProMealsImageSource
        {
            get { return _proMealsImageSource; }
            set { SetProperty(ref _proMealsImageSource, value, ProMealsImageSourcePropertyName); }
        }

        private string _mealHeaderImage;
        public const string MealHeaderImagePropertyName = "MealHeaderImage";

        public string MealHeaderImage
        {
            get { return _mealHeaderImage; }
            set { SetProperty(ref _mealHeaderImage, value, MealHeaderImagePropertyName, ChangeMealHeaderImage); }
        }

        private void ChangeMealHeaderImage()
        {
            var mealHeader = App.Configuration.GetImageSizeByID(ImageIdentity.MEAL_PLAN_PAGE_MEAL_HEADER);
            this.MealHeaderImageSource = ImageResizer.ResizeImage(this.MealHeaderImage, mealHeader);
            if (mealHeader != null)
            {
                this.MealPlanHeaderHeight = mealHeader.Height;
                this.MealPlanHeaderWidth = mealHeader.Width;
            }
        }

        private ImageSource _mealHeaderImageSource;
        public const string MealHeaderImageSourcePropertyName = "MealHeaderImageSource";

        public ImageSource MealHeaderImageSource
        {
            get { return _mealHeaderImageSource; }
            set { SetProperty(ref _mealHeaderImageSource, value, MealHeaderImageSourcePropertyName); }
        }

        private bool _mealHeaderImageExists;
        public const string MealHeaderImageExistsPropertyName = "MealHeaderImageExists";

        public bool MealHeaderImageExists
        {
            get { return _mealHeaderImageExists; }
            set { SetProperty(ref _mealHeaderImageExists, value, MealHeaderImageExistsPropertyName); }
        }

        public Action BindDataSourceAction { get; set; }

        private void ExecuteBind()
        {
            BindDataSourceAction?.Invoke();
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    this.Root.IsPresented = this.Root.IsPresented == false;
                }));
            }
        }

        private void SetPageImageSize()
        {
            this.MealImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.MEAL_PLAN_PAGE_MEAL_IMAGE);
            if (this.MealImageSize != null)
            {
                this.MealPlanImageHeight = this.MealImageSize.Height;
                this.MealPlanImageWidth = this.MealImageSize.Width;
            }
        }

        private ImageSize MealImageSize { get; set; }

        private float mealPlanImageHeight;
        public const string MealPlanImageHeightPropertyName = "MealPlanImageHeight";

        public float MealPlanImageHeight
        {
            get { return mealPlanImageHeight; }
            set { SetProperty(ref mealPlanImageHeight, value, MealPlanImageHeightPropertyName); }
        }

        private float mealPlanImageWidth;
        public const string MealPlanImageWidthPropertyName = "MealPlanImageWidth";

        public float MealPlanImageWidth
        {
            get { return mealPlanImageWidth; }
            set { SetProperty(ref mealPlanImageWidth, value, MealPlanImageWidthPropertyName); }
        }

        private float mealPlanHeaderHeight;
        public const string MealPlanHeaderHeightPropertyName = "MealPlanHeaderHeight";

        public float MealPlanHeaderHeight
        {
            get { return mealPlanHeaderHeight; }
            set { SetProperty(ref mealPlanHeaderHeight, value, MealPlanHeaderHeightPropertyName); }
        }

        private float mealPlanHeaderWidth;
        public const string MealPlanHeaderWidthPropertyName = "MealPlanHeaderWidth";

        public float MealPlanHeaderWidth
        {
            get { return mealPlanHeaderWidth; }
            set { SetProperty(ref mealPlanHeaderWidth, value, MealPlanHeaderWidthPropertyName); }
        }
    }

    public enum MealOptionSelected
    {
        None,
        FullMeals,
        Shakes,
        ProMeals
    }

    public class DataViewCell : ViewCell
    {
        public DataViewCell()
        {
            var label = new Label();
            label.SetBinding(Label.TextProperty, new Binding("TextValue"));
            label.SetBinding(Label.ClassIdProperty, new Binding("DataValue"));
            View = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(12, 8),
                Children = {label}
            };
        }
    }

    public class MealPlanObject
    {
        public string TextValue { get; set; }
        public string DataValue { get; set; }
        public MealPlanDetail MealPlanDetails { get; set; }
    }
}