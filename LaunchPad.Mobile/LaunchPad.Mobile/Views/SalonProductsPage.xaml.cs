using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalonProductsPage : AnimationPage
    {
        public List<Product> Products = new List<Product>();
        public SalonProductsPage(PageAnimation pageAnimation)
        {
            this.PageAnimation = pageAnimation;
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            SalonProductsPageViewModel.CartItemAdded += AddOrUpdateBadge;
            this.BindingContext = new SalonProductsPageViewModel();
        }
        public SalonProductsPage()
        {
            this.PageAnimation = new PageAnimation
            {
                Type = AnimationType.Push,
                Duration = AnimationDuration.Medium,
                Subtype = AnimationSubtype.FromRight
            };
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            SalonProductsPageViewModel.CartItemAdded += AddOrUpdateBadge;
            this.BindingContext = new SalonProductsPageViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            (this.BindingContext as SalonProductsPageViewModel)?.RefreshBadgeCountAsync();
        }

        private void AddOrUpdateBadge(int obj)
        {
            if (ToolbarItems.Count > 0)
                DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), $"{obj}", Color.White, Color.Black);
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}