using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalonClientsPage : AnimationPage
    {
        public SalonClientsPage()
        {
            InitializeComponent();
        }

        public SalonClientsPage(PageAnimation pageAnimation)
        {
            PageAnimation = pageAnimation;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void item_tapped(object sender, System.EventArgs e)
        {
            try
            {               
                var param = (e as TappedEventArgs)?.Parameter as Consumer;
                if (param != null)
                {
                    (this.BindingContext as SalonClientsPageViewModel)?.SelectConsumerCommand.Execute(param);
                }
            }
            catch (System.Exception)
            {

            }
        }

     
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (this.BindingContext as SalonClientsPageViewModel)?.RefreshCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}