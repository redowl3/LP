using FFImageLoading.Forms;
using FormsControls.Base;
using LaunchPad.Mobile.CustomLayouts;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Models;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.ViewModels;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedHealthPlanPage : AnimationPage
    {      
        public CompletedHealthPlanPage()
        {
            InitializeComponent();
            CompletedHealthPlanPageViewModel.BadgeCountAction += AddOrUpdateBadge;
            CompleteHealthPlanHeaderLabel.Text = $"{Settings.ClientFirstName}'s optimum skin health plan is now complete";
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var screenWidth = mainDisplayInfo.Width;
            if (screenWidth <= 2050)
            {
                CompleteHealthPlanHeaderLabel.FontSize = 32;
                ToScanMessageLabel.FontSize = 26;
            }
            else
            {
                CompleteHealthPlanHeaderLabel.FontSize = 40;
                ToScanMessageLabel.FontSize = 32;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new CompletedHealthPlanPageViewModel();
        }
        private void AddOrUpdateBadge(int obj)
        {
            if (ToolbarItems.Count > 0)
                DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), $"{obj}", Color.White, Color.Black);
        }

        private void scan_event(object sender, System.EventArgs e)
        {
            try
            {
                var senderElement = sender as CachedImage;
                if (senderElement != null)
                {
                    var subParent = senderElement.Parent as Grid;
                    if (subParent != null)
                    {
                        var parent = subParent.Parent as Grid;
                        if (parent == null) return;
                        var scannerStack = parent.Children[1] as StackLayout;
                        scannerStack?.Children?.Clear();
                        scannerStack.Children.Add(new ScannerViewLayout());
                        var param = (e as TappedEventArgs).Parameter as CompletedHealthPlan;
                        if (param != null)
                        {
                            param.ScanCommand.Execute(param.HealthPlanToComplete.Product);
                        }
                    }
                }
            }
            catch (System.Exception)
            {

            }
           
        }
        private void remove_loyalty(object sender, System.EventArgs e)
        {
            try
            {
                var senderElement = sender as Button;
                if (senderElement != null)
                {
                    var parent = senderElement.Parent as Grid;
                    if (parent != null)
                    {
                        var topParent = parent.Parent as Grid;
                        if (topParent!=null)
                        {
                            var scannerStack = topParent.Children[1] as StackLayout;
                            scannerStack?.Children?.Clear();
                            var param = senderElement.CommandParameter as CompletedHealthPlan;
                            if (param != null)
                            {
                                param.RemoveCommand.Execute(param.HealthPlanToComplete.Product);
                            }
                        }
                        
                    }
                }
              
            }
            catch (System.Exception)
            {

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.BindingContext = null;
        }
        private void GoBack(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}