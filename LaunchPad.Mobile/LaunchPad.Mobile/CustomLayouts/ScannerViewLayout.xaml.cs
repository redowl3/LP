using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerViewLayout : ContentView
    {
        public ScannerViewLayout()
        {
            InitializeComponent();
            var myScannerView = new ZXingScannerView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 300
            };

            myScannerView.SetBinding(ZXingScannerView.ScanResultCommandProperty, "ScanResultCommand");
            myScannerView.SetBinding(ZXingScannerView.IsScanningProperty, "IsScanning");
            myScannerView.SetBinding(ZXingScannerView.IsVisibleProperty, "IsScanning");

            ScannerGrid.Children.Add(myScannerView);
        }
    }
}