using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Milestones;
using System;
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.MilestonePages
{
    public partial class BasicMilestonePage : BasicMilestonePageXaml
    {
        private MilestoneViewModel _model;

        public BasicMilestonePage(MilestoneViewModel model)
        {
            try
            {
                InitializeComponent();
                _model = model;
                BindingContext = _model;
                sliderCurrentWeight.ValueChanged += (sender, e) =>
                {
                    if ((short) e.NewValue < App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE)
                        sliderCurrentWeight.Value = _model.CurrentWeightValue;
                    else
                        _model.CurrentWeightValue = (short) e.NewValue;
                };
                _model.ViewComponents.Add(sliderCurrentWeight);
                sliderCurrentWeight.SetMinValueAsync(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }

    public abstract class BasicMilestonePageXaml : ModelBoundContentPage<MilestoneViewModel> { }
}