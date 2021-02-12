using LaunchPad.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace LaunchPad.Mobile.ViewModels
{
    public class NetworkConfigPageViewModel : ViewModelBase
    {
        private IEnumerable<string> _wifiService;
        private ObservableCollection<string> _wifiList;
        public ObservableCollection<string> WifiList
        {
            get => _wifiList;
            set => SetProperty(ref _wifiList, value);
        }
        public NetworkConfigPageViewModel()
        {
            GetWifiNetworks();
            //App.ListChanged += UpdateNetworkList;
            WifiList = new ObservableCollection<string>();
        }

        private void UpdateNetworkList(List<string> obj)
        {
            if (WifiList.Count == 0)
            {
                WifiList = new ObservableCollection<string>(obj);
            }
            else
            {
                foreach (var item in obj)
                {
                    if (WifiList.Count(a => a.ToLower() == item.ToLower())==0)
                    {
                        WifiList.Add(item);
                    }
                }
               
            }
           
        }

        private void GetWifiNetworks()
        {
            Device.StartTimer(new TimeSpan(0, 0, 15), () =>
            {
                Device.BeginInvokeOnMainThread(() => ExceptionHandler(async () =>
                {
                    _wifiService = null;
                    _wifiService = await DependencyService.Get<IWifi>().GetAvailableNetworksAsync();
                }));

                return true; // runs again, or false to stop
            });

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
