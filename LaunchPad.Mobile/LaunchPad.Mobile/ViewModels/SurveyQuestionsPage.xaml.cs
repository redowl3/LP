using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurveyQuestionsPage : TabbedPage
    {
        public SurveyQuestionsPage()
        {
            InitializeComponent();
        }
    }
}