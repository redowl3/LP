using FormsControls.Base;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRegistrationPage : AnimationPage
    {
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public ClientRegistrationPage()
        {
            InitializeComponent();
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var screenWidth = mainDisplayInfo.Width;
            if (screenWidth <= 2050)
            {
                MidDobContainerGrid.ColumnSpacing = 20;
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(350) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            }
            else
            {
                MidDobContainerGrid.ColumnSpacing = 30;
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(350) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                //MidDobContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            }
            this.FirstnameEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.FirstnameEntry.Unfocused += (s, e) =>
            { 
                if (!string.IsNullOrEmpty(FirstnameEntry.Text))
                {
                    this.FirstnameEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.LastnameEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.LastnameEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(LastnameEntry.Text))
                {
                    this.LastnameEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.EmailEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.EmailEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(EmailEntry.Text) && EmailRegex.IsMatch(EmailEntry.Text))
                {
                    this.EmailEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.ConfirmEmailEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.ConfirmEmailEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(ConfirmEmailEntry.Text) && (ConfirmEmailEntry.Text == EmailEntry.Text))
                {
                    this.ConfirmEmailEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.DDEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.DDEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(DDEntry.Text))
                {
                    this.DDEntry.BackgroundColor = Color.FromHex("#fff");
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                }
                SetLayoutPosition(onFocus: false);
            };
            this.MMEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.MMEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(MMEntry.Text))
                {
                    this.MMEntry.BackgroundColor = Color.FromHex("#fff");
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                }
                SetLayoutPosition(onFocus: false);
            };
            this.YYEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.YYEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(YYEntry.Text) && YYEntry.Text.Length==4 && !string.IsNullOrEmpty(MMEntry.Text) && !string.IsNullOrEmpty(DDEntry.Text))
                {
                    this.YYEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.MobileEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.MobileEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(MobileEntry.Text) && MobileEntry.Text.Length <= 15)
                {
                    this.MobileEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.PostcodeEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.PostcodeEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(PostcodeEntry.Text))
                {
                    this.PostcodeEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.AddressLine1Entry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.AddressLine1Entry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(AddressLine1Entry.Text))
                {
                    this.AddressLine1Entry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.AddressLine2Entry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.AddressLine2Entry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(AddressLine2Entry.Text))
                {
                    this.AddressLine2Entry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.AddressLine3Entry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.AddressLine3Entry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(AddressLine3Entry.Text))
                {
                    this.AddressLine3Entry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.CityEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.CityEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(CityEntry.Text))
                {
                    this.CityEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
            this.CountyEntry.Focused += (s, e) => { SetLayoutPosition(onFocus: true); };
            this.CountyEntry.Unfocused += (s, e) =>
            {
                if (!string.IsNullOrEmpty(CountyEntry.Text))
                {
                    this.CountyEntry.BackgroundColor = Color.FromHex("#fff");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = true;
                }
                else
                {
                    (s as Entry).BackgroundColor = Color.FromHex("#f2f2f2");
                    (((s as Entry)?.Parent as Grid)?.Children[2] as Image).IsVisible = false;
                }
                SetLayoutPosition(onFocus: false);
            };
        }
        void SetLayoutPosition(bool onFocus)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.CenteredStackLayout.TranslateTo(0, -100, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.CenteredStackLayout.TranslateTo(0, -100, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.CenteredStackLayout.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.CenteredStackLayout.TranslateTo(0, 0, 50);
                }
            }
        }
    }
}