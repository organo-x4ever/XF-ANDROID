using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Converters
{
    public class SplitStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (value as string);
            var files = data.Split('/');
            if (int.TryParse(parameter as string, out int param))
                if (param == 9)
                    return files[files.Length - 1];
                else
                    return files[param];
            else
                return files[files.Length - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}