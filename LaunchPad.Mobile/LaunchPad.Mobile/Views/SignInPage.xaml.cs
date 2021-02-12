using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : AnimationPage
    {
        public SignInPage()
        {
            InitializeComponent();
            this.PinEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.PinEntry.Unfocused += (s, e) => { SetLayoutPosition(onFocus: false); };
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        void SetLayoutPosition(bool onFocus)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.CenteredStackLayout.TranslateTo(0, -50, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.CenteredStackLayout.TranslateTo(0, -50, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.CenteredStackLayout.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.CenteredStackLayout.TranslateTo(0, 0, 50);
                }
            }
        }
        private void searchuserlist(object sender, System.EventArgs e)
        {
            UserListDropdownContainer.IsVisible = !UserListDropdownContainer.IsVisible;
            
        }

        private void CustomEntry_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {

        }

        private void itemselected(object sender, System.EventArgs e)
        {
            try
            {
                UserListDropdownContainer.IsVisible = false;
                var senderElement = sender as Grid;
                if (senderElement != null)
                {
                    var param = (e as TappedEventArgs)?.Parameter as CustomTherapist;
                    if (param != null)
                    {
                        param.SelectCommand.Execute(param.Therapist);
                    }
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}