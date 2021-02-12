using LaunchPad.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaunchPad.Mobile.Views.Filters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedFilterPage : ContentPage
    {
        public FeedFilterPage(SalonProductsPageViewModel userHealthPlanPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = userHealthPlanPageViewModel;
        }

        private void CloseThis(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}