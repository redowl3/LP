using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LaunchPad.Mobile.Converters
{
    public class TextToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            else
            {
                if (value.ToString().ToLower() == "very high" || value.ToString().ToLower() == "8 hrs >")
                {
                    return System.Convert.ToDouble("120");
                }
                if (value.ToString().ToLower() == "high" || value.ToString().ToLower() == "7 hrs")
                {
                    return System.Convert.ToDouble("100");
                }
                if (value.ToString().ToLower() == "moderate" || value.ToString().ToLower() == "6 hrs")
                {
                    return System.Convert.ToDouble("80");
                }
                if (value.ToString().ToLower() == "low" || value.ToString().ToLower() == "5 hrs")
                {
                    return System.Convert.ToDouble("60");
                }
                if (value.ToString().ToLower() == "very low" || value.ToString().ToLower() == "< 4 hrs")
                {
                    return System.Convert.ToDouble("40");
                }
                else
                {
                    return 0;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    
    public class SecondaryTextToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            else
            {
                if (value.ToString().ToLower() == "very high" || value.ToString().ToLower() == "8 hrs >")
                {
                    return System.Convert.ToDouble("60");
                }
                if (value.ToString().ToLower() == "high" || value.ToString().ToLower() == "7 hrs")
                {
                    return System.Convert.ToDouble("45");
                }
                if (value.ToString().ToLower() == "moderate" || value.ToString().ToLower() == "6 hrs")
                {
                    return System.Convert.ToDouble("30");
                }
                if (value.ToString().ToLower() == "low" || value.ToString().ToLower() == "5 hrs")
                {
                    return System.Convert.ToDouble("15");
                }
                if (value.ToString().ToLower() == "very low" || value.ToString().ToLower() == "< 4 hrs")
                {
                    return System.Convert.ToDouble("0");
                }
                else
                {
                    return 0;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
