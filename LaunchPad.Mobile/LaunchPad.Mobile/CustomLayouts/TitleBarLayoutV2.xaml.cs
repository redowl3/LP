using LaunchPad.Mobile.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitleBarLayoutV2 : ContentView
    {
        public TitleBarLayoutV2()
        {
            InitializeComponent();
            LoggedInUserDetailLabel.Text = $"{Settings.CurrentUserName} | {Settings.SalonName}";
            CurrentClientName.Text = Settings.ClientName;
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var screenWidth = mainDisplayInfo.Width;
            if (screenWidth <= 2050)
            {
                ContainerStack.Spacing = 20;
            }
            else
            {
                ContainerStack.Spacing = 35;
            }
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}