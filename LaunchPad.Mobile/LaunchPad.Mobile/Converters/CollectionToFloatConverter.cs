using LaunchPad.Mobile.ViewModels;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;

namespace LaunchPad.Mobile.Converters
{
    public class CollectionToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            var collection = value as ObservableCollection<IndexedQuestions>;
            if (collection.Count == 3)
            {
                return new FlexBasis(0.3f, true);
            }
            else if (collection.Count == 2)
            {
                return new FlexBasis(0.5f, true);
            }
            else
            {
                return new FlexBasis(1f, true);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
