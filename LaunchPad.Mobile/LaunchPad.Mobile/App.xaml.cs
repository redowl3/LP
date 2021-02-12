using FormsControls.Base;
using LaunchPad.Mobile.ViewModels;
using LaunchPad.Mobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
[assembly: ExportFont("UniNeueBook.ttf", Alias = "BoldFont")]
[assembly: ExportFont("UniNeueRegular.ttf", Alias = "RegularFont")]
namespace LaunchPad.Mobile
{
    public partial class App : Application
    {
        public static string UserName { get; set; }
        public static string SalonName { get; set; }
        public static Page CurrentPage { get; set; }
        public static Page NextPage { get; set; }
        public static SurveyPageViewModel surveyPageViewModelInstance { get; set; }
        public static ConcernsAndSkinCareSurveyViewModel ConcernsAndSkinCareSurveyViewModel { get; set; }
        public static HealthQuestionsSurveyViewModel HealthQuestionsSurveyViewModel { get; set; }
        public static LifestylesSurveyViewModel LifestylesSurveyViewModel { get; set; }
        public static AnimationNavigationPage SurveyPageInstance { get; set; }
        public App()
        {
            InitializeComponent();
            CurrentPage = new AnimationNavigationPage(new SplashPage());
            MainPage = CurrentPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
