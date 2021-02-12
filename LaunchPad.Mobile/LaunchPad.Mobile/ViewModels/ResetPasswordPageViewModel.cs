using LaunchPad.Mobile.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaunchPad.Mobile.ViewModels
{
    public class ResetPasswordPageViewModel : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public ICommand ContinueCommand => new Command(() => ExceptionHandler(() => SendPasswordLinkIfInputValidAsync()));

        private void SendPasswordLinkIfInputValidAsync()
        {
            if (!string.IsNullOrEmpty(Email))
            {
            }
        }

        private void ExceptionHandler(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
