using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return CommonConstants.NOVALUE.ToCapital();
            var returnValue = "";
            var par = parameter?.ToString().ToLower();
            switch (par)
            {
                case "yesno":
                    if ((bool) value)
                        returnValue = CommonConstants.YES;
                    else
                        returnValue = CommonConstants.NO;
                    break;
                case "onoff":
                    if ((bool) value)
                        returnValue = CommonConstants.ON;
                    else
                        returnValue = CommonConstants.OFF;
                    break;
                case "enabledisable":
                    if ((bool) value)
                        returnValue = CommonConstants.ENABLE;
                    else
                        returnValue = CommonConstants.DISABLE;
                    break;
                default:
                    returnValue = CommonConstants.NOVALUE;
                    break;
            }

            return returnValue.ToCapital();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}