using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Client;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Models;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaunchPad.Mobile.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        public static Action<string> LoadLoggedInUserDetail;
        public static void OnLoadLoggedInUserDetail(string param)
        {
            LoadLoggedInUserDetail?.Invoke(param);
        }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private Salon Salon = new Salon();
        private ObservableCollection<CustomTherapist> _therapists;
        public ObservableCollection<CustomTherapist> Therapists
        {
            get => _therapists;
            set => SetProperty(ref _therapists, value);
        }

        private Therapist SelectedTherapist = null;
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand ContinueCommand => new Command(() => ExceptionHandler(() => LoginAsync()));
        public ICommand ForgotPasswordCommand => new Command(() => ExceptionHandler(() => ForgotPasswordAsync()));

        public SignInPageViewModel()
        {
            Therapists = new ObservableCollection<CustomTherapist>();
            GetTherapistsAsync();
        }

        private void GetTherapistsAsync()
        {
            Task.Run(() => ExceptionHandler(async () =>
            {
                if (Salon == null || Salon.Id == Guid.Empty)
                {
                    Salon = await DatabaseServices.Get<Salon>("salon");
                    if (Salon == null || Salon.Id == Guid.Empty)
                    {
                        Salon = await ApiServices.Client.GetAsync<Salon>("salon");
                        await DatabaseServices.InsertData("salon", Salon);
                    }

                    if (Salon.Therapists?.Count > 0)
                        Therapists = new ObservableCollection<CustomTherapist>(Salon.Therapists.Select(a => new CustomTherapist
                        {
                            Therapist = a,
                            SelectCommand = new Command<Therapist>((param) =>
                            {
                                Username = param.Username;
                                SelectedTherapist = param;
                            })
                        }));
                }
            }));
        }

        private void LoginAsync()
        {
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(() =>
            {
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    if (SelectedTherapist.Username == Username && SelectedTherapist.PasswordHash == Password)
                    {
                        var jsonString = JsonConvert.SerializeObject(SelectedTherapist);
                        SecureStorage.SetAsync("currentTherapist", jsonString);
                        SecureStorage.SetAsync("currentUserName", $"{SelectedTherapist.Firstname} {SelectedTherapist.Surname}");
                        SecureStorage.SetAsync("currentUserImage", SelectedTherapist.ImageUrl);
                        Settings.CurrentTherapistId = SelectedTherapist.Id.ToString();
                        App.UserName = $"{SelectedTherapist.Firstname}  {SelectedTherapist.Surname}";
                        Settings.CurrentUserName = $"{SelectedTherapist.Firstname}  {SelectedTherapist.Surname}";
                        Application.Current.MainPage=new AnimationNavigationPage(new SalonClientsPage());
                    }
                }
            }));
        }
        private void ForgotPasswordAsync()
        {
            Task.Run(() => ExceptionHandler(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ResetPasswordPage());
                });
            }));
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
