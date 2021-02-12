using LaunchPad.Mobile.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitleBarLayout : ContentView
    {
        public TitleBarLayout()
        {
            InitializeComponent();
            LoggedInUserDetailLabel.Text = $"{Settings.CurrentUserName} | {Settings.SalonName}";
            PageHeaderTitle.Text = Settings.ClientHeader;
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
    }
}