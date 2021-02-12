using Android.Widget;
using LaunchPad.Mobile.Droid.CustomServices;
using LaunchPad.Mobile.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly:Dependency(typeof(ToastServices))]
namespace LaunchPad.Mobile.Droid.CustomServices
{
    public class ToastServices : IToastServices
    {
        public void ShowToast(string message)
        {
            Toast.MakeText(CrossCurrentActivity.Current.Activity, message, ToastLength.Long).Show();
        }
    }
}