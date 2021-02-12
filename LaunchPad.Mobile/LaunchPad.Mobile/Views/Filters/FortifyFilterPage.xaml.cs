using LaunchPad.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaunchPad.Mobile.Views.Filters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FortifyFilterPage : ContentPage
    {
        public FortifyFilterPage(SalonProductsPageViewModel userHealthPlanPageViewModel)
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