using com.organo.xchallenge.Localization;
using com.organo.xchallenge.ViewModels.Base;
using System;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Extensions;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Account
{
    public class PersonalInfoViewModel : BaseViewModel
    {
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public PersonalInfoViewModel(INavigation navigation = null) : base(navigation)
        {
            AgeMinimumValue = 0;
            AgeMaximumValue = App.Configuration.AppConfig.MAXIMUM_AGE;
            AgeValue = AgeMinimumValue;

            CurrentWeightMinimumValue = 0;
            CurrentWeightMaximumValue =
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MAXIMUM_CURRENT_WEIGHT_KG,
                    App.Configuration.AppConfig.MAXIMUM_CURRENT_WEIGHT_LB);
            CurrentWeightValue = CurrentWeightMinimumValue;

            WeightLossGoalMinimumValue = 0;
            WeightLossGoalMaximumValue =
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MAXIMUM_WEIGHT_LOSE_KG,
                    App.Configuration.AppConfig.MAXIMUM_WEIGHT_LOSE_LB);
            WeightLossGoalValue = WeightLossGoalMinimumValue;
        }

        public void SetSliders()
        {
            SliderAgeModel.ValueChanged += (sender, e) =>
            {
                if ((short) e.NewValue < App.Configuration.AppConfig.MINIMUM_AGE)
                    SliderAgeModel.Value = AgeValue;
                else
                    AgeValue = (short) e.NewValue;
            };
            SliderCurrentWeightModel.ValueChanged += (sender, e) =>
            {
                if (e.NewValue < _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                        App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB))
                    SliderCurrentWeightModel.Value = CurrentWeightValue;
                else
                {
                    double.TryParse(e.NewValue.ToString("0"), out double v);
                    CurrentWeightValue = v;
                }
            };
            SliderWeightLossGoalModel.ValueChanged += (sender, e) =>
            {
                if (e.NewValue < _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                        App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB))
                    SliderWeightLossGoalModel.Value = WeightLossGoalValue;
                else
                {
                    double.TryParse(e.NewValue.ToString("0"), out double v);
                    WeightLossGoalValue = v;
                }
            };

            SliderAgeModel.SetMinValueAsync(App.Configuration.AppConfig.MINIMUM_AGE, delay: 2);
            SliderCurrentWeightModel.SetMinValueAsync(
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                    App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB), delay: 2);
            SliderWeightLossGoalModel.SetMinValueAsync(
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                    App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB), delay: 2);
        }

        public string YourCurrentWeightText => string.Format(TextResources.YourCurrentWeightFormat1,
            App.Configuration.AppConfig.DefaultWeightVolume);

        public string WeightLossGoalMinText => string.Format(TextResources.WeightLossGoalMinFormat2,
            _converter.DisplayWeightVolume((double) App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                (double) App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB),
            App.Configuration.AppConfig.DefaultWeightVolume);


        private Slider _sliderAgeModel;

        public const string SliderAgeModelPropertyName = "SliderAgeModel";

        // CHANGED
        public Slider SliderAgeModel
        {
            get { return _sliderAgeModel; }
            set { SetProperty(ref _sliderAgeModel, value, SliderAgeModelPropertyName); }
        }

        private Int16 ageValue = 0;
        public const string AgeValuePropertyName = "AgeValue";

        public Int16 AgeValue
        {
            get { return ageValue; }
            set { SetProperty(ref ageValue, value, AgeValuePropertyName); }
        }

        private Int16 ageMaximumValue = 0;
        public const string AgeMaximumValuePropertyName = "AgeMaximumValue";

        public Int16 AgeMaximumValue
        {
            get { return ageMaximumValue; }
            set { SetProperty(ref ageMaximumValue, value, AgeMaximumValuePropertyName); }
        }

        private Int16 ageMinimumValue = 0;
        public const string AgeMinimumValuePropertyName = "AgeMinimumValue";

        public Int16 AgeMinimumValue
        {
            get { return ageMinimumValue; }
            set { SetProperty(ref ageMinimumValue, value, AgeMinimumValuePropertyName); }
        }


        private Slider _sliderCurrentWeightModel;
        public const string SliderCurrentWeightModelPropertyName = "SliderCurrentWeightModel";

        public Slider SliderCurrentWeightModel
        {
            get { return _sliderCurrentWeightModel; }
            set { SetProperty(ref _sliderCurrentWeightModel, value, SliderCurrentWeightModelPropertyName); }
        }

        private double currentWeightValue = 0;
        public const string CurrentWeightValuePropertyName = "CurrentWeightValue";

        public double CurrentWeightValue
        {
            get { return currentWeightValue; }
            set { SetProperty(ref currentWeightValue, value, CurrentWeightValuePropertyName); }
        }

        private double currentWeightMaximumValue = 0;
        public const string CurrentWeightMaximumValuePropertyName = "CurrentWeightMaximumValue";

        public double CurrentWeightMaximumValue
        {
            get { return currentWeightMaximumValue; }
            set { SetProperty(ref currentWeightMaximumValue, value, CurrentWeightMaximumValuePropertyName); }
        }

        private double currentWeightMinimumValue = 0;
        public const string CurrentWeightMinimumValuePropertyName = "CurrentWeightMinimumValue";

        public double CurrentWeightMinimumValue
        {
            get { return currentWeightMinimumValue; }
            set { SetProperty(ref currentWeightMinimumValue, value, CurrentWeightMinimumValuePropertyName); }
        }


        private Slider _sliderWeightLossGoalModel;
        public const string SliderWeightLossGoalModelPropertyName = "SliderWeightLossGoalModel";

        public Slider SliderWeightLossGoalModel
        {
            get { return _sliderWeightLossGoalModel; }
            set { SetProperty(ref _sliderWeightLossGoalModel, value, SliderWeightLossGoalModelPropertyName); }
        }

        private double weightLossGoalValue = 0;
        public const string WeightLossGoalValuePropertyName = "WeightLossGoalValue";

        public double WeightLossGoalValue
        {
            get { return weightLossGoalValue; }
            set { SetProperty(ref weightLossGoalValue, value, WeightLossGoalValuePropertyName); }
        }

        private double weightLossGoalMaximumValue = 0;
        public const string WeightLossGoalMaximumValuePropertyName = "WeightLossGoalMaximumValue";

        public double WeightLossGoalMaximumValue
        {
            get { return weightLossGoalMaximumValue; }
            set { SetProperty(ref weightLossGoalMaximumValue, value, WeightLossGoalMaximumValuePropertyName); }
        }

        private double weightLossGoalMinimumValue = 0;
        public const string WeightLossGoalMinimumValuePropertyName = "WeightLossGoalMinimumValue";

        public double WeightLossGoalMinimumValue
        {
            get { return weightLossGoalMinimumValue; }
            set { SetProperty(ref weightLossGoalMinimumValue, value, WeightLossGoalMinimumValuePropertyName); }
        }
    }
}