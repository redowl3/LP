using Android.Content;
using Android.Net.Wifi;
using LaunchPad.Mobile.Droid.CustomServices;
using LaunchPad.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(WifiUtility))]
namespace LaunchPad.Mobile.Droid.CustomServices
{
    public class WifiUtility : IWifi
    {
        private Context context = null;
      
        public WifiUtility()
        {
            this.context = Android.App.Application.Context;
        }

        [System.Obsolete]
        public async Task<IEnumerable<string>> GetAvailableNetworksAsync()
        {
            IEnumerable<string> availableNetworks = null;

            // Get a handle to the Wifi
            var wifiMgr = (WifiManager)context.GetSystemService(Context.WifiService);
            var wifiReceiver = new WifiReceiver(wifiMgr);

            await Task.Run(() =>
            {
                // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
                context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                availableNetworks = wifiReceiver.Scan();
            });
            if (availableNetworks.Count() > 0)
            {
                //App.ListChanged?.Invoke(availableNetworks.ToList());
            }
            return availableNetworks;
        }


        [BroadcastReceiver(Enabled = true, Exported = false)]
        class WifiReceiver : BroadcastReceiver
        {
            private WifiManager wifi;
            private List<string> wifiNetworks;
            private AutoResetEvent receiverARE;
            private Timer tmr;
            private const int TIMEOUT_MILLIS = 20000; // 20 seconds timeout

            public WifiReceiver()
            {

            }
            public WifiReceiver(WifiManager wifi)
            {
                this.wifi = wifi;
                wifiNetworks = new List<string>();
                receiverARE = new AutoResetEvent(false);
            }

            [System.Obsolete]
            public IEnumerable<string> Scan()
            {
                tmr = new Timer(Timeout, null, TIMEOUT_MILLIS, System.Threading.Timeout.Infinite);
                wifi.StartScan();
                receiverARE.WaitOne();
                return wifiNetworks;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    wifiNetworks.Add(wifinetwork.Ssid);
                }

                receiverARE.Set();
            }

            private void Timeout(object sender) =>
                // NOTE release scan, which we are using now, or we throw an error?
                receiverARE.Set();
        }
    }
}