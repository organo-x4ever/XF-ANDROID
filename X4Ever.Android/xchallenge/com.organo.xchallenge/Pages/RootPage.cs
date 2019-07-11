
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Pages.Community;
using com.organo.xchallenge.Pages.HowItWorks;
using com.organo.xchallenge.Pages.MealPlan;
using com.organo.xchallenge.Pages.Media;
using com.organo.xchallenge.Pages.News;
using com.organo.xchallenge.Pages.OGX;
using com.organo.xchallenge.Pages.Profile;
using com.organo.xchallenge.Pages.Rewards;
using com.organo.xchallenge.Pages.Video;
using com.organo.xchallenge.Pages.YouTube;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Blog;
using Xamarin.Forms;
using com.organo.xchallenge.Pages.Miscellaneous;

namespace com.organo.xchallenge.Pages
{
    public class RootPage : MasterDetailPage
    {
        private Dictionary<MenuType, NavigationPage> Pages { get; set; }
        private MenuType LastMenuType { get; set; }
        private VisitedPages _visitedPages { get; set; }

        public VisitedPages VisitedPages
        {
            get { return _visitedPages; }
            set { _visitedPages = value; }
        }

        public RootPage()
        {
            LastMenuType = MenuType.Logout;
            Pages = new Dictionary<MenuType, NavigationPage>();
            Title = TextResources.XChallenge;
            Master = new MenuPage(this);
            BackgroundColor = Color.Transparent;
            BindingContext = new BaseViewModel(Navigation)
            {
                Title = TextResources.XChallenge,
                Icon = TextResources.logo_transparent
            };
            VisitedPages = new VisitedPages();
            Initial();
        }

        private async void Initial()
        {
            //setup home page
            await NavigateAsync(Pages.Count == 0 ? MenuType.MyProfile : Pages.LastOrDefault().Key);
        }

        private void SetDetailIfNull(Page page)
        {
            if (Detail == null && page != null)
            {
                Detail = page;
            }
        }

        public async Task NavigateAsync(MenuType id, bool backPressed = false)
        {
            try
            {
                Page newPage;
                if (!Pages.ContainsKey(id))
                {
                    switch (id)
                    {
                        case MenuType.MyProfile:
                            var page = new XNavigationPage(new ProfileEnhanced(this)
                            {
                                Title = TextResources.MainTabs_MyProfile,
                                Icon = new FileImageSource {File = TextResources.MainTabs_MyProfile_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.LatestNews:
                            page = new XNavigationPage(new BlogPage(this) //NewsPage(this)
                            {
                                Title = TextResources.MainTabs_LatestNews,
                                Icon = new FileImageSource {File = TextResources.MainTabs_LatestNews_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.HowItWorks:
                            page = new XNavigationPage(new HowItWorksPage(this)
                            {
                                Title = TextResources.MainTabs_HowItWorks,
                                Icon = new FileImageSource {File = TextResources.MainTabs_HowItWorks_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.OgxSystem:
                            page = new XNavigationPage(new OgxSystemPage(this)
                            {
                                Title = TextResources.MainTabs_OGX_System,
                                Icon = new FileImageSource {File = TextResources.MainTabs_OGX_System_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.Rewards:
                            page = new XNavigationPage(new RewardsPage(this)
                            {
                                Title = TextResources.MainTabs_Rewards,
                                Icon = new FileImageSource { File = TextResources.MainTabs_Rewards_Icon }
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.MealOptions:
                            page = new XNavigationPage(new MealPlanPage(this)
                            {
                                Title = TextResources.MainTabs_Meal_Options,
                                Icon = new FileImageSource {File = TextResources.MainTabs_Meal_Options_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.Testimonials:
                            page = new XNavigationPage(new YoutubeTestimonialPage(this)
                            {
                                Title = TextResources.MainTabs_Testimonials,
                                Icon = new FileImageSource {File = TextResources.MainTabs_Testimonials_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.WorkoutVideos:
                            page = new XNavigationPage(new PlaylistPage(this)
                            {
                                Title = TextResources.MainTabs_WorkoutVideos,
                                Icon = new FileImageSource {File = TextResources.MainTabs_WorkoutVideos_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.MyMusic:
                            page = new XNavigationPage(new AudioPlayerPage(this)
                            {
                                Title = TextResources.MainTabs_MyMusic,
                                Icon = new FileImageSource {File = TextResources.MainTabs_MyMusic_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.Community:
                            page = new XNavigationPage(new CommunityPage(this)
                            {
                                Title = TextResources.MainTabs_Community,
                                Icon = new FileImageSource {File = TextResources.MainTabs_Community_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.Settings:
                            page = new XNavigationPage(new Settings(this)
                            {
                                Title = TextResources.MainTabs_Settings,
                                Icon = new FileImageSource {File = TextResources.MainTabs_Settings_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;
                            
                        case MenuType.More:
                            page = new XNavigationPage(new MiscContentPage(this)
                            {
                                Title = TextResources.MainTabs_More,
                                Icon = new FileImageSource {File = TextResources.MainTabs_More_Icon}
                            });
                            SetDetailIfNull(page);
                            Pages.Add(id, page);
                            break;

                        case MenuType.Logout:
                            await App.LogoutAsync();
                            App.GoToAccountPage();
                            return;
                    }
                }

                newPage = Pages[id];
                if (newPage == null)
                    return;

                // Remove page from root page loaded list
                if (id == MenuType.MyMusic ||
                    id == MenuType.WorkoutVideos ||
                    id == MenuType.Settings ||
                    id == MenuType.OgxSystem ||
                    id == MenuType.More)
                    Pages.Remove(id);

                //pop to root for Windows Phone
                // if (Detail != null && Device.RuntimePlatform == Device.WinPhone)
                // {
                    // await Detail.Navigation.PopToRootAsync();
                // }

                Detail = new Page();
                Detail = newPage;
                if (!backPressed)
                    VisitedPages.Add(id, true);
                MasterBehavior = MasterBehavior.Popover;
                if (Device.Idiom == TargetIdiom.Phone)
				{
					IsPresented = false;
                    MasterBehavior = MasterBehavior.Popover;
				}
                else if (Device.Idiom == TargetIdiom.Tablet)
				{
					IsPresented = false;
                    MasterBehavior = MasterBehavior.SplitOnLandscape;
				}
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(typeof(RootPage).FullName, ex);
            }
        }
    }

    public class XNavigationPage : NavigationPage
    {
        public XNavigationPage(Page root)
            : base(root)
        {
            Init();
        }

        public XNavigationPage()
        {
            Init();
        }

        private void Init()
        { 
            BarBackgroundColor = Palette._MainAccent;
            BarTextColor = Palette._MainBackground;
            Title = "My Profile";
            SetHasBackButton(CurrentPage, false);
            SetHasNavigationBar(CurrentPage, false);
        }
    }

    public enum MenuType
    {
        MyProfile,
        LatestNews,
        HowItWorks,
        OgxSystem,
        Rewards,
        MealOptions,
        Testimonials,
        WorkoutVideos,
        MyMusic,
        Community,
        Settings,
        More,
        Logout
    }

    public class HomeMenuItem : INotifyPropertyChanged
    {
        public HomeMenuItem()
        {
            MenuType = MenuType.MyProfile;
            IsIconVisible = true;
            IsSelected = false;
        }

        public string MenuIcon { get; set; }
        public ImageSource IconSource { get; set; }

        public MenuType MenuType { get; set; }

        public string MenuTitle { get; set; }

        public string MenuDetails { get; set; }

        public int MenuId { get; set; }

        public bool IsIconVisible { get; set; }

        public float IconWidth { get; set; }

        public float IconHeight { get; set; }
        public bool IsSelected { get; set; }
        public Style IconStyle { get; set; }
        public Thickness ItemPadding { get; set; }

        public bool IsVisible { get; set; }

        private Style _textStyle;
        public const string TextStylePropertyName = "TextStyle";

        public Style TextStyle
        {
            get => _textStyle;
            set => SetProperty(ref _textStyle, value, TextStylePropertyName);
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion INotifyPropertyChanging implementation

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /*public class RootPage : TabbedPage
    {
        public RootPage()
        {
             the Sales tab page
            Children.Add(
                new NavigationPage(new HomePage())
                {
                    Title = TextResources.MainTabs_Home,
                    Icon = new FileImageSource() { File = "SalesTab" }
                }
            );

             the Customers tab page
            Children.Add(
                new CustomersPage()
                {
                    BindingContext = new CustomersViewModel(Navigation),
                    Title = TextResources.MainTabs_Customers,
                    Icon = new FileImageSource() { File = "CustomersTab" }
                }
            );

             the Products tab page
            Children.Add(
                new NavigationPage(new CategoryListPage() { BindingContext = new CategoriesViewModel() } )
                {
                    Title = TextResources.MainTabs_Products,
                    Icon = new FileImageSource() { File = "ProductsTab" }
                }
            );
        }
    }*/
}