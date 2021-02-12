using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace LaunchPad.Mobile.Converters
{
    public class CollectionToBasisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new FlexBasis(1f,true);
            else
            {
                ICollection list = value as ICollection;
                if (list != null)
                {
                    if (list.Count == 0)
                        return new FlexBasis(1f, true);
                    else
                        return new FlexBasis(0.25f, true);
                }
                else
                    return new FlexBasis(1f, true);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TextToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 220;
            else
            {
                var text = value.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Length < 20)
                        return 230;
                    else if (text.Length <25)
                        return 250; 
                    else if (25<text.Length && text.Length < 30)
                        return 300;
                   else return 400;
                }
                else
                    return 230;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
