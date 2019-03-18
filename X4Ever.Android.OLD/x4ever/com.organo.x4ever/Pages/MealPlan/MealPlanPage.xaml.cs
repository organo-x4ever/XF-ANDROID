using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.MealPlan;
using System;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.MealPlan
{
    public partial class MealPlanPage : MealPlanPageXaml
    {
        private MealPlanViewModel _model;

        public MealPlanPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                this._model = new MealPlanViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    BindDataSourceAction = () =>
                    {
                        AccordionMain.DataSource = this._model.AccordionSources;
                        AccordionMain.DataBind();
                    },
                };
                BindingContext = this._model;
                AccordionMain.FirstExpaned = true;
                this.Page_Load();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        protected async void Page_Load()
        {
            await this._model.UpdateMealOptionSelected(MealOptionSelected.FullMeals);

        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class MealPlanPageXaml : ModelBoundContentPage<MealPlanViewModel>
    {
    }
}