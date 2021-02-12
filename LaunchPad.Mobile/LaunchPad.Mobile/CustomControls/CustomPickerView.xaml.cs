using LaunchPad.Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPickerView : ContentView
    {
        public CustomPickerView()
        {
            InitializeComponent();
            PickerItemsView.ItemSelected += UpdateSelectedItem;
        }

        private void UpdateSelectedItem(string obj)
        {
            SelectedItemLabel.Text = obj;
        }

        private void TogglePicker(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new PickerItemsView());
        }
    }
}