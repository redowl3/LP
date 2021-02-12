
using LaunchPad.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeclarationView : ContentView
    {
        public DeclarationView()
        {
            InitializeComponent();
            DeclarationTextLabel.Text = Constants.DeclarationText;
        }

        private void Check_Checked(object sender, System.EventArgs e)
        {
            SaveButton.IsEnabled = CheckButton.IsChecked;
            if (SaveButton.IsEnabled)
            {
                SaveButton.BackgroundColor = Color.Black;
                SaveButton.Clicked += SaveButton_Clicked;
            }
        }
        private void SaveButton_Clicked(object sender, System.EventArgs e)
        {
            (this.BindingContext as ClientRegistrationPageViewModel)?.DeclarationAcceptedCommand.Execute(null);
        }
    }
}