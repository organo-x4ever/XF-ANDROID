using System;
using System.Globalization;
using Xamarin.Forms;

namespace com.organo.xchallenge.Converters
{
    public class PoundToKiligramConverter : IValueConverter
    {
        private const double ConversionValue = 0.45359237;

        //2.20462262
        private double _pound, _kg;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _kg = _pound = 0;
            try
            {
                if (value != null && double.TryParse(value.ToString(), out _pound))
                    _kg = _pound * ConversionValue;
            }
            catch (Exception)
            {
                return 0;
            }

            double.TryParse(_kg.ToString("0"), out double v);
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _pound = _kg = 0;
            try
            {
                if (value != null && double.TryParse(value.ToString(), out _kg))
                    _pound = _kg / ConversionValue;
            }
            catch (Exception)
            {
                return 0;
            }

            double.TryParse(_pound.ToString("0"), out double v);
            return v;
        }

        public double StorageWeightVolume(double weightKg, double weightLb)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return weightLb;
            return weightKg;
        }

        public double StorageWeightVolume(string weightKg, string weightLb)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb") &&
                double.TryParse(weightLb, out double val))
                return val;
            return double.TryParse(weightKg, out double kg) ? kg : 0;
        }

        public double DisplayWeightVolume(double weightKg, double weightLb)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return weightLb;
            return weightKg;
        }

        public double DisplayWeightVolume(string weightKg, string weightLb)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb") &&
                double.TryParse(weightLb, out double val))
                return val;
            return double.TryParse(weightKg, out double kg) ? kg : 0;
        }
        
        public double StorageWeightVolume(double weight)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return (double)Convert(weight, typeof(double), null, App.Configuration.LanguageInfo);
            else
            {
                double.TryParse(weight.ToString("0"), out double v);
                return v;
            }
        }

        public double StorageWeightVolume(string weight)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return (double)Convert(weight, typeof(double), null, App.Configuration.LanguageInfo);
            else
            {
                double.TryParse(weight, out double w);
                double.TryParse(w.ToString("0"), out double v);
                return v;
            }
        }

        public double DisplayWeightVolume(double weight)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return (double)ConvertBack(weight, typeof(double), null, App.Configuration.LanguageInfo);
            else
            {
                double.TryParse(weight.ToString("0"), out double v);
                return v;
            }
        }

        public double DisplayWeightVolume(string weight)
        {
            if (App.Configuration.AppConfig.DefaultWeightVolume.Contains("lb"))
                return (double)ConvertBack(weight, typeof(double), null, App.Configuration.LanguageInfo);
            else
            {
                double.TryParse(weight, out double w);
                double.TryParse(w.ToString("0"), out double v);
                return v;
            }
        }

    }
}