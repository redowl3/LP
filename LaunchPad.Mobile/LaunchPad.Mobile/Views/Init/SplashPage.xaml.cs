using FormsControls.Base;
using LaunchPad.Mobile.ViewModels;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : AnimationPage
    {
        int start = 0;
        int end = 1;
        public SplashPage()
        {
            InitializeComponent();
           // SplashPageViewModel.UpdateProgress += UpdateProgress;
        }

        private void UpdateProgress(int obj)
        {
            ProgressBar.IsVisible = true;
            ProgressBar.Progress =start+ (1-( obj / 100));
            //ProgressBar.ProgressTo(obj / 100, 2000, Easing.Linear);
        }
    }
}