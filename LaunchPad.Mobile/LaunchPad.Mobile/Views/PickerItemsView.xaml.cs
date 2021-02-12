using LaunchPad.Mobile.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickerItemsView : ContentPage
    {
        public static Action<string> ItemSelected;
        public static void OnItemSelected(string name)
        {
            ItemSelected?.Invoke(name);
        }
        public PickerItemsView()
        {
            InitializeComponent();
        }

        private void CloseThis(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void itemselected(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
            var param =(e as TappedEventArgs).Parameter  as CustomVariant;
            if (param != null)
            {
                param.IsSelected = true;
                ItemSelected?.Invoke(param.Variant.Name);
            }
        }
    }
}