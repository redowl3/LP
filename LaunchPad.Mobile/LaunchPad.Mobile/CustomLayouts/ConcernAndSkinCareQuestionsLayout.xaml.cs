using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConcernAndSkinCareQuestionsLayout : ContentView
    {
        public ConcernAndSkinCareQuestionsLayout()
        {
            InitializeComponent();
        }

        private void ToggleContainer(object sender, EventArgs e)
        {            
          
            OptionContainer.IsVisible = !OptionContainer.IsVisible;
            if (OptionContainer.IsVisible)
            {
                ((Grid)sender).BackgroundColor = Color.FromHex("#000");
            }
            else
            {
                ((Grid)sender).BackgroundColor = Color.Transparent;
            }
        }
    }
}