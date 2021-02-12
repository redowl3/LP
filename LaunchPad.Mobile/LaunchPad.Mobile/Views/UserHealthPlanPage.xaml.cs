
using FormsControls.Base;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserHealthPlanPage : AnimationPage
    {
        private bool isExpanded;
        public bool ShouldAnimateOut { get; set; }
        public UserHealthPlanPage()
        {
            InitializeComponent();
            isExpanded = true;
            ShouldAnimateOut = true;
            UserHealthPlanPageViewModel.BadgeCountAction += AddOrUpdateBadge;
            UserHealthPlanPageViewModel.CloseDrawer += CloseDrawer;
            UserHealthPlanPageViewModel.ShouldAnimateOut += SetShouldAnimateOut;
            HealthPlanHeader.Text = $"{Settings.ClientFirstName}'s Optimum Skin Health Plan";
        }

        private void CloseDrawer()
        {
            AnimateOut();
            isExpanded = false;
        }

        private void SetShouldAnimateOut(bool obj)
        {
            ShouldAnimateOut = obj;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            var count=await (this.BindingContext as UserHealthPlanPageViewModel)?.RefreshBadgeCountAsync();
            if (ToolbarItems.Count > 0)
                DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), $"{count}", Color.White, Color.Black);
        }
        private void AddOrUpdateBadge(int obj)
        {
            if (ToolbarItems.Count > 0)
                DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), $"{obj}", Color.White, Color.Black);
        }
        void Handle_Tapped(object sender, System.EventArgs e)
        {
            if (!isExpanded)
                AnimateIn();
            else
                AnimateOut();

            isExpanded = !isExpanded;
        }
        private void AnimateIn()
        {
            BasketView.IsVisible = true;
            var rect = new Rectangle(-(GridLayout.Width - BasketView.Width), BasketView.Y, BasketView.Width, BasketView.Height);
            BasketView.LayoutTo(rect, 400, Easing.Linear);
        }
        private async void AnimateOut()
        {
            var rect = new Rectangle(GridLayout.Width, BasketView.Y, BasketView.Width, BasketView.Height);
            await BasketView.LayoutTo(rect, 400, Easing.Linear);

            BasketView.IsVisible = false;
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (isExpanded && ShouldAnimateOut)
            {
                AnimateOut();
                isExpanded = false;
            }
        }

        private void OpenBasketView(object sender, System.EventArgs e)
        {
            AnimateIn();
            isExpanded = true;
        }

        private void CloseBasketView(object sender, System.EventArgs e)
        {
            AnimateOut();
            isExpanded = false;
        }

        private void GoBack(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}