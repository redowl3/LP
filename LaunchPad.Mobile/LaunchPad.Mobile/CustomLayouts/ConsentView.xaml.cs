using LaunchPad.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsentView : ContentView
    {
        public ConsentView()
        {
            InitializeComponent();
        }

        private void CheckBox1_CheckChanged(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = Check1.IsChecked && Check2.IsChecked && Check3.IsChecked && Check4.IsChecked && Check5.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void CheckBox2_CheckChanged(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = Check1.IsChecked && Check2.IsChecked && Check3.IsChecked && Check4.IsChecked && Check5.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void CheckBox3_CheckChanged(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = Check1.IsChecked && Check2.IsChecked && Check3.IsChecked && Check4.IsChecked && Check5.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void CheckBox4_CheckChanged(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = Check1.IsChecked && Check2.IsChecked && Check3.IsChecked && Check4.IsChecked && Check5.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void CheckBox5_CheckChanged(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = Check1.IsChecked && Check2.IsChecked && Check3.IsChecked && Check4.IsChecked && Check5.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void SaveButton_Clicked(object sender, System.EventArgs e)
        {
            (this.BindingContext as ClientRegistrationPageViewModel)?.ConsentsAcceptedCommand.Execute(null);
        }
    }
}