using LaunchPad.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace LaunchPad.Mobile.Converters
{
    public class ComparisionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var responseText = (string)values[0];
            var childQuestions = (List<CustomFormQuestion>)values[1];
            return new List<string>();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
