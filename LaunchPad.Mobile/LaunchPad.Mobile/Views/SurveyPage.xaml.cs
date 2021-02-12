using FormsControls.Base;
using LaunchPad.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurveyPage : AnimationPage
    {
        private Color ActiveColor = Color.FromHex("#c7c9cb");
        private Color DisableColor = Color.FromHex("#f4f4f5");
        public SurveyPage()
        {
            InitializeComponent();
            ConcernPageSurveyContainer.IsVisible = false;
            HealthQuestionSurveyContainer.IsVisible = true;
            LifestylesSurveyContainer.IsVisible = false;
            //SurveyPageViewModel.AddChildren += AddChildrenLayout;
            //Tabcomponent.SelectedIndex = 1;
            ConcernsAndSkinCareSurveyViewModel.Next += NextToConcernsPage;
            HealthQuestionsSurveyViewModel.Next += NextToHealthPage;
            SurveyPageViewModel.GoBack += GoBack;
        }

        private void GoBack()
        {
            if (LifestylesSurveyContainer.IsVisible)
            {
                ConcernsButtonClicked(null, null);
            }else if (ConcernPageSurveyContainer.IsVisible)
            {
                HealthButtonClicked(null, null);
            }
        }

        private void NextToConcernsPage()
        {
            LifeStyleButtonClicked(null, null);
        }
        private void NextToHealthPage()
        {
            ConcernsButtonClicked(null, null);
        }

        private void AddChildrenLayout(string obj)
        {
            //if (obj.ToLower() == "concernpage")
            //{
            //    MainContainer.Children?.Clear();
            //    MainContainer.Children.Add(new ConcernsQuestionContainerLayout());
            //}
            //if (obj.ToLower() == "healthquestionpage")
            //{
            //    MainContainer.Children?.Clear();
            //    MainContainer.Children.Add(new HealthQuestionsContainerLayout());
            //}
            //if (obj.ToLower() == "lifestylepage")
            //{
            //    MainContainer.Children?.Clear();
            //    MainContainer.Children.Add(new LifeStylesQuestionContainerLayout());
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // App.surveyPageViewModelInstance?.SelectSurveyAsync();
        }
        public SurveyPage(PageAnimation pageAnimation)
        {
            PageAnimation = pageAnimation;
            InitializeComponent();
        }

        private void ConcernsButtonClicked(object sender, System.EventArgs e)
        {
            if (ConcernPageSurveyContainer.IsVisible) return;
            ConcernPageSurveyContainer.IsVisible = true;
            HealthQuestionSurveyContainer.IsVisible = false;
            LifestylesSurveyContainer.IsVisible = false;
            ConcernSurveyBoxView.BackgroundColor = ActiveColor;
            HealthSurveyBoxView.BackgroundColor = LifeStyleSurveyBoxView.BackgroundColor = DisableColor;
        }
        private void HealthButtonClicked(object sender, System.EventArgs e)
        {
            if (HealthQuestionSurveyContainer.IsVisible) return;
            ConcernPageSurveyContainer.IsVisible = false;
            HealthQuestionSurveyContainer.IsVisible = true;
            LifestylesSurveyContainer.IsVisible = false;
            HealthSurveyBoxView.BackgroundColor = ActiveColor;
            ConcernSurveyBoxView.BackgroundColor = LifeStyleSurveyBoxView.BackgroundColor = DisableColor;
        }
        private void LifeStyleButtonClicked(object sender, System.EventArgs e)
        {
            if (LifestylesSurveyContainer.IsVisible) return;
            ConcernPageSurveyContainer.IsVisible = false;
            HealthQuestionSurveyContainer.IsVisible = false;
            LifestylesSurveyContainer.IsVisible = true;
            LifeStyleSurveyBoxView.BackgroundColor = ActiveColor;
            ConcernSurveyBoxView.BackgroundColor = HealthSurveyBoxView.BackgroundColor = DisableColor;
        }

        private void NextButtonClicked(object sender, System.EventArgs e)
        {
            if (ConcernPageSurveyContainer.IsVisible)
            {
                //(ConcernPageSurveyContainer.BindingContext as ConcernsAndSkinCareSurveyViewModel)?.NextCommand.Execute(null);
            }
            if (HealthQuestionSurveyContainer.IsVisible)
            {
                (HealthQuestionSurveyContainer.BindingContext as HealthQuestionsSurveyViewModel)?.NextCommand.Execute(null);
            }
            if (LifestylesSurveyContainer.IsVisible)
            {
                (LifestylesSurveyContainer.BindingContext as LifestylesSurveyViewModel)?.NextCommand.Execute(null);
            }
        }

        //private void Tab_TabClicked(object sender, Xam.TabView.OnTabClickedEventArgs args)
        //{
        //    tab3.BackgroundColor = Color.FromHex("#fff");
        //    tab2.BackgroundColor = Color.FromHex("#fff");
        //    tab1.BackgroundColor = Color.FromHex("#fff");
        //    Tabcomponent.XFTabPages.ForEach(a => {
        //        a.Header.BackgroundColor = Color.FromHex("#fff");
        //    });

        //}

        //private void tab3_XFTabHeader_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
        //{
        //    tab3.BackgroundColor = Color.FromHex("#fff");
        //}

        //private void tab2_Unfocused(object sender, FocusEventArgs e)
        //{
        //    tab2.BackgroundColor = Color.FromHex("#fff");
        //}

        //private void tab1_Unfocused(object sender, FocusEventArgs e)
        //{
        //    tab1.BackgroundColor = Color.FromHex("#fff");
        //}
    }
}