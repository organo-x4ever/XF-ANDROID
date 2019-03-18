using com.organo.x4ever.Localization;
using com.organo.x4ever.ViewModels.Base;
using System;
using com.organo.x4ever.Converters;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Account
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
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MAXIMUM_CURRENT_WEIGHT);
            CurrentWeightValue = CurrentWeightMinimumValue;

            WeightLossGoalMinimumValue = 0;
            WeightLossGoalMaximumValue =
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MAXIMUM_WEIGHT_LOSE);
            WeightLossGoalValue = WeightLossGoalMinimumValue;
        }

        public string YourCurrentWeightText => string.Format(TextResources.YourCurrentWeightFormat1,
            App.Configuration.AppConfig.DefaultWeightVolume);

        public string WeightLossGoalMinText => string.Format(TextResources.WeightLossGoalMinFormat2,
            _converter.DisplayWeightVolume((double)App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE),
            App.Configuration.AppConfig.DefaultWeightVolume);

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